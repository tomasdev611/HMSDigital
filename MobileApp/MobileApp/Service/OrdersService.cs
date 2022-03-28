using MobileApp.Assets.Enums;
using MobileApp.DataBaseAttributes.Repositories;
using MobileApp.Methods;
using MobileApp.Models;
using MobileApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobileApp.Service
{
    public class OrdersService
    {
        private readonly DriverService _driverService;

        private readonly PatientService _patientService;

        private readonly PendingOrderRepository _pendingOrderRepository;

        public OrdersService()
        {
            _driverService = new DriverService();
            _patientService = new PatientService();
            _pendingOrderRepository = new PendingOrderRepository();
        }

        public async Task<IEnumerable<OrderData>> GetOrderListAsync()
        {
            var ordersList = new List<OrderData>();
            try
            {
                var dispatchInstructions = await _driverService.GetDriversDispatchInstructions();
                if (dispatchInstructions != null && dispatchInstructions.OrderHeaders != null)
                {
                    foreach (var orderheader in dispatchInstructions.OrderHeaders)
                    {
                        var orderData = OrderDataUtils.GetOrderData(orderheader);

                        if (orderheader.OrderTypeId == (int)OrderTypes.Patient_Move)
                        {
                            var pendingOrder = await _pendingOrderRepository.GetAsync(po => po.OrderId == orderData.OrderID && po.OrderTypeId == orderData.OrderTypeId);
                            if (pendingOrder != null)
                            {
                                orderData.ShippingAddress = pendingOrder.IsMovePickupComplete ? orderheader.DeliveryAddress : orderheader.PickupAddress;
                            }
                        }
                        ordersList.Add(orderData);
                    }
                    ordersList = await AddPatientDetailsToOrders(ordersList);
                    ordersList = AddSequenceForOrdersFulfillment(ordersList, dispatchInstructions.Routes).ToList();
                }
                return ordersList;
            }
            catch
            {
                throw;
            }
        }

        private IEnumerable<OrderData> AddSequenceForOrdersFulfillment(List<OrderData> ordersLists, IEnumerable<RouteItem> routeItems)
        {
            foreach (var orderItem in ordersLists)
            {
                orderItem.SequenceNumber = routeItems.FirstOrDefault(ri => ri.OrderHeaderId == orderItem.OrderID
                                                                           && CommonUtility.CompareAddress(orderItem.ShippingAddress, ri.Address)
                                                                    ).SequenceNumber;
            }
            return ordersLists.OrderBy(ol => ol.SequenceNumber);
        }

        private async Task<List<OrderData>> AddPatientDetailsToOrders(List<OrderData> orderDataList)
        {
            try
            {
                var patientDetails = await _patientService.GetPatientDetails(orderDataList.Select(o => o.PatientUuId));

                if (patientDetails == null || patientDetails.Records.Count() == 0)
                {
                    return orderDataList;
                }

                foreach (var order in orderDataList)
                {
                    var patientDetail = patientDetails.Records.FirstOrDefault(p => p.UniqueId == order.PatientUuId.ToString());

                    if(patientDetail != null)
                    {
                        order.ContactNumber = patientDetail.PhoneNumbers.FirstOrDefault().Number;
                        order.ContactPerson = $"{patientDetail.FirstName} {patientDetail.LastName}";
                        order.PatientNotes = patientDetail.PatientNotes;
                        order.IsInfectious = patientDetail.IsInfectious;
                    }
                }
                return orderDataList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}