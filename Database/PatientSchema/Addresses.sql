CREATE TABLE [patient].[Addresses]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AddressLine1] NVARCHAR(MAX) NULL, 
    [AddressLine2] NVARCHAR(MAX) NULL, 
    [AddressLine3] NVARCHAR(MAX) NULL, 
    [City] NVARCHAR(MAX) NULL, 
    [Country] NVARCHAR(MAX) NULL, 
    [State] NVARCHAR(MAX) NULL, 
    [County] NVARCHAR(MAX) NULL, 
    [ZipCode] INT NOT NULL, 
    [Latitude] DECIMAL(11, 8) NOT NULL DEFAULT 0.00000000, 
    [Longitude] DECIMAL(11, 8) NOT NULL DEFAULT 0.00000000,
    [Plus4Code] INT NULL, 
    [IsVerified] BIT NOT NULL DEFAULT 0, 
    [VerifiedBy] VARCHAR(MAX) NULL, 
    [AddressUUID] UNIQUEIDENTIFIER NULL 
)
