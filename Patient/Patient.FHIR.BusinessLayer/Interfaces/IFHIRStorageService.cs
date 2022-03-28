using System.Threading.Tasks;

namespace HMSDigital.Patient.FHIR.BusinessLayer.Interfaces
{
    public interface IFHIRStorageService
    {
        public Task UploadJsonFile(string fileContent);
    }
}
