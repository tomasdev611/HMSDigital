CREATE TABLE [core].[Roles]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(MAX) NULL, 
    [Level] INT NULL, 
    [IsStatic] BIT NOT NULL DEFAULT 0, 
    [RoleType] VARCHAR(MAX) NOT NULL DEFAULT 'Internal'
)
