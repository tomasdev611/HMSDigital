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
DECLARE @CustomerTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @CustomerTypeList VALUES
			   (1, 'Direct'),
               (2, 'TPA')

INSERT INTO [core].[CustomerTypes]              
Select a.Id, a.Name from @CustomerTypeList a 
	left join [core].[CustomerTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null