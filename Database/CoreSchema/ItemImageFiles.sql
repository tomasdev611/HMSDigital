CREATE TABLE [core].[ItemImageFiles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL, 
    [FileMetadataId] INT NOT NULL, 
    [IsUploaded] BIT NOT NULL DEFAULT 0,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL  , 
    [UpdatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    CONSTRAINT [AK_IMAGE_FILEMETADATA] UNIQUE ([FileMetadataId]),
    CONSTRAINT [FK_ItemImageFiles_ToUsers_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_ItemImageFiles_ToUsers_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_ItemImageFiles_ToItems] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]), 
    CONSTRAINT [FK_ItemImageFiles_ToFilesMetadata] FOREIGN KEY ([FileMetadataId]) REFERENCES [core].[FilesMetadata]([Id]) ON DELETE CASCADE
)
