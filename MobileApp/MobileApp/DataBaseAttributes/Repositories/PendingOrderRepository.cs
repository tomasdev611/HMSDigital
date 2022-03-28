using MobileApp.Models;
using MobileApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.DataBaseAttributes.Repositories
{
    class PendingOrderRepository : DatabaseService<PendingOrder>
    {

        public PendingOrderRepository() : base()
        {

        }

        public async Task<IEnumerable<PendingOrder>> UpdatePendingOrders(IEnumerable<OrderData> newOrderList)
        {
            var pendingOrdersList = await GetAllAsync();

            var updatedList = new List<PendingOrder>();

            foreach (var order in newOrderList)
            {

                var pendingOrder = pendingOrdersList.FirstOrDefault(g =>
                        g.OrderId == order.OrderID
                        && order.OrderTypeId == g.OrderTypeId
                    );

                if(pendingOrder != null)
                {
                    updatedList.Add(pendingOrder);
                }
                else
                {
                    updatedList.Add(new PendingOrder()
                    {
                        OrderId = order.OrderID,
                        IsProcessed = false,
                        OrderType = order.OrderType,
                        OrderTypeId = order.OrderTypeId,
                        IsMovePickupComplete = false
                    });
                }
            }

            await DeleteAllAsync();

            await SaveAllAsync(updatedList);

            return updatedList;
        }

    }
}
