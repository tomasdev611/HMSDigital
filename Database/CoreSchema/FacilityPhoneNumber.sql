CREATE TABLE [core].[FacilityPhoneNumber]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FacilityId] INT NULL, 
    [PhoneNumberId] INT NULL, 
    CONSTRAINT [FK_FacilityPhoneNumber_Facility] FOREIGN KEY ([FacilityId]) REFERENCES [core].[Facilities]([Id]), 
    CONSTRAINT [FK_FacilityPhoneNumber_PhoneNumber] FOREIGN KEY ([PhoneNumberId]) REFERENCES [core].[PhoneNumbers]([Id])
)
