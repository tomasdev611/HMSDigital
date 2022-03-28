using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class EquipmentSettingTypeRepository : RepositoryBase<EquipmentSettingTypes>, IEquipmentSettingTypeRepository
    {
        public EquipmentSettingTypeRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
