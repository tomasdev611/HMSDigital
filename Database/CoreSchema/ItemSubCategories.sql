CREATE TABLE [core].[ItemSubCategories]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [CategoryId] INT NOT NULL,
    [NetSuiteSubCategoryId] INT NULL,
    CONSTRAINT [FK_ItemSubCategories_ToItemCategories] FOREIGN KEY ([CategoryId]) REFERENCES [core].[ItemCategories]([Id])
)
