CREATE TABLE [core].[SiteMembers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SiteId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
    [Designation] VARCHAR(MAX) NULL,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL  , 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    CONSTRAINT [FK_SiteMembers_ToUsers_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_SiteMembers_ToUsers_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_SiteMembers_Sites] FOREIGN KEY ([SiteId]) REFERENCES [core].[Sites]([Id]),
    CONSTRAINT [FK_SiteMembers_Users] FOREIGN KEY ([UserId]) REFERENCES [core].[Users]([Id]) ON DELETE CASCADE,
)
