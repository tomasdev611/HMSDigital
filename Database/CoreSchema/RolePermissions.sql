CREATE TABLE [core].[RolePermissions]
(
    [RoleId] INT NOT NULL, 
    [PermissionNounId] INT NOT NULL, 
    [PermissionVerbId] INT NOT NULL, 
    CONSTRAINT [FK_RolePermission_ToRoles] FOREIGN KEY ([RoleId]) REFERENCES [core].[Roles]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_RolePermission_ToPermissionNouns] FOREIGN KEY ([PermissionNounId]) REFERENCES [core].[PermissionNouns]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_RolePermissions_ToPermissionVerbs] FOREIGN KEY ([PermissionVerbId]) REFERENCES [core].[PermissionVerbs]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId],[PermissionNounId],[PermissionVerbId])
)
