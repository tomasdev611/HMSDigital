CREATE TABLE [core].[Facilities]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NULL, 
    [HospiceId] int NOT NULL,
    [HospiceLocationId] int NULL DEFAULT NULL,
    [AddressId] int NULL DEFAULT NULL,
    [IsDisable] BIT NOT NULL DEFAULT 0 ,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NULL   , 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    [SiteId] int NULL DEFAULT NULL,
    CONSTRAINT [FK_Facilities_ToHospices] FOREIGN KEY ([HospiceId]) REFERENCES [core].[Hospices]([Id]),
    CONSTRAINT [FK_Facilities_ToHospiceLocations] FOREIGN KEY ([HospiceLocationId]) REFERENCES [core].[HospiceLocations]([Id]),
    CONSTRAINT [FK_Facilities_ToUsers_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Facilities_ToUsers_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_Facilities_Address] FOREIGN KEY ([AddressId]) REFERENCES [core].[Addresses]([Id]), 
    CONSTRAINT [FK_Facilities_Sites] FOREIGN KEY ([SiteId]) REFERENCES [core].[Sites]([Id])
)
