using HMSDigital.Common.BusinessLayer.Config;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.Data.Enums;
using HMSDigital.Common.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services
{
    public class AzureBlobStorageService : IFileStorageService
	{
		private readonly BlobStorageConfig _blobStorageConfig;

		public AzureBlobStorageService(IOptions<BlobStorageConfig> blobStorageOptions)
		{
			_blobStorageConfig = blobStorageOptions.Value;
		}

		/// <summary>
		/// Identifier for this storage type
		/// </summary>
		/// <returns></returns>
		public int GetStorageTypeId()
		{
			return (int)FileStorageTypes.AzureBlobStorage;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileMetadata"></param>
		/// <returns></returns>
		public string GetStorageRoot(FileMetadataRequest fileMetadata)
		{
			if (fileMetadata.GetType() == typeof(ItemImageFileRequest))
			{
				return _blobStorageConfig.Containers.ItemCatalog;
			}
			if (fileMetadata.GetType() == typeof(ItemUserManualFileRequest))
			{
				return _blobStorageConfig.Containers.ItemUserManual;
			}
			if (fileMetadata.GetType() == typeof(UserPictureFileRequest))
			{
				return _blobStorageConfig.Containers.UserPicture;
			}
			if(fileMetadata.GetType() == typeof(FhirFileRequest))
			{
				return _blobStorageConfig.Containers.FhirCatalog;
			}
			if (fileMetadata.GetType() == typeof(ReceiptImageFileRequest))
			{
				return _blobStorageConfig.Containers.ReceiptImages;
			}
			return _blobStorageConfig.Containers.CommonFiles;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileMetadata"></param>
		/// <returns></returns>
		public string GetStorageFilePath(FileMetadataRequest fileMetadata)
		{
			var fileExtension = fileMetadata.ContentType == "application/json" ? "json" : fileMetadata.ContentType;
			return $"{fileMetadata.Name}_{Guid.NewGuid().ToString("N")}.{fileExtension}";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileMetadata"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public async Task<Uri> GetUploadUrl(FileMetadataRequest fileMetadata, string filePath)
		{
			var storageAccount = CloudStorageAccount.Parse(_blobStorageConfig.ConnectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(GetStorageRoot(fileMetadata));
			await container.CreateIfNotExistsAsync();
			if (fileMetadata.IsPublic)
			{
				var permissions = await container.GetPermissionsAsync();
				if (permissions.PublicAccess != BlobContainerPublicAccessType.Blob)
				{
					permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
					await container.SetPermissionsAsync(permissions);
				}
			}
			var blob = container.GetBlockBlobReference(filePath);
			blob.Properties.ContentType = fileMetadata.ContentType;
			var sasConstraints = new SharedAccessBlobPolicy();
			sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(_blobStorageConfig.UploadUrlExpiresInMinutes);
			sasConstraints.Permissions = SharedAccessBlobPermissions.Write;
			return new Uri(blob.Uri, blob.GetSharedAccessSignature(sasConstraints));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="storageRoot"></param>
		/// <param name="storageFilePath"></param>
		/// <param name="durationInMinutes"></param>
		/// <returns></returns>
		public async Task<Uri> GetDownloadUrl(string storageRoot, string storageFilePath, int? mins = null)
		{
			var storageAccount = CloudStorageAccount.Parse(_blobStorageConfig.ConnectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(storageRoot);
			var blob = container.GetBlockBlobReference(storageFilePath);
			var permissions = await container.GetPermissionsAsync();
			if (permissions.PublicAccess == BlobContainerPublicAccessType.Blob)
			{
				// Publicly accessible blob, no SAS needed
				return blob.Uri;
			}
			var sasConstraints = new SharedAccessBlobPolicy();
			sasConstraints.SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(mins ?? _blobStorageConfig.DownloadUrlExpiresInMinutes);
			sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
			return new Uri(blob.Uri, blob.GetSharedAccessSignature(sasConstraints));
		}

		public async Task DeleteFile(string storageRoot, string storageFilePath)
		{
			var storageAccount = CloudStorageAccount.Parse(_blobStorageConfig.ConnectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(storageRoot);
			var blob = container.GetBlockBlobReference(storageFilePath);
			await blob.DeleteAsync();
		}

		/// <summary>
		/// Upload a file to storage
		/// </summary>
		/// <param name="storageFile"></param>
		/// <returns></returns>
		public async Task UploadFile(StorageFile storageFile)
        {
			var storageAccount = CloudStorageAccount.Parse(_blobStorageConfig.ConnectionString);
			var blobClient = storageAccount.CreateCloudBlobClient();
			var container = blobClient.GetContainerReference(GetStorageRoot(storageFile.FileMetadata));
			await container.CreateIfNotExistsAsync();
			if (storageFile.FileMetadata.IsPublic)
			{
				var permissions = await container.GetPermissionsAsync();
				if (permissions.PublicAccess != BlobContainerPublicAccessType.Blob)
				{
					permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
					await container.SetPermissionsAsync(permissions);
				}
			}

			if (storageFile.FileContent is MemoryStream)
			{
				storageFile.FileContent.Seek(0, SeekOrigin.Begin);
			}
			
			var blob = container.GetBlockBlobReference(storageFile.StorageFilePath);
			blob.Properties.ContentType = storageFile.FileMetadata.ContentType;
			await blob.UploadFromStreamAsync(storageFile.FileContent);
		}
    }
}
