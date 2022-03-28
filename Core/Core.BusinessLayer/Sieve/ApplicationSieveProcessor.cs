using HMSDigital.Core.Data.Models;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using HospiceMember = HMSDigital.Core.Data.Models.HospiceMember;
using Inventory = HMSDigital.Core.Data.Models.Inventory;
using PatientInventory = HMSDigital.Core.Data.Models.PatientInventory;
using CreditHoldHistory = HMSDigital.Core.Data.Models.CreditHoldHistory;
using HMSDigital.Common.ViewModels;

namespace HMSDigital.Core.BusinessLayer.Sieve
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods sieveCustomSortMethods, ISieveCustomFilterMethods sieveCustomFilterMethods) : base(options, sieveCustomSortMethods, sieveCustomFilterMethods)
        {
        }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            #region DispatchAuditLog

            mapper.Property<Data.Models.DispatchAuditLog>(d => d.AuditDate).CanFilter().CanSort();

            mapper.Property<Data.Models.DispatchAuditLog>(d => d.PatientUuid).CanFilter().CanSort();

            #endregion

            #region HospiceLocation
            mapper.Property<HospiceLocations>(l => l.HospiceId).CanFilter().CanSort();

            mapper.Property<HospiceLocations>(l => l.Id).CanFilter().CanSort();

            mapper.Property<HospiceLocations>(l => l.Name).CanFilter().CanSort();
            #endregion

            #region Hospice
            mapper.Property<Hospices>(l => l.Id).CanFilter().CanSort();

            mapper.Property<Hospices>(l => l.Name).CanFilter().CanSort();
            #endregion

            #region FacilityPatientHistory

            mapper.Property<FacilityPatientHistory>(h => h.CreatedDateTime).CanFilter().CanSort().HasName("AssignedDateTime");

            #endregion

            #region Inventory

            mapper.Property<Inventory>(h => h.SerialNumber).CanFilter().CanSort();

            mapper.Property<Inventory>(h => h.AssetTagNumber).CanFilter().CanSort();

            mapper.Property<Inventory>(h => h.CurrentLocationId).CanFilter().CanSort();

            mapper.Property<Inventory>(i => i.StatusId).CanFilter().CanSort();

            mapper.Property<Inventory>(i => i.ItemId).CanFilter().CanSort();

            mapper.Property<Inventory>(h => h.LotNumber).CanFilter().CanSort();

            mapper.Property<Inventory>(h => h.QuantityAvailable).CanFilter().CanSort().HasName("Count");

            mapper.Property<Inventory>(h => h.Item.Name).CanFilter().CanSort().HasName("ItemName");

            #endregion

            #region OrderHeaders

            mapper.Property<OrderHeaders>(l => l.Id).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.StatusId).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.DispatchStatusId).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.OrderTypeId).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.PatientUuid).CanFilter().CanSort().HasName("PatientUUID");

            mapper.Property<OrderHeaders>(l => l.SiteId).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.StatOrder).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.OrderDateTime).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.RequestedStartDateTime).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.FulfillmentEndDateTime).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.HospiceLocationId).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.OrderNumber).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.HospiceId).CanFilter().CanSort();

            mapper.Property<OrderHeaders>(l => l.IsExceptionFulfillment).CanFilter().CanSort();

            #endregion

            #region Sites

            mapper.Property<Sites>(s => s.Id).CanFilter().CanSort();

            mapper.Property<Sites>(s => s.Name).CanFilter().CanSort();

            mapper.Property<Sites>(s => s.SiteCode).CanFilter().CanSort();

            mapper.Property<Sites>(s => s.LocationType).CanFilter().CanSort();

            mapper.Property<Sites>(s => s.LocationTypeId).CanFilter().CanSort();

            mapper.Property<Sites>(s => s.LicensePlate).CanFilter().CanSort();

            mapper.Property<Sites>(s => s.Cvn).CanFilter().CanSort();

            #endregion

            #region PatientInventory

            mapper.Property<PatientInventory>(i => i.OrderHeaderId).CanFilter().CanSort();

            mapper.Property<PatientInventory>(i => i.ItemId).CanFilter().CanSort();

            mapper.Property<PatientInventory>(i => i.Item.IsAssetTagged).CanFilter().CanSort().HasName("IsAssetTagged");

            mapper.Property<PatientInventory>(i => i.Item.IsConsumable).CanFilter().CanSort().HasName("IsConsumable");

            mapper.Property<PatientInventory>(i => i.Inventory.AssetTagNumber).CanFilter().CanSort().HasName("InventoryAssetTagNumber").HasName("AssetTagNumber");

            mapper.Property<PatientInventory>(i => i.Inventory.SerialNumber).CanFilter().CanSort().HasName("SerialNumber");

            mapper.Property<PatientInventory>(i => i.Inventory.LotNumber).CanFilter().CanSort().HasName("LotNumber");

            mapper.Property<PatientInventory>(i => i.IsExceptionFulfillment).CanFilter().CanSort();

            #endregion

            #region User

            mapper.Property<Users>(u => u.FirstName).CanFilter().CanSort().HasName("Name");

            mapper.Property<Users>(u => u.Email).CanFilter().CanSort();

            mapper.Property<Users>(u => u.IsDisabled).CanFilter().CanSort();

            mapper.Property<Users>(u => u.Id).CanFilter().CanSort();

            #endregion

            #region DispatchInstruction

            mapper.Property<DispatchInstructions>(d => d.DispatchStartDateTime).CanFilter().CanSort();

            mapper.Property<DispatchInstructions>(d => d.DispatchEndDateTime).CanFilter().CanSort();

            mapper.Property<DispatchInstructions>(d => d.VehicleId).CanFilter().CanSort();

            mapper.Property<DispatchInstructions>(d => d.OrderHeaderId).CanFilter().CanSort().HasName("orderId");

            mapper.Property<DispatchInstructions>(d => d.OrderHeader.StatusId).CanFilter().CanSort().HasName("orderStatusId");

            mapper.Property<DispatchInstructions>(d => d.OrderHeader.DispatchStatusId).CanFilter().CanSort().HasName("orderDispatchStatusId");

            #endregion

            #region PatientFacility

            mapper.Property<FacilityPatient>(d => d.PatientUuid).CanFilter().CanSort();

            #endregion

            #region HospiceMember

            mapper.Property<HospiceMember>(hm => hm.UserId).CanFilter().CanSort();

            mapper.Property<HospiceMember>(hm => hm.HospiceId).CanFilter().CanSort();

            #endregion

            #region Facility

            mapper.Property<Facilities>(f => f.HospiceId).CanFilter().CanSort();

            #endregion

            #region ItemImages

            mapper.Property<ItemImages>(d => d.ItemId).CanFilter().CanSort();

            #endregion

            #region ItemCategories

            mapper.Property<ItemCategories>(d => d.Name).CanFilter().CanSort();
            mapper.Property<ItemCategories>(d => d.NetSuiteCategoryId).CanFilter().CanSort();

            #endregion

            #region Drivers

            mapper.Property<Drivers>(p => p.User.FirstName).CanSort().HasName("Name");

            mapper.Property<Drivers>(p => p.CurrentSiteId).CanSort().CanFilter();

            #endregion

            #region Vehicles

            mapper.Property<Vehicle>(d => d.SiteId).CanFilter().CanSort();

            mapper.Property<Vehicle>(v => v.SiteName).CanFilter().CanSort();

            mapper.Property<Vehicle>(v => v.CurrentDriverName).CanFilter().CanSort();

            mapper.Property<Vehicle>(v => v.Cvn).CanFilter().CanSort();

            mapper.Property<Vehicle>(v => v.Vin).CanFilter().CanSort();

            mapper.Property<Vehicle>(v => v.LicensePlate).CanFilter().CanSort();

            #endregion

            #region Items

            mapper.Property<Items>(d => d.ItemNumber).CanFilter().CanSort();

            mapper.Property<Items>(d => d.Name).CanFilter().CanSort();

            mapper.Property<Items>(i => i.EquipmentSettingsConfig).CanFilter();

            #endregion

            #region Roles

            mapper.Property<Roles>(r => r.Name).CanSort();

            mapper.Property<Roles>(r => r.RoleType).CanSort();

            #endregion

            #region CreditHoldHistory

            mapper.Property<CreditHoldHistory>(c => c.CreditHoldDateTime).CanFilter().CanSort();

            mapper.Property<CreditHoldHistory>(c => c.HospiceId).CanFilter().CanSort();

            mapper.Property<CreditHoldHistory>(c => c.CreditHoldByUserId).CanFilter().CanSort();

            #endregion

            #region ContractRecords

            mapper.Property<ContractRecords>(c => c.NetSuiteCustomerId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.HospiceId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.HospiceLocationId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.NetSuiteContractRecordId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.NetSuiteSubscriptionId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.NetSuiteBillingItemId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.NetSuiteRelatedItemId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.ItemId).CanFilter().CanSort();

            mapper.Property<ContractRecords>(c => c.Item.Name).CanFilter().CanSort().HasName("ItemName");

            mapper.Property<ContractRecords>(c => c.Item.ItemNumber).CanFilter().CanSort().HasName("ItemNumber");

            #endregion

            #region Hms2ContractItems

            mapper.Property<Hms2ContractItems>(c => c.Contract.Hms2CustomerId).CanFilter().CanSort();

            mapper.Property<Hms2ContractItems>(c => c.Contract.HospiceId).CanFilter().CanSort();

            mapper.Property<Hms2ContractItems>(c => c.Contract.HospiceLocationId).CanFilter().CanSort();

            mapper.Property<Hms2ContractItems>(c => c.Hms2ContractItemId).CanFilter().CanSort();

            mapper.Property<Hms2ContractItems>(c => c.Hms2ContractId).CanFilter().CanSort();

            mapper.Property<Hms2ContractItems>(c => c.ItemId).CanFilter().CanSort();

            mapper.Property<Hms2ContractItems>(c => c.Item.Name).CanFilter().CanSort().HasName("ItemName");

            mapper.Property<Hms2ContractItems>(c => c.Item.ItemNumber).CanFilter().CanSort().HasName("ItemNumber");

            #endregion

            #region EquipmentSettingTypes

            mapper.Property<EquipmentSettingTypes>(r => r.Name).CanSort().CanFilter();

            mapper.Property<EquipmentSettingTypes>(r => r.Id).CanSort().CanFilter();

            #endregion

            return mapper;
        }
    }
}
