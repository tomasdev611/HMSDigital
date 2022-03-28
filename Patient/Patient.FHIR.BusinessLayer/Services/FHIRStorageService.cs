using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Patient.FHIR.BusinessLayer.Services
{
    public class FHIRStorageService : IFHIRStorageService
    {
        private readonly IFileStorageService _fileStorageService;

        public FHIRStorageService(IFileStorageService fileStorageService) 
        {
            _fileStorageService = fileStorageService;
        }

        public async Task UploadJsonFile(string jsonFileContent)
        {
            var fileContentByteArray = Encoding.UTF8.GetBytes(jsonFileContent);
            var fileContentStream = new MemoryStream(fileContentByteArray);

            var fileMetadata = new FhirFileRequest();
            fileMetadata.Name = "fhir-request";
            fileMetadata.ContentType = "application/json";

            var storageFile = new StorageFile();
            storageFile.FileContent = fileContentStream;
            storageFile.FileMetadata = fileMetadata;
            storageFile.StorageFilePath = _fileStorageService.GetStorageFilePath(fileMetadata);

            await _fileStorageService.UploadFile(storageFile);
        }
    }
}
