CREATE TABLE [core].[PhoneNumbers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CountryCode] INT NOT NULL, 
    [Number] BIGINT NOT NULL  , 
    [IsVerified] BIT NOT NULL DEFAULT 0, 
    [NumberTypeId] INT NULL DEFAULT NULL ,
    [IsPrimary] BIT NULL ,
    [SKEntityType] VARCHAR(50) NULL, 
    [SKEntityID] INT NULL,
    [AdditionalField1] INT NULL DEFAULT NULL, 
    CONSTRAINT [FK_PhoneNumbers_PhoneNumberTypes] FOREIGN KEY ([NumberTypeId]) REFERENCES [core].[PhoneNumberTypes]([Id])
)
