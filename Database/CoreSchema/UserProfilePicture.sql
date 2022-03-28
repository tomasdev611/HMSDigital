CREATE TABLE [core].[UserProfilePicture]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [FileMetadataId] INT NOT NULL, 
    [IsUploaded] BIT NOT NULL DEFAULT 0,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL  , 
    [UpdatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    [DownloadUrl] NVARCHAR(MAX) NULL, 
    [CacheExpiryDateTime] DATETIME2 NULL, 
    CONSTRAINT [AK_USERPROFILEPICTURE_FILEMETADATA] UNIQUE ([FileMetadataId]),
    CONSTRAINT [FK_UserProfilePicture_User] FOREIGN KEY ([UserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_UserProfilePicture_ToUsers_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_UserProfilePicture_ToUsers_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_UserProfilePicture_FiltesMetadata] FOREIGN KEY ([FileMetadataId]) REFERENCES [core].[FilesMetadata]([Id]) 
)
