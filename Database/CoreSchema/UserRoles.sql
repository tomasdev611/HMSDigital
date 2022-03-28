CREATE TABLE [core].[UserRoles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL, 
    [ResourceId] VARCHAR(MAX) NOT NULL, 
    [ResourceType] VARCHAR(MAX) NOT NULL, 
    [RoleId] INT NULL, 
    CONSTRAINT [FK_UserRoles_ToUsers] FOREIGN KEY ([UserId]) REFERENCES [core].[Users]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_UserRoles_ToRole] FOREIGN KEY ([RoleId]) REFERENCES [core].[Roles]([Id]) ON DELETE CASCADE
)
