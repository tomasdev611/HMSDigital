using HMSOnlineSDK.BusinessLayer.Interfaces;
using HMSOnlineSDK.Data.Models;
using HMSOnlineSDK.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSOnlineSDK.BusinessLayer
{
    public class SitesService : ISitesService
    {
        private readonly ISitesRepository _sitesRepository;

        public SitesService(ISitesRepository sitesRepository)
        {
            _sitesRepository = sitesRepository;
        }

      public  Task<IEnumerable<Sites>> GetAllSites()
        {
            return _sitesRepository.GetAllAsync();
        }
    }
}
