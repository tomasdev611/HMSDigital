using HMSDigital.Common.ViewModels;
using System;
using System.Threading.Tasks;

namespace HMSDigital.Common.BusinessLayer.Services.Interfaces
{
    public interface IFileStorageService
    {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileMetadata"></param>
		/// <param name="filePath"></param>
		Task<Uri> GetUploadUrl(FileMetadataRequest fileMetadata, string filePath);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="storageRoot"></param>
		/// <param name="storageFilePath"></param>
		/// <param name="durationInMinutes"></param>
		/// <returns></returns>
		Task<Uri> GetDownloadUrl(string storageRoot, string storageFilePath, int? mins = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="storageRoot"></param>
		/// <param name="storageFilePath"></param>
		/// <param name="durationInMinutes"></param>
		/// <returns></returns>
		Task DeleteFile(string storageRoot, string storageFilePath);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		int GetStorageTypeId();
		
		/// <summary>
		/// This maps to root part of the storage location
		/// </summary>
		/// <returns></returns>
		string GetStorageRoot(FileMetadataRequest fileMetadata);
		
		/// <summary>
		/// This maps to the path relative to the root path
		/// </summary>
		/// <returns></returns>
		string GetStorageFilePath(FileMetadataRequest fileMetadata);
		
		/// <summary>
		/// Upload file to storage
		/// </summary>
		/// <returns></returns>
		Task UploadFile(StorageFile storageFile);
	}
}
