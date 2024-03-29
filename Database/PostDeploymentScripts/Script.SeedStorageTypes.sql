﻿/*
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
DECLARE @StorageTypeList TABLE (Id int, Name VARCHAR(MAX))
INSERT INTO @StorageTypeList VALUES
			   (1, 'AzureStorage')

INSERT INTO [core].[StorageTypes]              
Select a.Id, a.Name from @StorageTypeList a 
	left join [core].[StorageTypes] b 
		on a.Id = b.Id and a.Name = b.Name
	where b.Id is null and b.Name is null