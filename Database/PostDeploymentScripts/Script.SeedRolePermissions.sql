/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/* Permission nouns declaration*/

DECLARE @P_System int = 1;
DECLARE @P_User int = 2;
DECLARE @P_Audit int = 3;
DECLARE @P_Hospice int = 4;
DECLARE @P_HospiceLocation int = 5;
DECLARE @P_Patient int = 6;
DECLARE @P_Orders int = 7;
DECLARE @P_Dispatch int = 8;
DECLARE @P_Inventory int = 9;
DECLARE @P_Site int = 10;
DECLARE @P_Driver int = 11;
DECLARE @P_Vehicle int = 12;
DECLARE @P_Facility int = 13;
DECLARE @P_CustomerContract int = 14;
DECLARE @P_Finance int = 15;
DECLARE @P_Metrics int = 16;
DECLARE @P_CustomersMetrics int = 17;
DECLARE @P_ClientServicesMetrics int = 18;
DECLARE @P_OperationsMetrics int = 19;

DECLARE @PermissionNounList TABLE (Id int, Name NVARCHAR(50))
INSERT INTO @PermissionNounList VALUES
			   (@P_System, 'System'),
               (@P_User, 'User'),
               (@P_Audit, 'Audit'),
               (@P_Hospice, 'Hospice'),
               (@P_HospiceLocation, 'HospiceLocation'),
               (@P_Patient, 'Patient'),
               (@P_Orders, 'Orders'),
               (@P_Dispatch, 'Dispatch'),
               (@P_Inventory, 'Inventory'),
               (@P_Site, 'Site'),
               (@P_Driver, 'Driver'),
               (@P_Vehicle, 'Vehicle'),
               (@P_Facility, 'Facility'),
               (@P_CustomerContract, 'CustomerContract'),
               (@P_Finance, 'Finance'),
               (@P_Metrics, 'Metrics'),
               (@P_CustomersMetrics, 'CustomersMetrics'),
               (@P_ClientServicesMetrics, 'ClientServicesMetrics'),
               (@P_OperationsMetrics, 'OperationsMetrics')

 
/* Permission verbs declaration */
DECLARE @PV_Create int = 1;
DECLARE @PV_Read int = 2;
DECLARE @PV_Update int = 3;
DECLARE @PV_Delete int = 4;
DECLARE @PV_Approve int = 5;
DECLARE @PV_Fulfill int = 6;
DECLARE @PV_CreditHold int = 7;
DECLARE @PV_OverrideCreditHold int = 8;
DECLARE @PV_MultipleLoad int = 9;
DECLARE @PV_MobileFulfill int = 10;
 
DECLARE @PermissionVerbList TABLE (Id int, Name NVARCHAR(50))
INSERT INTO @PermissionVerbList VALUES
			   (@PV_Create, 'Create'),
               (@PV_Read, 'Read'),
               (@PV_Update, 'Update'),
               (@PV_Delete, 'Delete'),
               (@PV_Approve, 'Approve'),
               (@PV_Fulfill, 'Fulfill'),
               (@PV_CreditHold, 'CreditHold'),
               (@PV_OverrideCreditHold, 'OverrideCreditHold'),
               (@PV_MultipleLoad, 'MultipleLoad'),
               (@PV_MobileFulfill, 'MobileFulfill')


/* roles declaration*/
/* If change anything here please corresponding change in Roles enum file in core enum section */
DECLARE @R_MasterAdmin int = 1;
DECLARE @R_UserAdmin int = 2;
DECLARE @R_SiteManager  int = 3;
DECLARE @R_Driver  int = 4;
DECLARE @R_InventoryRVP int = 5;
DECLARE @R_HospiceAdmin  int = 6;
DECLARE @R_StandardHospiceUser  int = 7;
DECLARE @R_ExecutiveReporting int = 8;
DECLARE @R_CustomerService int = 9;
DECLARE @R_CustomerServiceSupervisor int = 10;
DECLARE @R_Finance int = 11;
DECLARE @R_InventoryManager int = 12;
DECLARE @R_FleetManager int = 13;
DECLARE @R_DMEStandardUser int = 14;
DECLARE @R_DMEAdmin int = 15;
DECLARE @R_BusinessAdmin int = 16;

