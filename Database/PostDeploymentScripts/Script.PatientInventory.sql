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
IF COL_LENGTH('core.PatientInventory', 'AdditionalField1') IS NOT NULL
BEGIN
    update [core].[PatientInventory] set [AdditionalField1] = NULL;
END

IF COL_LENGTH('core.PatientInventory', 'AdditionalField2') IS NOT NULL
BEGIN
    update [core].[PatientInventory] set [AdditionalField2] = NULL;
END
