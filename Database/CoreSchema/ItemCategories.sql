CREATE TABLE [core].[ItemCategories]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NOT NULL,
    [NetSuiteCategoryId] INT NULL,
	[CreatedByUserId] INT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [UpdatedByUserId] INT NULL, 
    [UpdatedDateTime] DATETIME2 NOT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
    [DeletedByUserId] INT NULL, 
    [DeletedDateTime] DATETIME2 NOT NULL,
    CONSTRAINT [FK_ItemCategories_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_ItemCategories_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_ItemCategories_Users_Deleted] FOREIGN KEY ([DeletedByUserId]) REFERENCES [core].[Users]([Id]),
)
