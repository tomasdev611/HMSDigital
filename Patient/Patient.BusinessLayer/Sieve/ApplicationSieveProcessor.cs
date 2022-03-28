using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace HMSDigital.Patient.BusinessLayer.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods sieveCustomFilterMethods) : base(options, customSortMethods, sieveCustomFilterMethods)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            #region patient

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.HospiceLocationId).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.UniqueId).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.Id).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.DateOfBirth).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.CreatedDateTime).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.LastOrderDateTime).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.HospiceId).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.Hms2Id).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.FirstName).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.LastName).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.StatusId).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.FhirPatientId).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientDetails>(p => p.Status.Name).CanFilter().CanSort().HasName("status");

            #endregion

            #region PatientMergeHistory

            mapper.Property<Patient.Data.Models.PatientMergeHistory>(p => p.PatientUuid).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientMergeHistory>(p => p.DuplicatePatientUuid).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientMergeHistory>(p => p.CreatedByUserId).CanFilter().CanSort();

            mapper.Property<Patient.Data.Models.PatientMergeHistory>(p => p.CreatedDateTime).CanFilter().CanSort();

            #endregion

            return mapper;
        }
    }
}
