CREATE TABLE [notification].[PushNotificationMetadata]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DeviceId] VARCHAR(MAX) NOT NULL, 
    [InstallationId] UNIQUEIDENTIFIER NOT NULL, 
    [Platform] VARCHAR(MAX) NOT NULL, 
    [UserId] INT NULL, 
    [CurrentSiteId] INT NULL

)
