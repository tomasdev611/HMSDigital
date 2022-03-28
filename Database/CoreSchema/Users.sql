CREATE TABLE [core].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [FirstName] VARCHAR(MAX) NULL, 
    [LastName] VARCHAR(MAX) NULL, 
    [Email] VARCHAR(MAX) NULL, 
    [IsEmailVerified] BIT DEFAULT 0 NOT NULL,
    [PhoneNumber] BIGINT NOT NULL, 
    [IsPhoneNumberVerified] BIT DEFAULT 0 NOT NULL,
    [CountryCode] INT NOT NULL, 
    [CognitoUserId] VARCHAR(MAX) NULL, 
    [SKLocationID] INT NULL, 
    [SKRoleID] INT NULL, 
    [LockoutEnd] DATETIME2 NULL DEFAULT NULL,
    [OtpVerifyFailCount] INT NOT NULL DEFAULT 0, 
    [DisabledByUserId] INT NULL DEFAULT null, 
    [IsDisabled] BIT NULL DEFAULT 0,
    [CreatedByUserId] INT NULL DEFAULT null,
    [CreatedDateTime] DATETIME2 NULL DEFAULT NULL, 
    [UpdatedDateTime] DATETIME2 NULL DEFAULT NULL, 
    [UpdatedByUserId] INT NULL DEFAULT NULL,
    CONSTRAINT [FK_Created_User] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]), 
    CONSTRAINT [FK_Disabled_User] FOREIGN KEY ([DisabledByUserId]) REFERENCES [core].[Users]([Id]) 
)
