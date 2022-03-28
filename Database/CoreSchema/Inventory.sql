CREATE TABLE [core].[Inventory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [SerialNumber] VARCHAR(MAX) NULL, 
    [QuantityAvailable] INT NOT NULL DEFAULT 0,
    [QuantityOnHand] INT NULL, 
    [StatusId] INT NOT NULL, 
    [CurrentLocationId] INT NOT NULL,
    [NetSuiteInventoryId] INT NULL,
    [LotNumber] VARCHAR(MAX) NULL,
    [LotExpirationDate] DATETIME2 NULL,
    [AssetTagNumber] VARCHAR(MAX) NULL,
    [CreatedByUserId] INT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedByUserId] INT NULL, 
    [DeletedDateTime] DATETIME2 NOT NULL,
    [AdditionalField1] VARCHAR(MAX) NULL, 
    [AdditionalField2] INT NULL, 
    CONSTRAINT [FK_Inventory_ToItems] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Inventory_ToInventoryStatusTypes] FOREIGN KEY ([StatusId]) REFERENCES [core].[InventoryStatusTypes]([Id]),
    CONSTRAINT [FK_Inventory_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Inventory_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Inventory_Users_Deleted] FOREIGN KEY ([DeletedByUserId]) REFERENCES [core].[Users]([Id]),
)
GO
    
CREATE NONCLUSTERED INDEX IX_NetsuiteInventoryId
ON [core].[Inventory]([NetSuiteInventoryId]);
