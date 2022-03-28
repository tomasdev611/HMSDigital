CREATE TABLE [core].[Emails]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [EmailAddress] NVARCHAR(MAX) NOT NULL, 
    [IsVerified] BIT NOT NULL DEFAULT 0, 
    [EmailType] VARCHAR(50) NULL , 
    [IsPrimary] BIT NULL,
    [AdditionalField1] INT NULL, 
)
