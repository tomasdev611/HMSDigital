CREATE TABLE [core].[CreditHoldHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [HospiceId] INT NOT NULL, 
    [CreditHoldByUserId] INT NOT NULL, 
    [CreditHoldNote] VARCHAR(MAX) NULL, 
    [CreditHoldDateTime] DATETIME2 NOT NULL, 
    [IsCreditOnHold] BIT NOT NULL,
    CONSTRAINT [FK_Credit_Hold_By_User] FOREIGN KEY ([CreditHoldByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_Credit_Hold_History_Hospice] FOREIGN KEY ([HospiceId]) REFERENCES [core].[Hospices]([Id]),
)
