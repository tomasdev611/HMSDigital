
namespace HMSDigital.Core.BusinessLayer.Enums
{
    public enum Roles
    {
        //If change anything here please corresponding change in Script.SeedRolePermission.sql file in roles section
        MasterAdmin = 1,
        UserAdmin = 2,
        SiteManager = 3,
        Driver = 4,
        InventoryRVP = 5,
        HospiceAdmin = 6,
        StandardHospiceUser = 7,
        ExecutiveReporting = 8,
        CustomerService = 9,
        CustomerServiceSupervisor = 10,
        Finance = 11,
        InventoryManager = 12,
        FleetManager = 13,
        DMEStandardUser = 14,
        DMEAdmin = 15,
        BusinessAdmin = 16,
    }
}
