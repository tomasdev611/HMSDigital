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
:r Script.SeedRolePermissions.sql
:r Script.SeedAddressType.sql
:r Script.SeedPhoneNumberTypes.sql
:r Script.SeedInventoryStatusTypes.sql
:r Script.SeedStorageTypes.sql
:r Script.SeedTransferRequestStatusTypes.sql
:r Script.SeedPatientStatusTypes.sql
:r Script.SeedOrderTypes.sql
:r Script.SeedOrderHeaderStatusTypes.sql
:r Script.SeedOrderLineItemStatusTypes.sql
:r Script.SeedFeatures.sql
:r Script.Users.sql
:r Script.OrderLineItems.sql
:r Script.PatientDetails.sql
:r Script.FacilityPatient.sql
:r Script.FacilityPatientHistory.sql
:r Script.PatientInventory.sql
:r Script.HospiceMember.sql
:r Script.SeedCustomerTypes.sql
:r Script.Inventory.sql
:r Script.SiteServiceAreas.sql
:r Script.SeedHms2HmsDigitalMapping.sql

:r 0203202115333012_UpdatePatientStatusAndReason.sql

:r 1003202117422146_UpdateAuditIdValueAsNull.sql

:r Script.OrderHeaders.sql

:R 08132021192614687_UpdatePatientStatusReason.sql

:r .\1808202111200085_UpdateMemberIdInOrderAndOrderNotes.sql

:r .\2508202111543117_FixAddressUUID.sql
