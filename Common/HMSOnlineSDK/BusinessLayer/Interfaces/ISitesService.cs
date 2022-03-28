using HMSOnlineSDK.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace HMSOnlineSDK.BusinessLayer.Interfaces
{
   public interface ISitesService
    {
       Task<IEnumerable<Sites>> GetAllSites();
    }
}