DECLARE @RolesList TABLE (Id int, Name VARCHAR(50), Level int,IsStatic bit,RoleType VARCHAR(50))
INSERT INTO @RolesList VALUES
		   (@R_MasterAdmin, 'Master Admin', 1, 1,'Internal'),
           (@R_UserAdmin, 'User Admin', 2, 1,'Internal'),
           (@R_SiteManager,'Site Manager', 3, 1,'Internal'),
           (@R_Driver, 'Driver', 4, 1,'Internal'),
           (@R_InventoryRVP, 'Inventory RVP', 4, 1,'Internal'),
           (@R_HospiceAdmin, 'Hospice Admin', 11, 1,'Hospice'),
           (@R_StandardHospiceUser, 'Standard Hospice User', 12, 1,'Hospice'),
           (@R_ExecutiveReporting,'Executive Reporting', 5,1,'Internal'),
           (@R_CustomerService,'Customer Service', 6, 1,'Internal'),
           (@R_CustomerServiceSupervisor,'Customer Service Supervisor', 5, 1,'Internal'),
           (@R_Finance,'Finance', 5, 1,'Internal'),
           (@R_InventoryManager,'Inventory Manager', 5, 1,'Internal'),
           (@R_FleetManager,'Fleet Manager', 5, 1,'Internal'),
           (@R_DMEStandardUser,'DME Standard User', 12, 1,'DME'),
           (@R_DMEAdmin,'DME Admin', 12, 1,'DME'),
           (@R_BusinessAdmin,'Business Admin', 5, 1,'Internal')

/* seed permission nouns*/
INSERT INTO [core].[PermissionNouns]  
SELECT a.Id, a.Name FROM @PermissionNounList a 
LEFT JOIN [core].[PermissionNouns] b 
ON a.Id = b.Id
WHERE b.Id IS NULL

UPDATE  [core].[PermissionNouns]  
SET [Name]=a.Name      
FROM @PermissionNounList a 
LEFT JOIN [core].[PermissionNouns] b 
ON a.Id = b.Id AND a.Name <> b.Name
WHERE b.Id IS NOT NULL

DELETE FROM [core].[PermissionNouns] 
WHERE Id IN 
(SELECT b.Id FROM @PermissionNounList a 
RIGHT JOIN [core].[PermissionNouns] b 
ON a.Id = b.Id
WHERE a.Id IS NULL);

/*seed permission verbs*/
INSERT INTO [core].[PermissionVerbs]  
SELECT a.Id, a.Name FROM @PermissionVerbList a 
LEFT JOIN [core].[PermissionVerbs] b 
ON a.Id = b.Id
WHERE b.Id IS NULL

UPDATE  [core].[PermissionVerbs]  
SET [Name]=a.Name      
FROM @PermissionVerbList a 
LEFT JOIN [core].[PermissionVerbs] b 
ON a.Id = b.Id AND a.Name <> b.Name
WHERE b.Id IS NOT NULL

DELETE FROM [core].[PermissionVerbs] 
WHERE Id IN 
(SELECT b.Id FROM @PermissionVerbList a 
RIGHT JOIN [core].[PermissionVerbs] b 
ON a.Id = b.Id
WHERE a.Id IS NULL);


/* seed roles */

INSERT INTO [core].[Roles] (Id, Name, Level, IsStatic, RoleType)
SELECT a.* FROM @RolesList a 
LEFT JOIN [core].[Roles] b 
ON a.Id = b.Id
WHERE b.Id IS NULL

UPDATE  [core].[Roles]  
SET [Name] = a.Name, [Level] = a.Level, [IsStatic] = a.IsStatic, [RoleType] = a.RoleType     
FROM  @RolesList a 
LEFT JOIN [core].[Roles] b 
ON a.Id = b.Id
WHERE b.Id IS NOT NULL


DELETE FROM [core].[Roles] 
WHERE Id IN 
(SELECT b.Id FROM @RolesList a 
RIGHT JOIN [core].[Roles] b 
ON a.Id = b.Id
WHERE a.Id IS NULL);

