CREATE TABLE [core].[ItemSubCategoryMapping]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ItemId] INT NULL, 
    [ItemSubCategoryId] INT NULL,
    CONSTRAINT [FK_ItemSubCategoryMapping_Items] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ItemSubCategoryMapping_ItemCategories] FOREIGN KEY ([ItemSubCategoryId]) REFERENCES [core].[ItemSubCategories]([Id]) ON DELETE CASCADE,
)
