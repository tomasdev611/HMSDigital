CREATE TABLE [core].[ItemCategoryMapping]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NULL, 
    [ItemCategoryId] INT NULL,
    CONSTRAINT [FK_ItemCategoryMapping_Items] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ItemCategoryMapping_ItemCategories] FOREIGN KEY ([ItemCategoryId]) REFERENCES [core].[ItemCategories]([Id]) ON DELETE CASCADE,
)
