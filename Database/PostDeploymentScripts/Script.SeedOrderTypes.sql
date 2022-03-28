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
DECLARE @OrderTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @OrderTypeList VALUES
			   (10, 'Delivery'),
               (11, 'Exchange'),
			   (12, 'Move'),
			   (13, 'Respite'),
			   (14, 'Pickup')

UPDATE b SET b.Name = a.Name 
from @OrderTypeList a 
	left join [core].[OrderTypes] b 
	on a.Id = b.Id
	where b.Name <> a.Name;

INSERT INTO [core].[OrderTypes]              
Select a.Id, a.Name from @OrderTypeList a 
	left join [core].[OrderTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null

DELETE FROM [core].[OrderTypes] where Id in(
Select b.Id from @OrderTypeList a 
	right join [core].[OrderTypes] b 
		on a.Id = b.Id 
	where a.Id is null);