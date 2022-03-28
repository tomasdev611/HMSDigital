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
IF COL_LENGTH('core.OrderLineItems', 'AdditionalField1') IS NOT NULL
BEGIN
    update [core].[OrderLineItems] set [AdditionalField1] = NULL;
END

IF COL_LENGTH('core.OrderLineItems', 'AdditionalField2') IS NOT NULL
BEGIN
    update [core].[OrderLineItems] set [AdditionalField2] = NULL;
END


IF COL_LENGTH('core.OrderLineItems', 'AdditionalField3') IS NOT NULL
BEGIN
    update [core].[OrderLineItems] set [AdditionalField3] = NULL;
END

IF COL_LENGTH('core.OrderLineItems', 'AdditionalField4') IS NOT NULL
BEGIN
    update [core].[OrderLineItems] set [AdditionalField4] = NULL;
END

IF COL_LENGTH('core.OrderLineItems', 'AdditionalField5') IS NOT NULL
BEGIN
    update [core].[OrderLineItems] set [AdditionalField5] = NULL;
END

IF COL_LENGTH('core.OrderLineItems', 'AdditionalField6') IS NOT NULL
BEGIN
    update [core].[OrderLineItems] set [AdditionalField6] = NULL;
END
