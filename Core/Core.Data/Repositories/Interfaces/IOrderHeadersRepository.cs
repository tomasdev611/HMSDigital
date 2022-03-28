using HMSDigital.Core.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories.Interfaces
{
    public interface IOrderHeadersRepository : IRepository<OrderHeaders>
    {
        Task<IEnumerable<OrderHeaders>> GetDispatchOrderAsync(Expression<Func<OrderHeaders, bool>> where);
    }
}
