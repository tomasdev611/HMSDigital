using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Patient.ViewModels
{
    public class PatientMergeHistoryResponse
    {
        public Guid PatientUuid { get; set; }

        public Guid DuplicatePatientUuid { get; set; }

        public MergePatientBaseRequest ChangeLog { get; set; }

        public DateTime? MergedDateTime { get; set; }

        public int? MergedByUserId { get; set; }

        public string MergedByUserName { get; set; }
    }
}
