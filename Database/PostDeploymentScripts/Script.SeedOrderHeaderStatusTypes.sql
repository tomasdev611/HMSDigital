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
DECLARE @OrderStatusTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @OrderStatusTypeList VALUES
        (10, 'Planned'),
        (11, 'Scheduled'), 
        (12, 'Staged'), 
        (13, 'OnTruck'),
        (14, 'OutForFulfillment'),
        (15, 'Completed'),
        (16, 'BackOrdered'),
        (17, 'Cancelled'), 
        (18, 'Saved'), 
        (19, 'Pending Approval'),
        (20, 'Submitted'),
        (21, 'Partial Fulfillment' ),
        (22, 'PreLoad'),
        (23, 'Loading Truck'),
        (24, 'Enroute'),
        (25, 'OnSite')


INSERT INTO [core].[OrderHeaderStatusTypes]              
Select a.Id, a.Name from @OrderStatusTypeList a 
	left join [core].[OrderHeaderStatusTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null