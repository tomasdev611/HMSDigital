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
DECLARE @InventoryStatusTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @InventoryStatusTypeList VALUES
			   (1, 'Available'),
               (2, 'NotReady')

UPDATE b SET b.Name = a.Name 
from @InventoryStatusTypeList a 
	left join [core].[InventoryStatusTypes] b 
	on a.Id = b.Id
	where b.Name <> a.Name;

INSERT INTO [core].[InventoryStatusTypes]              
Select a.Id, a.Name from @InventoryStatusTypeList a 
	left join [core].[InventoryStatusTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null

DELETE FROM [core].[InventoryStatusTypes] where Id in(
Select b.Id from @InventoryStatusTypeList a 
	right join [core].[InventoryStatusTypes] b 
		on a.Id = b.Id 
	where a.Id is null);