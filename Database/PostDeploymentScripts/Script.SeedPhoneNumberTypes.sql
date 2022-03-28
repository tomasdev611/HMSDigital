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
DECLARE @PhoneNumberTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @PhoneNumberTypeList VALUES
			   (1, 'Work'),
               (2, 'Personal'),
               (3, 'Emergency'),
               (4, 'Fax')

INSERT INTO [patient].[PhoneNumberTypes]              
Select a.Id, a.Name from @PhoneNumberTypeList a 
	left join [patient].[PhoneNumberTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null

INSERT INTO [core].[PhoneNumberTypes]              
Select a.Id, a.Name from @PhoneNumberTypeList a 
	left join [core].[PhoneNumberTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null
