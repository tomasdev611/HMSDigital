using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class HospiceMemberCsvRequest : UserMinimal
    {
        public string Role { get; set; }

        public string HospiceLocation { get; set; }

        public string Designation { get; set; }

        public bool? CanAccessWebStore { get; set; }
    }
}
