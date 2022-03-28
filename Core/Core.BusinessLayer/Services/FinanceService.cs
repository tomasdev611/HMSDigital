using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using NSDKViewModels = HospiceSource.Digital.NetSuite.SDK.ViewModels;


namespace HMSDigital.Core.BusinessLayer.Services
{
    public class FinanceService : IFinanceService
    {
        private readonly IPatientService _patientService;

        private readonly IFacilityService _facilityService;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IFacilityRepository _facilityRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly INetSuiteService _netsuiteService;

        private readonly IMapper _mapper;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly HttpContext _httpContext;

        public FinanceService(IPatientService patientService,
            IHospiceRepository hospiceRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            IPatientInventoryRepository patientInventoryRepository,
            INetSuiteService netSuiteService,
            IMapper mapper,
            IOrderFulfillmentLineItemsRepository orderFulfillmentLineItemsRepository,
            IOrderHeadersRepository orderHeadersRepository,
            IHttpContextAccessor httpContextAccessor,
            IFacilityRepository facilityRepository,
            IFacilityService facilityService,
            IOptions<NetSuiteConfig> netSuiteOptions)
        {
            _patientService = patientService;
            _facilityService = facilityService;
            _hospiceRepository = hospiceRepository;
            _facilityRepository = facilityRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _patientInventoryRepository = patientInventoryRepository;
            _netsuiteService = netSuiteService;
            _orderFulfillmentLineItemsRepository = orderFulfillmentLineItemsRepository;
            _orderHeadersRepository = orderHeadersRepository;
            _mapper = mapper;
            _httpContext = httpContextAccessor.HttpContext;
            _netSuiteConfig = netSuiteOptions.Value;
        }

        public async Task FixPatientHospice(Guid patientUUID, FixPatientHospiceRequest fixPatientHospiceRequest)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(fixPatientHospiceRequest.HospiceId);

            if (hospiceModel == null)
            {
                throw new ValidationException($"hospiceId ({fixPatientHospiceRequest.HospiceId}) is not valid");
            }

            var hospiceLocationModel = await _hospiceLocationRepository.GetByIdAsync(fixPatientHospiceRequest.HospiceLocationId);
            if (hospiceLocationModel == null)
            {
                throw new ValidationException($"hospice location Id ({fixPatientHospiceRequest.HospiceLocationId}) is not valid");
            }

            if (!hospiceLocationModel.NetSuiteCustomerId.HasValue)
            {
                throw new ValidationException($"Hospice location \"{hospiceLocationModel.Name}\" does not have netsuite customer Id");
            }

            var validHospiceLocations = hospiceModel.HospiceLocations.Select(l => l.Id);
            if (!validHospiceLocations.Contains(fixPatientHospiceRequest.HospiceLocationId))
            {
                throw new ValidationException($"Hospice location \"{hospiceLocationModel.Name}\" is not part of hospice \"{hospiceModel.Name}\"");
            }

            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => pi.PatientUuid == patientUUID);

            foreach (var patientInventory in patientInventories)
            {
                patientInventory.HospiceId = fixPatientHospiceRequest.HospiceId;
                patientInventory.HospiceLocationId = fixPatientHospiceRequest.HospiceLocationId;
            }

            await _patientInventoryRepository.UpdateManyAsync(patientInventories);

            var orderHeaderModels = await _orderHeadersRepository.GetManyAsync(o => o.PatientUuid == patientUUID);

