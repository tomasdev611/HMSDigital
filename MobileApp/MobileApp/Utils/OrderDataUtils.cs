using System;
using System.Collections.Generic;
using System.Linq;
using MobileApp.Assets.Enums;
using MobileApp.Models;

namespace MobileApp.Utils
{
    public class OrderDataUtils
    {
        public static OrderData GetOrderData(OrderHeader orderheaders)
        {
            var orderTypeWithDelivery = GetOrderTypesWithDelivery();

            var expectedDeliveryDate = DateTime.Equals(orderheaders.RequestedEndDateTime.Date, DateTime.UtcNow.Date) ?
                        null : orderheaders.RequestedEndDateTime.ToLocalTime().ToString("dd MMM");

            return new OrderData
            {
                StatOrder = orderheaders.StatOrder,
                ItemCount = orderheaders.OrderLineItems.Sum(j => j.ItemCount),
                ShippingAddress = orderTypeWithDelivery.Contains(orderheaders.OrderTypeId) ? orderheaders.DeliveryAddress : orderheaders.PickupAddress,
                OrderLineItems = orderheaders.OrderLineItems,
                ETA = orderheaders.RequestedEndDateTime.ToLocalTime().ToString("hh:mm tt"),
                ExpectedDeliveryDate = expectedDeliveryDate,
                OrderID = orderheaders.Id,
                StatusId = orderheaders.StatusId,
                OrderType = orderheaders.OrderType,
                OrderTypeId = orderheaders.OrderTypeId,
                PatientUuId = orderheaders.PatientUuid,
                OrderStatus = orderheaders.OrderStatus,
                IsCompleted = orderheaders.StatusId == (int)OrderHeaderStatusTypes.Completed,
                IsExceptionFulfillment = orderheaders.IsExceptionFulfillment && orderheaders.StatusId == (int)OrderHeaderStatusTypes.Partial_Fulfillment,
                OrderNotes = orderheaders.OrderNotes,
                OrderNumber = orderheaders.OrderNumber
            };
        }

        public static IEnumerable<int> GetOrderTypesWithDelivery()
        {
            return new List<int> { (int)OrderTypes.Delivery, (int)OrderTypes.Respite, (int)OrderTypes.Exchange, (int)OrderTypes.Patient_Move };
        }

        public static IEnumerable<int> GetOrderTypesWithPickup()
        {
            return new List<int> { (int)OrderTypes.Exchange, (int)OrderTypes.Pickup, (int)OrderTypes.Patient_Move };
        }
    }
}