/* seed role permissions */

DECLARE @RolePermissionList TABLE (RoleId int, PermissionNounId int, PermissionVerbId int)
INSERT INTO @RolePermissionList VALUES
                /* Master Admin */
			   (@R_MasterAdmin, @P_System, @PV_Create),
               (@R_MasterAdmin, @P_System, @PV_Read),
               (@R_MasterAdmin, @P_System, @PV_Update),
               (@R_MasterAdmin, @P_System, @PV_Delete),
               
               (@R_MasterAdmin, @P_User, @PV_Create),
               (@R_MasterAdmin, @P_User, @PV_Read),
               (@R_MasterAdmin, @P_User, @PV_Update),
               (@R_MasterAdmin, @P_User, @PV_Delete),
               
               (@R_MasterAdmin, @P_Audit, @PV_Read),
               
               (@R_MasterAdmin, @P_Hospice, @PV_Create),
               (@R_MasterAdmin, @P_Hospice, @PV_Read),
               (@R_MasterAdmin, @P_Hospice, @PV_Update),
               (@R_MasterAdmin, @P_Hospice, @PV_Delete),
               (@R_MasterAdmin, @P_Hospice, @PV_CreditHold),

               (@R_MasterAdmin, @P_Facility, @PV_Create),
               (@R_MasterAdmin, @P_Facility, @PV_Read),
               (@R_MasterAdmin, @P_Facility, @PV_Update),
               (@R_MasterAdmin, @P_Facility, @PV_Delete),

               (@R_MasterAdmin, @P_HospiceLocation, @PV_Read),
               
               (@R_MasterAdmin, @P_Patient, @PV_Create),
               (@R_MasterAdmin, @P_Patient, @PV_Read),
               (@R_MasterAdmin, @P_Patient, @PV_Update),
               (@R_MasterAdmin, @P_Patient, @PV_Delete),
               
               (@R_MasterAdmin, @P_Orders, @PV_Read),
               (@R_MasterAdmin, @P_Orders, @PV_Create),
               (@R_MasterAdmin, @P_Orders, @PV_Update),
               (@R_MasterAdmin, @P_Orders, @PV_Fulfill),
               (@R_MasterAdmin, @P_Orders, @PV_OverrideCreditHold),
               
               (@R_MasterAdmin, @P_Dispatch, @PV_Create),
               (@R_MasterAdmin, @P_Dispatch, @PV_Read),
               (@R_MasterAdmin, @P_Dispatch, @PV_Update),
               
               (@R_MasterAdmin, @P_Inventory, @PV_Create),
               (@R_MasterAdmin, @P_Inventory, @PV_Read),
               (@R_MasterAdmin, @P_Inventory, @PV_Update),
               (@R_MasterAdmin, @P_Inventory, @PV_Delete),

               (@R_MasterAdmin, @P_Site, @PV_Create),
               (@R_MasterAdmin, @P_Site, @PV_Read),
               (@R_MasterAdmin, @P_Site, @PV_Update),
               (@R_MasterAdmin, @P_Site, @PV_Delete),

               (@R_MasterAdmin, @P_Driver, @PV_Create),
               (@R_MasterAdmin, @P_Driver, @PV_Read),
               (@R_MasterAdmin, @P_Driver, @PV_Update),
               (@R_MasterAdmin, @P_Driver, @PV_Delete),

               (@R_MasterAdmin, @P_Vehicle, @PV_Create),
               (@R_MasterAdmin, @P_Vehicle, @PV_Read),
               (@R_MasterAdmin, @P_Vehicle, @PV_Update),
               (@R_MasterAdmin, @P_Vehicle, @PV_Delete),

               (@R_MasterAdmin, @P_Finance, @PV_Read),
               (@R_MasterAdmin, @P_Finance, @PV_Update),

               (@R_MasterAdmin, @P_CustomerContract, @PV_Read),

               (@R_MasterAdmin, @P_Metrics, @PV_Read),
               (@R_MasterAdmin, @P_CustomersMetrics, @PV_Read),
               (@R_MasterAdmin, @P_ClientServicesMetrics, @PV_Read),
               (@R_MasterAdmin, @P_OperationsMetrics, @PV_Read),

               /* User Admin */
               (@R_UserAdmin, @P_User, @PV_Create),
               (@R_UserAdmin, @P_User, @PV_Read),
               (@R_UserAdmin, @P_User, @PV_Update),
               (@R_UserAdmin, @P_User, @PV_Delete),
               
               (@R_UserAdmin, @P_Hospice, @PV_Create),
               (@R_UserAdmin, @P_Hospice, @PV_Read),
               (@R_UserAdmin, @P_Hospice, @PV_Update),
               (@R_UserAdmin, @P_Hospice, @PV_Delete),
               
               (@R_UserAdmin, @P_Facility, @PV_Read),

               (@R_UserAdmin, @P_Site, @PV_Create),
               (@R_UserAdmin, @P_Site, @PV_Read),
               
               (@R_UserAdmin, @P_Driver, @PV_Create),
               (@R_UserAdmin, @P_Driver, @PV_Read),
               (@R_UserAdmin, @P_Driver, @PV_Update),
               (@R_UserAdmin, @P_Driver, @PV_Delete),

               /* Driver */
               (@R_Driver, @P_Inventory, @PV_Create),
               (@R_Driver, @P_Inventory, @PV_Read),
               (@R_Driver, @P_Inventory, @PV_Update),
               (@R_Driver, @P_Inventory, @PV_Delete),

               (@R_Driver, @P_Patient, @PV_Read),

               (@R_Driver, @P_Orders, @PV_Read),
               (@R_Driver, @P_Orders, @PV_Fulfill),
               (@R_Driver,@P_Orders,@PV_MobileFulfill),

               (@R_Driver, @P_Site, @PV_Read),
               
               /* Site Manager */
               (@R_SiteManager, @P_Site, @PV_Create),
               (@R_SiteManager, @P_Site, @PV_Read),
               (@R_SiteManager, @P_Site, @PV_Update),
               (@R_SiteManager, @P_Site, @PV_Delete),
               
               (@R_SiteManager, @P_Inventory, @PV_Read),
               (@R_SiteManager, @P_Inventory, @PV_Create),
               (@R_SiteManager, @P_Inventory, @PV_Update),
               (@R_SiteManager, @P_Inventory, @PV_Delete),
               
               (@R_SiteManager, @P_Patient, @PV_Read), 
               (@R_SiteManager, @P_Patient, @PV_Create),
               (@R_SiteManager, @P_Patient, @PV_Update),
               (@R_SiteManager, @P_Patient, @PV_Delete),
               
               (@R_SiteManager, @P_Dispatch, @PV_Create),
               (@R_SiteManager, @P_Dispatch, @PV_Read),
               
               (@R_SiteManager, @P_Orders, @PV_Read),
               (@R_SiteManager, @P_Orders, @PV_Fulfill),

               (@R_SiteManager, @P_Hospice, @PV_Read),

               (@R_SiteManager, @P_Facility, @PV_Read),
               
               (@R_SiteManager, @P_HospiceLocation, @PV_Read),
               
               
               (@R_SiteManager, @P_Driver, @PV_Create),
               (@R_SiteManager, @P_Driver, @PV_Read),
               (@R_SiteManager, @P_Driver, @PV_Update),
               (@R_SiteManager, @P_Driver, @PV_Delete),

               (@R_SiteManager, @P_Vehicle, @PV_Create),
               (@R_SiteManager, @P_Vehicle, @PV_Read),
               (@R_SiteManager, @P_Vehicle, @PV_Update),
               (@R_SiteManager, @P_Vehicle, @PV_Delete),
               (@R_SiteManager, @P_Vehicle, @PV_MultipleLoad),

               (@R_SiteManager, @P_Metrics, @PV_Read),
               (@R_SiteManager, @P_OperationsMetrics, @PV_Read),

               /* Inventory RVP */
               (@R_InventoryRVP, @P_Inventory, @PV_Create),
               (@R_InventoryRVP, @P_Inventory, @PV_Read),
               (@R_InventoryRVP, @P_Inventory, @PV_Update),
               (@R_InventoryRVP, @P_Inventory, @PV_Delete),
              
               (@R_InventoryRVP, @P_Site, @PV_Read),
               
               (@R_InventoryRVP, @P_Hospice, @PV_Read),

               (@R_InventoryRVP, @P_Facility, @PV_Read),
               
               (@R_InventoryRVP, @P_HospiceLocation, @PV_Read),

               (@R_InventoryRVP, @P_Patient, @PV_Read),
               
               /* Hospice Admin */
               (@R_HospiceAdmin, @P_Hospice, @PV_Create),
               (@R_HospiceAdmin, @P_Hospice, @PV_Read),
               (@R_HospiceAdmin, @P_Hospice, @PV_Update),
               (@R_HospiceAdmin, @P_Hospice, @PV_Delete),
               
               (@R_HospiceAdmin, @P_Facility, @PV_Create),
               (@R_HospiceAdmin, @P_Facility, @PV_Read),
               (@R_HospiceAdmin, @P_Facility, @PV_Update),
               (@R_HospiceAdmin, @P_Facility, @PV_Delete),

               (@R_HospiceAdmin, @P_HospiceLocation, @PV_Read),
               
               (@R_HospiceAdmin, @P_Patient, @PV_Create),
               (@R_HospiceAdmin, @P_Patient, @PV_Read),
               (@R_HospiceAdmin, @P_Patient, @PV_Update),
               (@R_HospiceAdmin, @P_Patient, @PV_Delete),

               (@R_HospiceAdmin, @P_Orders, @PV_Read),
               (@R_HospiceAdmin, @P_Orders, @PV_Create),
               (@R_HospiceAdmin, @P_Orders, @PV_Update),
               (@R_HospiceAdmin, @P_Orders, @PV_Approve),

               /* Standard Hospice User */
               (@R_StandardHospiceUser, @P_Hospice, @PV_Read),

               (@R_StandardHospiceUser, @P_Facility, @PV_Create),
               (@R_StandardHospiceUser, @P_Facility, @PV_Read),
               (@R_StandardHospiceUser, @P_Facility, @PV_Update),
               (@R_StandardHospiceUser, @P_Facility, @PV_Delete),

               (@R_StandardHospiceUser, @P_HospiceLocation, @PV_Read),

               (@R_StandardHospiceUser, @P_Orders, @PV_Read),
               (@R_StandardHospiceUser, @P_Orders, @PV_Create),
               (@R_StandardHospiceUser, @P_Orders, @PV_Update),
               (@R_StandardHospiceUser, @P_Orders, @PV_Approve),

               (@R_StandardHospiceUser, @P_Patient, @PV_Create),
               (@R_StandardHospiceUser, @P_Patient, @PV_Read),
               (@R_StandardHospiceUser, @P_Patient, @PV_Update),
               (@R_StandardHospiceUser, @P_Patient, @PV_Delete),
               
               /* Executive Reporting */
               (@R_ExecutiveReporting, @P_Site, @PV_Read),
               (@R_ExecutiveReporting, @P_Hospice, @PV_Read),
               (@R_ExecutiveReporting, @P_HospiceLocation, @PV_Read),
               (@R_ExecutiveReporting, @P_Metrics, @PV_Read),
               
               /* Customer Service */
               (@R_CustomerService, @P_Orders, @PV_Read),
               (@R_CustomerService, @P_Orders, @PV_Create),
               (@R_CustomerService, @P_Orders, @PV_Update),
               (@R_CustomerService, @P_Orders, @PV_Fulfill),

               (@R_CustomerService, @P_Dispatch, @PV_Create),
               (@R_CustomerService, @P_Dispatch, @PV_Read),
               
               (@R_CustomerService, @P_Hospice, @PV_Create),
               (@R_CustomerService, @P_Hospice, @PV_Read),
               (@R_CustomerService, @P_Hospice, @PV_Update),
               (@R_CustomerService, @P_Hospice, @PV_Delete),
               
               (@R_CustomerService, @P_Facility, @PV_Create),
               (@R_CustomerService, @P_Facility, @PV_Read),
               (@R_CustomerService, @P_Facility, @PV_Update),
               (@R_CustomerService, @P_Facility, @PV_Delete),

               (@R_CustomerService, @P_HospiceLocation, @PV_Read),
               
               (@R_CustomerService, @P_Patient, @PV_Create),
               (@R_CustomerService, @P_Patient, @PV_Read),
               (@R_CustomerService, @P_Patient, @PV_Update),
               (@R_CustomerService, @P_Patient, @PV_Delete),
               
               (@R_CustomerService, @P_Site, @PV_Create),
               (@R_CustomerService, @P_Site, @PV_Read),

               (@R_CustomerService, @P_Driver, @PV_Create),
               (@R_CustomerService, @P_Driver, @PV_Read),
               (@R_CustomerService, @P_Driver, @PV_Update),
               (@R_CustomerService, @P_Driver, @PV_Delete),

               (@R_CustomerService, @P_Vehicle, @PV_Create),
               (@R_CustomerService, @P_Vehicle, @PV_Read),
               (@R_CustomerService, @P_Vehicle, @PV_Update),
               (@R_CustomerService, @P_Vehicle, @PV_Delete),

               (@R_CustomerService, @P_Inventory, @PV_Read),

               (@R_CustomerService, @P_Metrics, @PV_Read),
               (@R_CustomerService, @P_ClientServicesMetrics, @PV_Read),

               /* Customer Service Supervisor*/
               (@R_CustomerServiceSupervisor, @P_Orders, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Orders, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Orders, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Orders, @PV_OverrideCreditHold),

               (@R_CustomerServiceSupervisor, @P_Dispatch, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Dispatch, @PV_Read),
               
               (@R_CustomerServiceSupervisor, @P_Hospice, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Hospice, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Hospice, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Hospice, @PV_Delete),
               
               (@R_CustomerServiceSupervisor, @P_Facility, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Facility, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Facility, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Facility, @PV_Delete),

               (@R_CustomerServiceSupervisor, @P_HospiceLocation, @PV_Read),
               
               (@R_CustomerServiceSupervisor, @P_Patient, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Patient, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Patient, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Patient, @PV_Delete),
               
               (@R_CustomerServiceSupervisor, @P_Site, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Site, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Site, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Site, @PV_Delete),

               (@R_CustomerServiceSupervisor, @P_Driver, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Driver, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Driver, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Driver, @PV_Delete),

               (@R_CustomerServiceSupervisor, @P_Vehicle, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Vehicle, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Vehicle, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Vehicle, @PV_Delete),
               
               (@R_CustomerServiceSupervisor, @P_Inventory, @PV_Create),
               (@R_CustomerServiceSupervisor, @P_Inventory, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_Inventory, @PV_Update),
               (@R_CustomerServiceSupervisor, @P_Inventory, @PV_Delete),

               (@R_CustomerServiceSupervisor, @P_Metrics, @PV_Read),
               (@R_CustomerServiceSupervisor, @P_ClientServicesMetrics, @PV_Read),

               /* Finance */
               (@R_Finance, @P_Orders, @PV_Read),
               (@R_Finance, @P_Orders, @PV_Create),
               (@R_Finance, @P_Orders, @PV_Update),
               (@R_Finance, @P_Orders, @PV_OverrideCreditHold),

               (@R_Finance, @P_Patient, @PV_Read),

               (@R_Finance, @P_Audit, @PV_Read),

               (@R_Finance, @P_Hospice, @PV_CreditHold),
               (@R_Finance, @P_Hospice, @PV_Read),
               (@R_Finance, @P_Hospice, @PV_Update),

               (@R_Finance, @P_HospiceLocation, @PV_Read),

               (@R_Finance, @P_Dispatch, @PV_Read),
               (@R_Finance, @P_Dispatch, @PV_Update),


               (@R_Finance, @P_Facility, @PV_Read),

               (@R_Finance, @P_Inventory, @PV_Read),
               (@R_Finance, @P_Inventory, @PV_Update),

               (@R_Finance, @P_Site, @PV_Read),

               (@R_Finance, @P_User, @PV_Read),

               (@R_Finance, @P_Finance, @PV_Read),
               (@R_Finance, @P_Finance, @PV_Update),

               (@R_Finance, @P_System, @PV_Read),

               /* Inventory Manager */
               (@R_InventoryManager, @P_Inventory, @PV_Create),
               (@R_InventoryManager, @P_Inventory, @PV_Read),
               (@R_InventoryManager, @P_Inventory, @PV_Update),
               (@R_InventoryManager, @P_Inventory, @PV_Delete),
               (@R_InventoryManager, @P_Site, @PV_Read),
               (@R_InventoryManager, @P_Hospice, @PV_Read),


               /* Fleet Manager */
               (@R_FleetManager, @P_Driver, @PV_Create),
               (@R_FleetManager, @P_Driver, @PV_Read),
               (@R_FleetManager, @P_Driver, @PV_Update),
               (@R_FleetManager, @P_Driver, @PV_Delete),

               (@R_FleetManager, @P_Vehicle, @PV_Create),
               (@R_FleetManager, @P_Vehicle, @PV_Read),
               (@R_FleetManager, @P_Vehicle, @PV_Update),
               (@R_FleetManager, @P_Vehicle, @PV_Delete),

               /* DME standard User */
               
               (@R_DMEStandardUser, @P_Orders, @PV_Read),
               
               (@R_DMEStandardUser, @P_Patient, @PV_Create),
               (@R_DMEStandardUser, @P_Patient, @PV_Read),
               (@R_DMEStandardUser, @P_Patient, @PV_Update),
               (@R_DMEStandardUser, @P_Patient, @PV_Delete),

               (@R_DMEStandardUser, @P_Hospice, @PV_Read),

               (@R_DMEStandardUser, @P_HospiceLocation, @PV_Read),

               (@R_DMEStandardUser, @P_Facility, @PV_Create),
               (@R_DMEStandardUser, @P_Facility, @PV_Read),
               (@R_DMEStandardUser, @P_Facility, @PV_Update),
               (@R_DMEStandardUser, @P_Facility, @PV_Delete),
               
               /* DME Admin */
               (@R_DMEAdmin, @P_Orders, @PV_Read),
              
               (@R_DMEAdmin, @P_Patient, @PV_Create),
               (@R_DMEAdmin, @P_Patient, @PV_Read),
               (@R_DMEAdmin, @P_Patient, @PV_Update),
               (@R_DMEAdmin, @P_Patient, @PV_Delete),

               (@R_DMEAdmin, @P_Hospice, @PV_Read),

               (@R_DMEAdmin, @P_HospiceLocation, @PV_Read),

               (@R_DMEAdmin, @P_Facility, @PV_Create),
               (@R_DMEAdmin, @P_Facility, @PV_Read),
               (@R_DMEAdmin, @P_Facility, @PV_Update),
               (@R_DMEAdmin, @P_Facility, @PV_Delete),

                /* Business Admin */
               (@R_BusinessAdmin, @P_Inventory, @PV_Read),
               (@R_BusinessAdmin, @P_Inventory, @PV_Update),
               (@R_BusinessAdmin, @P_Site, @PV_Read),
               (@R_BusinessAdmin, @P_User, @PV_Read),
               (@R_BusinessAdmin, @P_Hospice, @PV_Read),
               (@R_BusinessAdmin, @P_Facility, @PV_Read),
               (@R_BusinessAdmin, @P_HospiceLocation, @PV_Read),
               (@R_BusinessAdmin, @P_Dispatch, @PV_Read)
              
               

INSERT INTO [core].[RolePermissions]
SELECT a.* FROM @RolePermissionList a 
LEFT JOIN [core].[RolePermissions] b 
ON a.RoleId = b.RoleId and a.PermissionNounId = b.PermissionNounId and a.PermissionVerbId = b.PermissionVerbId
WHERE b.RoleId IS NULL

DELETE FROM [core].[RolePermissions]
FROM [core].[RolePermissions] b LEFT JOIN @RolePermissionList a ON a.RoleId = b.RoleId and a.PermissionNounId = b.PermissionNounId and a.PermissionVerbId = b.PermissionVerbId
WHERE a.RoleId IS NULL OR a.PermissionNounId IS NULL OR a.PermissionVerbId IS NULL;