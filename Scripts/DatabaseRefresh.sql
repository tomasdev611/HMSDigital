CREATE EXTERNAL DATA SOURCE RandomNamesDataSource
WITH 
(
    TYPE = BLOB_STORAGE,
    LOCATION = 'https://hmsdstoragedev.blob.core.windows.net/scrambled-names'
);
CREATE TABLE dbo.RandomNames (FirstName VARCHAR(MAX),LastName VARCHAR(MAX));
BULK INSERT dbo.RandomNames
FROM 'RandomNames.csv'
WITH (DATA_SOURCE = 'RandomNamesDataSource',
      FORMAT = 'CSV');
GO
CREATE OR ALTER VIEW Get_NewId
AS
SELECT NEWID() AS NewId
GO

CREATE OR ALTER FUNCTION fn_GetRandomName(@type VARCHAR(10))
RETURNS VARCHAR(50)
AS
BEGIN
DECLARE @result VARCHAR(50);   
IF(LOWER(@type) = 'lastname')
		SET @result = (SELECT TOP(1) UPPER(LEFT(LastName,1))+LOWER(SUBSTRING(LastName,2,LEN(LastName))) FROM dbo.RandomNames ORDER BY (SELECT [NewId] FROM dbo.Get_NewId));
ELSE
		SET @result = (SELECT TOP(1) UPPER(LEFT(FirstName,1))+LOWER(SUBSTRING(FirstName,2,LEN(FirstName))) FROM dbo.RandomNames ORDER BY (SELECT [NewId] FROM dbo.Get_NewId));
 RETURN @result
End
GO

-- Patient data scramble
-- Scrambling First Name, Last Name, Date of Birth and Phone Numbers for patients
UPDATE patient.PatientDetails 
	SET FirstName = dbo.fn_GetRandomName('firstname'),
		LastName = dbo.fn_GetRandomName('lastname'),
		DateOfBirth = DATEADD(DAY, ABS(CHECKSUM(NEWID()) % 36500), '1900-01-01');

UPDATE patient.PhoneNumbers SET Number = CONCAT('555100',LEFT(ABS(CHECKSUM(NEWID())),4));

-- User data scramble 
-- Scrambling First Name, Last Name, Phone Number, Email for Users other than internal and test users
UPDATE core.Users  
		SET
			FirstName = dbo.fn_GetRandomName('firstname'),
			LastName = dbo.fn_GetRandomName('lastname'),
			PhoneNumber = CONCAT('555100',LEFT(ABS(CHECKSUM(NEWID())),4))
		WHERE (Email NOT LIKE '%@hospicesource%' 
				AND Email NOT LIKE '%@suitecentric%' 
				AND Email NOT LIKE '%@bushel%'
				AND Email NOT LIKE '%@nimBOLD%'
				AND Email NOT LIKE '%@4790118%'
				AND Email NOT LIKE '%@example%');

UPDATE core.Users  
		SET
			Email = CONCAT(FirstName,LastName,'@example.com')
		WHERE (Email NOT LIKE '%@hospicesource%' 
				AND Email NOT LIKE '%@suitecentric%' 
				AND Email NOT LIKE '%@bushel%'
				AND Email NOT LIKE '%@nimBOLD%'
				AND Email NOT LIKE '%@4790118%'
				AND Email NOT LIKE '%@example%');

-- setting CognitoUserId NULL as existing CognitoUserId refers to prod user pool. System tool to create Cognito users should be run to recreate this users in cognito
UPDATE core.Users SET CognitoUserId = NULL;

-- setting NetSuiteContactId NULL as existing Contact Ids refers to prod.System tool to create netsuite contact should be run to recreate netsuite contacts
UPDATE core.HospiceMember SET NetSuiteContactId = NULL;

UPDATE core.HospiceLocationMembers SET NetSuiteContactId = NULL;

DROP TABLE dbo.RandomNames;

DROP EXTERNAL DATA SOURCE RandomNamesDataSource