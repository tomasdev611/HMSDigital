CREATE TABLE [core].[ItemImages]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NOT NULL,
	[Url] NVARCHAR(MAX) NOT NULL,
	[CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NULL  , 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    CONSTRAINT [FK_ItemImages_ToItems] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ItemImages_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_ItemImages_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]),
)
