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

DECLARE @query nvarchar(1000);

SET @query = N'
ALTER SEQUENCE
    [core].[OrderNumberSequence]
RESTART WITH ' + CAST((SELECT  MAX(CAST(OrderNumber AS BIGINT)) + 1 FROM [core].[OrderHeaders] WHERE CAST(OrderNumber AS BIGINT) < 1000000000) AS NVARCHAR);

EXEC (@query);