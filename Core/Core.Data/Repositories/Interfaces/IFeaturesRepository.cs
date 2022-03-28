using HMSDigital.Core.Data.Models;
using System.Collections.Generic;

namespace HMSDigital.Core.Data.Repositories.Interfaces
{
    public interface IFeaturesRepository : IRepository<Features>
    {
        ICollection<KeyValuePair<string, string>> Features { get;set; }
    }
}
