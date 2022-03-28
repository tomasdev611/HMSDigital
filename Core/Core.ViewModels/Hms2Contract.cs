using HMSDigital.Common.ViewModels;
using System;
using System.Collections.Generic;

namespace HMSDigital.Core.ViewModels
{
    public partial class Hms2Contract
    {
        public int Id { get; set; }
        
        public int Hms2ContractId { get; set; }
                
        public string ContractName { get; set; }
        
        public string ContractNumber { get; set; }
        
        public int? Hms2HospiceId { get; set; }
        
        public int? Hms2CustomerId { get; set; }
        
        public decimal? PerDiemRate { get; set; }
        
        public DateTime? StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public int? HospiceId { get; set; }
        
        public int? HospiceLocationId { get; set; }

        public  Hospice Hospice { get; set; }
        
        public  HospiceLocation HospiceLocation { get; set; }
    }
}
