CREATE TABLE [core].[SiteServiceAreas]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SiteId] INT NULL, 
    [ZipCode] INT NULL, 
    [AdditionalField1] DECIMAL(11, 8) NULL DEFAULT NULL, 
    [AdditionalField2] DECIMAL(11, 8) NULL DEFAULT NULL,
    [CreatedByUserId] INT NULL DEFAULT NULL,
    [CreatedDateTime] DATETIME2 NULL DEFAULT NULL, 
    [UpdatedDateTime] DATETIME2 NULL DEFAULT NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL, 
    CONSTRAINT [FK_SiteServiceAreas_ToSites] FOREIGN KEY ([SiteId]) REFERENCES [core].[Sites]([Id]),
    CONSTRAINT [FK_SiteServiceAreas_Created_User] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_SiteServiceAreas_Updated_User] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]) 
)
