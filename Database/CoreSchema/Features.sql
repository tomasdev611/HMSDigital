CREATE TABLE [core].[Features]
(
	[Id] INT NOT NULL PRIMARY KEY, 
	[Name] VARCHAR(255) NOT NULL, 
	[IsEnabled] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [AK_Features_Name] UNIQUE ([Name])
)
