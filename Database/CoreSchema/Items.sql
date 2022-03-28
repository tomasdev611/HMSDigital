CREATE TABLE [core].[Items]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [ItemNumber] VARCHAR(MAX) NOT NULL,
    [IsAssetTagged] [bit] NOT NULL DEFAULT 0,
	[Depreciation] [decimal](18, 4) NULL,
	[AverageCost] [decimal](18, 4) NULL,
	[CogsAccountName] [varchar](MAX) NULL,
	[AvgDeliveryProcessingTime] [numeric](18, 0) NULL,
	[AvgPickUpProcessingTime] [numeric](18, 0) NULL,
    [NetSuiteItemId] INT NULL, 
    [IsSerialized] BIT NOT NULL DEFAULT 0, 
    [IsLotNumbered] BIT NOT NULL DEFAULT 0,
    [IsConsumable] BIT NOT NULL , 
    [IsDME] BIT NOT NULL , 
    [IsInactive] BIT NOT NULL DEFAULT 0, 
    [CreatedByUserId] INT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedByUserId] INT NULL, 
    [DeletedDateTime] DATETIME2 NOT NULL,
    CONSTRAINT [FK_Items_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Items_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Items_Users_Deleted] FOREIGN KEY ([DeletedByUserId]) REFERENCES [core].[Users]([Id]), 
)
GO

CREATE NONCLUSTERED INDEX IX_NetsuiteItemId
ON [core].[Items]([NetsuiteItemId]);

