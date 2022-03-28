CREATE TABLE [core].[UserVerify]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NULL,
    [Email] NVARCHAR(MAX) NULL, 
    [PhoneNumber] BIGINT NULL,
    [Nonce] NVARCHAR(MAX) NOT NULL,
    [ExpiryDateTime] DATETIME2 NOT NULL, 
    [CreatedDateTime] DATETIME2 NOT NULL
)
