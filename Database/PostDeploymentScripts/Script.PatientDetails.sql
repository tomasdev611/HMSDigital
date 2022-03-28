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
IF COL_LENGTH('patient.PatientDetails', 'AdditionalField3') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField3] = NULL;
END

IF COL_LENGTH('patient.PatientDetails', 'AdditionalField4') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField4] = NULL;
END

IF COL_LENGTH('patient.PatientDetails', 'AdditionalField5') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField5] = NULL;
END

IF COL_LENGTH('patient.PatientDetails', 'AdditionalField7') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField7] = NULL;
END

IF COL_LENGTH('patient.PatientDetails', 'AdditionalField8') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField8] = NULL;
END

IF COL_LENGTH('patient.PatientDetails', 'AdditionalField9') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField9] = NULL;
END

IF COL_LENGTH('patient.PatientDetails', 'AdditionalField10') IS NOT NULL
BEGIN
    update [patient].[PatientDetails] set [AdditionalField10] = NULL;
END
