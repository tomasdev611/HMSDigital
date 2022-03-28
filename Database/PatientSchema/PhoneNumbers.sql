CREATE TABLE [patient].[PhoneNumbers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PatientId] INT NOT NULL , 
    [CountryCode] INT NOT NULL, 
    [Number] BIGINT NOT NULL  , 
    [NumberTypeId] INT NOT NULL , 
    [IsPrimary] BIT NULL DEFAULT 0 , 
    [ReceiveEtaTextmessage] BIT NOT NULL DEFAULT 0, 
    [ReceiveSurveyTextMessage] BIT NOT NULL DEFAULT 0, 
    [ContactName] VARCHAR(MAX) NULL, 
    [IsSelfPhone] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_PhoneNumbers_Patients] FOREIGN KEY ([PatientId]) REFERENCES [patient].[PatientDetails]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PhoneNumbers_PhoneNumberType] FOREIGN KEY ([NumberTypeId]) REFERENCES [patient].[PhoneNumberTypes]([Id])
)
