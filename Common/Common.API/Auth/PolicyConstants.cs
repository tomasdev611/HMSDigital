namespace HMSDigital.Common.API.Auth
{
    public static class PolicyConstants
    {
        #region core

        public const string CAN_MANAGE_USER_ACCESS = "CanManageUserAccess";

        public const string CAN_MANAGE_ROLES = "CanManageRoles";

        public const string CAN_CREATE_USER = "CanCreateUser";

        public const string CAN_UPDATE_USER = "CanUpdateUser";

        public const string CAN_READ_USER = "CanReadUser";

        public const string CAN_READ_CONTACT = "CanReadContact";

        public const string CAN_CREATE_CONTACT = "CanCreateContact";

        public const string CAN_READ_AUDIT = "CanReadAudit";

        public const string CAN_READ_HOSPICES = "CanReadHospices";

        public const string CAN_MANAGE_HOSPICES = "CanManageHospices";

        public const string CAN_READ_LOCATIONS = "CanReadHospiceLocations";

        public const string CAN_READ_HOSPICE_MEMBERS = "CanReadHospiceMembers";

        public const string CAN_CREATE_HOSPICE_MEMBERS = "CanCreateHospiceMembers";

        public const string CAN_MANAGE_HOSPICE_MEMBERS = "CanManageHospiceMembers";

        public const string CAN_READ_SITE = "CanReadSites";

        public const string CAN_CREATE_SITE_SERVICE_AREA = "CanCreateSiteServiceAreas";

        public const string CAN_DELETE_SITE_SERVICE_AREA = "CanDeleteSiteServiceAreas";

        public const string CAN_READ_FACILITIES = "CanReadFacilities";

        public const string CAN_CREATE_FACILITIES = "CanCreateFacilities";

        public const string CAN_MANAGE_FACILITIES = "CanManageFacilities";

        public const string CAN_READ_ORDERS = "CanReadOrders";

        public const string CAN_CREATE_ORDERS = "CanCreateOrders";

        public const string CAN_FULFILL_ORDERS = "CanFulfillOrders";

        public const string CAN_READ_VEHICLES = "CanReadVehicles";

        public const string CAN_CREATE_VEHICLES = "CanCreateVehicles";

        public const string CAN_UPDATE_VEHICLES = "CanUpdateVehicles";

        public const string CAN_READ_INVENTORY = "CanReadInventory";

        public const string CAN_CREATE_INVENTORY = "CanCreateInventory";

        public const string CAN_UPDATE_INVENTORY = "CanUpdateInventory";

        public const string CAN_DELETE_INVENTORY = "CanDeleteInventory";

        public const string CAN_READ_DRIVERS = "CanReadDrivers";

        public const string CAN_CREATE_DRIVERS = "CanCreateDrivers";

        public const string CAN_UPDATE_DRIVERS = "CanUpdateDrivers";

        public const string CAN_READ_SITE_MEMBERS = "CanReadSiteMembers";

        public const string CAN_CREATE_SITE_MEMBERS = "CanCreateSiteMembers";

        public const string CAN_UPDATE_SITE_MEMBERS = "CanUpdateSiteMembers";

        public const string CAN_READ_DISPATCH_INSTRUCTIONS = "CanReadDispatchInstructions";

        public const string CAN_CREATE_DISPATCH_INSTRUCTIONS = "CanCreateDispatchInstructions";

        public const string CAN_UPDATE_DISPATCH_RECORDS = "CanUpdateDispatchRecords";

        public const string CAN_MANAGE_ORDERS = "CanManageOrders";

        public const string CAN_READ_TRANSFER_REQUESTS = "CanReadTransferRequests";

        public const string CAN_READ_SYSTEM = "CanReadSystem";

        public const string CAN_UPDATE_SYSTEM = "CanUpdateSystem";

        public const string CAN_MANAGE_INVENTORY = "CanManageInventory";

        public const string CAN_READ_CUSTOMER_CONTRACT = "CanReadCustomerContract";

        public const string CAN_MANAGE_CUSTOMER_CONTRACT = "CanManageCustomerContract";

        public const string CAN_MANAGE_CREDIT_ON_HOLD = "CanManageCreditOnHold";

        public const string CAN_UPDATE_FINANCE = "CanUpdateFinance";

        public const string CAN_READ_FINANCE = "CanReadFinance";

        public const string CAN_READ_CATEGORY = "CanReadCategory";

        #endregion

        #region patient

        public const string CAN_READ_PATIENTS = "CanReadPatients";

        public const string CAN_CREATE_PATIENTS = "CanCreatePatients";

        public const string CAN_MANAGE_PATIENTS = "CanManagePatients";

        public const string CAN_RECORD_PATIENT_ORDER = "CanRecordPatientOrder";

        #endregion

        #region netsuiteIntegration

        public const string CAN_CREATE_CUSTOMER_INTEGRATION = "CanCreateCustomerIntegration";

        public const string CAN_CREATE_WAREHOUSE_INTEGRATION = "CanCreateWarehouseIntegration";

        public const string CAN_CREATE_INVENTORY_INTEGRATION = "CanCreateInventoryIntegration";

        public const string CAN_READ_PATIENT_LOOK_UP = "CanReadPatientLookUp";

        public const string CAN_CREATE_CONTACT_INTEGRATION = "CanCreateContactIntegration";

        public const string CAN_CREATE_DISPATCH_RECORD_INTEGRATION = "CanCreateDispatchRecordIntegration";

        #endregion

        #region notification
        public const string CAN_SEND_NOTIFICATION = "CanSendNotification";
        #endregion

        #region reports
        public const string CAN_READ_METRICS = "CanReadMetrics";
        #endregion
    }
}
