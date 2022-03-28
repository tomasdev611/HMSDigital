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
DECLARE @TransferRequestStatusTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @TransferRequestStatusTypeList VALUES
			   (1, 'Created'),
               (2, 'InTransit'),
			   (3, 'Completed'),
			   (4, 'AssignedToVehicle')

INSERT INTO [core].[TransferRequestStatusTypes]              
Select a.Id, a.Name from @TransferRequestStatusTypeList a 
	left join [core].[TransferRequestStatusTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null