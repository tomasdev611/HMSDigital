CREATE TABLE [core].[AddOnGroups]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] VARCHAR(MAX) NOT NULL, 
    [AllowMultipleSelection] BIT NOT NULL DEFAULT 0, 
    [ItemId] INT NOT NULL,
	CONSTRAINT [FK_AddOnGroups_Items] FOREIGN KEY ([ItemId]) REFERENCES [core].[Items]([Id]) ON DELETE CASCADE,
)
