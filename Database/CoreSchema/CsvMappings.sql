CREATE TABLE [core].[CsvMappings]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HospiceId] INT NOT NULL,
    [MappingInJson] NVARCHAR(MAX) NULL,
    [MappingType] VARCHAR(50) NOT NULL,
    [MappedTable] VARCHAR(50) NOT NULL,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL  , 
    [UpdatedDateTime] DATETIME2 NULL DEFAULT NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL, 
    CONSTRAINT [FK_CsvMappings_CreatedUsers] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_CsvMappings_UpdatedUsers] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_CsvMappings_Hospice] FOREIGN KEY ([HospiceId]) REFERENCES [core].[Hospices]([Id]), 
)
