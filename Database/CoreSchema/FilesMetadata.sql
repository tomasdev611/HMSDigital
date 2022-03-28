CREATE TABLE [core].[FilesMetadata]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NULL, 
    [Description] VARCHAR(MAX) NULL, 
    [ContentType] VARCHAR(MAX) NOT NULL, 
    [SizeInBytes] INT NOT NULL, 
    [StorageTypeId] INT NOT NULL, 
    [StorageRoot] VARCHAR(MAX) NOT NULL, 
    [StorageFilePath] VARCHAR(MAX) NOT NULL, 
    [IsPublic] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_FilesMetadata_ToStorageTypes] FOREIGN KEY ([StorageTypeId]) REFERENCES [core].[StorageTypes]([Id])
)
