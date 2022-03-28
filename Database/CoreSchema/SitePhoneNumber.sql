CREATE TABLE [core].[SitePhoneNumber]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SiteId] INT NULL, 
    [PhoneNumberId] INT NULL, 
    CONSTRAINT [FK_SitePhoneNumber_Sites] FOREIGN KEY ([SiteId]) REFERENCES [core].[Sites]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_SitePhoneNumber_PhoneNumbers] FOREIGN KEY ([PhoneNumberId]) REFERENCES [core].[PhoneNumbers]([Id])

)
