using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class FacilityPatientHistoryRepository : RepositoryBase<FacilityPatientHistory>, IFacilityPatientHistoryRepository
    {
        public FacilityPatientHistoryRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
