CREATE TABLE [core].[AddOnGroupProducts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GroupId] INT NOT NULL, 
    [ItemId] INT NULL,
    CONSTRAINT [FK_AddOnGroupProducts_Items] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]),
    CONSTRAINT [FK_AddOnGroupProducts_AddOnGroups] FOREIGN KEY ([GroupId]) REFERENCES [core].[AddOnGroups]([Id]) ON DELETE CASCADE,
)