            foreach (var orderHeaderModel in orderHeaderModels)
            {
                orderHeaderModel.HospiceId = fixPatientHospiceRequest.HospiceId;
                orderHeaderModel.HospiceLocationId = fixPatientHospiceRequest.HospiceLocationId;
            }
            await _orderHeadersRepository.UpdateManyAsync(orderHeaderModels);
            await _patientService.UpdatePatientHospice(patientUUID, fixPatientHospiceRequest.HospiceId, fixPatientHospiceRequest.HospiceLocationId);
            _netsuiteService.FixPatientHospice(new NSDKViewModels.FixPatientHospiceRequest()
            {
                PatientUuid = patientUUID,
                CustomerLocationId = hospiceLocationModel.NetSuiteCustomerId.Value
            });
        }

        public async Task MergePatient(Guid patientUUID, MergePatientBaseRequest mergePatientReqeust)
        {
            var patientMergeRequest = _mapper.Map<MergePatientRequest>(mergePatientReqeust);

            var duplicatePatientInventory = await _patientInventoryRepository.GetManyAsync(pi => pi.PatientUuid == patientMergeRequest.DuplicatePatientUUID);
            var duplicatePatientOrderHeaders = await _orderHeadersRepository.GetManyAsync(o => o.PatientUuid == patientMergeRequest.DuplicatePatientUUID);

            var patientOrderHeaders = (await _orderHeadersRepository.GetManyAsync(o => o.PatientUuid == patientUUID));
            patientOrderHeaders = patientOrderHeaders.Concat(duplicatePatientOrderHeaders);
            patientMergeRequest.HasOpenOrders = patientOrderHeaders.Any(x => x.StatusId != (int)OrderHeaderStatusTypes.Completed
                                                             && x.StatusId != (int)OrderHeaderStatusTypes.Cancelled
                                                             && !(x.IsExceptionFulfillment && x.StatusId == (int)OrderHeaderStatusTypes.Partial_Fulfillment));

            var patientInventory = await _patientInventoryRepository.GetManyAsync(pi => pi.PatientUuid == patientUUID);
            patientInventory = patientInventory.Concat(duplicatePatientInventory);
            patientMergeRequest.IsDMEEquipmentLeft = patientInventory.Any(pi => pi.Item.IsDme && !pi.IsExceptionFulfillment);

            patientMergeRequest.MergedByUserId = GetLoggedInUserId();

            await _patientService.MergePatient(patientUUID, patientMergeRequest);

            foreach (var patientInventoryItem in duplicatePatientInventory)
            {
                patientInventoryItem.PatientUuid = patientUUID;
            }
            await _patientInventoryRepository.UpdateManyAsync(duplicatePatientInventory);

            foreach (var orderHeaderModel in duplicatePatientOrderHeaders)
            {
                orderHeaderModel.PatientUuid = patientUUID;
            }
            await _orderHeadersRepository.UpdateManyAsync(duplicatePatientOrderHeaders);

            var fulfillmentLineItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(o => o.PatientUuid == patientMergeRequest.DuplicatePatientUUID);
            foreach (var fulfillmentLineItem in fulfillmentLineItems)
            {
                fulfillmentLineItem.PatientUuid = patientUUID;
            }
            await _orderFulfillmentLineItemsRepository.UpdateManyAsync(fulfillmentLineItems);

            _netsuiteService.MergeDuplicatePatients(patientUUID, mergePatientReqeust.DuplicatePatientUUID);
        }

        public async Task MovePatientToHospiceLocation(Guid patientUUID, MovePatientToHospiceLocationRequest movePatientToHospiceLocationRequest)
        {
            await ValidateMovePatientRequest(movePatientToHospiceLocationRequest);

            var OrderFulfillmentItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(f => f.PatientUuid == patientUUID);
            var lastFulfillmentDate = OrderFulfillmentItems.OrderByDescending(f => f.FulfillmentEndDateTime).FirstOrDefault()?.FulfillmentEndDateTime;
            if(lastFulfillmentDate != null && lastFulfillmentDate > movePatientToHospiceLocationRequest.MovementDate)
            {
                throw new ValidationException($"Movement date should be greater than last fulfilment date");
            }

            var hospiceModel = await _hospiceRepository.GetByIdAsync(movePatientToHospiceLocationRequest.HospiceId);

            var newPatientRecord = await _patientService.MovePatientToHospiceLocation(patientUUID, movePatientToHospiceLocationRequest.HospiceId, movePatientToHospiceLocationRequest.HospiceLocationId, movePatientToHospiceLocationRequest.MovementDate);

            if (movePatientToHospiceLocationRequest.Facilities != null && movePatientToHospiceLocationRequest.Facilities.Count() > 0)
            {
                var facilityPatientRequest = new FacilityPatientRequest()
                {
                    PatientUuid = new Guid(newPatientRecord.UniqueId),
                    FacilityPatientRooms = movePatientToHospiceLocationRequest.Facilities
                };
                await _facilityService.AssignPatientToFacilities(facilityPatientRequest);
            }

            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => pi.PatientUuid == patientUUID);

            await CreateDispatchRecordsForMovingPatient(patientInventories, movePatientToHospiceLocationRequest.MovementDate, (int)hospiceModel.NetSuiteCustomerId, patientUUID, newPatientRecord.UniqueId);

            if (patientInventories.Count() > 0)
            {
                foreach (var patientInventory in patientInventories)
                {
                    patientInventory.HospiceId = movePatientToHospiceLocationRequest.HospiceId;
                    patientInventory.HospiceLocationId = movePatientToHospiceLocationRequest.HospiceLocationId;
                    patientInventory.PatientUuid = new Guid(newPatientRecord.UniqueId);
                }

                await _patientInventoryRepository.UpdateManyAsync(patientInventories);
            }
        }

        private async Task ValidateMovePatientRequest(MovePatientToHospiceLocationRequest movePatientToHospiceLocationRequest)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(movePatientToHospiceLocationRequest.HospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"hospiceId ({movePatientToHospiceLocationRequest.HospiceId}) is not valid");
            }

            var hospiceLocationModel = await _hospiceLocationRepository.GetByIdAsync(movePatientToHospiceLocationRequest.HospiceLocationId);
            if (hospiceLocationModel == null)
            {
                throw new ValidationException($"hospice location Id ({movePatientToHospiceLocationRequest.HospiceLocationId}) is not valid");
            }

            if (!hospiceLocationModel.NetSuiteCustomerId.HasValue)
            {
                throw new ValidationException($"Hospice location \"{hospiceLocationModel.Name}\" does not have netsuite customer Id");
            }

            if (hospiceLocationModel.HospiceId != hospiceModel.Id)
            {
                throw new ValidationException($"Hospice location \"{hospiceLocationModel.Name}\" is not part of hospice \"{hospiceModel.Name}\"");
            }

            if (movePatientToHospiceLocationRequest.Facilities != null && movePatientToHospiceLocationRequest.Facilities.Count() > 0)
            {
                var facilityIds = movePatientToHospiceLocationRequest.Facilities.Select(facility => facility.FacilityId);
                var facilities = await _facilityRepository.GetManyAsync(f => facilityIds.Contains(f.Id));

                var invalidFacilities = facilityIds.Except(facilities.Select(f => f.Id));

                if (invalidFacilities.Count() > 0)
                {
                    throw new ValidationException($"facilityIds ({string.Join(",", invalidFacilities)}) are not valid");
                }

                var invalidHospiceIdFacilities = facilities.Where(f => f.HospiceId != movePatientToHospiceLocationRequest.HospiceId).Select(f => f.Name);

                if (facilities.Any(f => f.HospiceId != movePatientToHospiceLocationRequest.HospiceId))
                {
                    throw new ValidationException($"facilities ({string.Join(",", invalidHospiceIdFacilities)}) do not belong to hospice ({hospiceModel.Name})");
                }
            }
        }

        private int? GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return null;
            }
            return int.Parse(userIdClaim.Value);
        }

        private async Task CreateDispatchRecordsForMovingPatient(IEnumerable<Data.Models.PatientInventory> patientInventories,
            DateTime? movementDate,
            int netSuiteCustomerId,
            Guid patientUUID,
            string newPatientUUID)
        {
            var pickupItems = patientInventories.Select(i => new NSDKViewModels.FulfilmentItem
            {
                NetSuiteItemId = (int)i.Item.NetSuiteItemId,
                Quantity = i.ItemCount,
                NetSuiteWarehouseId = _netSuiteConfig.PatientWarehouseId,
                PickupDateTime = movementDate,
                OrderType = "pickup",
                SerialNumber = i.Inventory.SerialNumber,
                LotNumber = i.Inventory.LotNumber,
                AssetTag = i.Inventory.AssetTagNumber,
                PickupOrderDateTime = movementDate,
            });

            var fulfillmentPickupRequest = new NSDKViewModels.ConfirmFulfilmentRequest()
            {
                NetSuiteCustomerId = netSuiteCustomerId,
                PatientUuid = patientUUID.ToString(),
                DispatchOnly = true,
                Items = pickupItems
            };

            var confirmFulfillmentPickupResponse = await _netsuiteService.ConfirmOrderFulfilment(fulfillmentPickupRequest);

            if (confirmFulfillmentPickupResponse == null || !confirmFulfillmentPickupResponse.Success)
            {
                throw new ValidationException($"Could not create pickup dispatch records for patient - ({confirmFulfillmentPickupResponse.Error.Message})");
            }

            var deliveryItems = patientInventories.Select(i => new NSDKViewModels.FulfilmentItem
            {
                NetSuiteItemId = (int)i.Item.NetSuiteItemId,
                Quantity = i.ItemCount,
                NetSuiteWarehouseId = _netSuiteConfig.PatientWarehouseId,
                DeliveryDateTime = movementDate?.AddDays(1),
                OrderType = "delivery",
                SerialNumber = i.Inventory.SerialNumber,
                LotNumber = i.Inventory.LotNumber,
                AssetTag = i.Inventory.AssetTagNumber,
                DeliveredStatus = "Delivered"
            });

            var fulfillmentDeliveryRequest = new NSDKViewModels.ConfirmFulfilmentRequest()
            {
                NetSuiteCustomerId = netSuiteCustomerId,
                PatientUuid = newPatientUUID,
                DispatchOnly = true,
                Items = deliveryItems
            };

            var confirmFulfillmentDeliveryResponse = await _netsuiteService.ConfirmOrderFulfilment(fulfillmentDeliveryRequest);

            if (confirmFulfillmentDeliveryResponse == null || !confirmFulfillmentDeliveryResponse.Success)
            {
                throw new ValidationException($"Could not create delivery dispatch records for newly created patient - ({confirmFulfillmentDeliveryResponse.Error.Message})");
            }
        }
    }
}
