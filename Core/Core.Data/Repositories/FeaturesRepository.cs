using System;
using System.Collections.Generic;
using System.Text;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class FeaturesRepository : RepositoryBase<Features>, IFeaturesRepository
    {
        //public static KeyValuePair<string, string> Features { get;set; }
        public FeaturesRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        ICollection<KeyValuePair<string, string>> IFeaturesRepository.Features { get; set ; }
    }
}
