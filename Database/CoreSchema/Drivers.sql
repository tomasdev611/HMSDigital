CREATE TABLE [core].[Drivers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [CurrentSiteId] INT NULL DEFAULT NULL,
    [CurrentVehicleId] INT NULL DEFAULT NULL,
    [LastKnownLatitude] DECIMAL(11, 8) NULL, 
    [LastKnownLongitude] DECIMAL(11, 8) NULL,
    [LocationUpdatedDateTime] DATETIME2 NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL  , 
    [UpdatedDateTime] DATETIME2 NULL DEFAULT NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedByUserId] INT NULL, 
    [DeletedDateTime] DATETIME2 NULL, 
    CONSTRAINT [FK_Drivers_Users] FOREIGN KEY ([UserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Drivers_Sites] FOREIGN KEY ([CurrentSiteId]) REFERENCES [core].[Sites]([Id]),
    CONSTRAINT [FK_Drivers_Vehicle] FOREIGN KEY ([CurrentVehicleId]) REFERENCES [core].[Sites]([Id]),
    CONSTRAINT [FK_Drivers_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Drivers_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
)
Go
CREATE UNIQUE INDEX [AK_Drivers_Vehicle] ON [core].[Drivers] ([CurrentVehicleId])
WHERE [CurrentVehicleId] IS NOT NULL AND [IsDeleted] = 0;