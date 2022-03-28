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
DECLARE @AddressTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @AddressTypeList VALUES
			   (1, 'Home'),
               (2, 'Facility')

INSERT INTO [patient].[AddressType]              
Select a.Id, a.Name from @AddressTypeList a 
	left join [patient].[AddressType] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null