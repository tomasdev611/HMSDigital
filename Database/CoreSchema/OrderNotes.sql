CREATE TABLE [core].[OrderNotes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderHeaderId] INT NOT NULL, 
    [Note] NVARCHAR(MAX) NOT NULL,
    [NetSuiteOrderNoteId] INT NULL,
    [NetSuiteContactId] INT NULL, 
    [HospiceMemberId] INT NULL,
    [CreatedDateTime] DATETIME2 NOT NULL, 
    [CreatedByUserId] INT NOT NULL, 
    [UpdatedDateTime] DATETIME2 NULL, 
    [UpdatedByUserId] INT NULL,  
    CONSTRAINT [FK_OrderNotes_OrderHeaders] FOREIGN KEY ([OrderHeaderId]) REFERENCES [core].[OrderHeaders](Id) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderNotes_Users_Created] FOREIGN KEY ([CreatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_OrderNotes_Users_Updated] FOREIGN KEY ([UpdatedByUserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [FK_OrderNotes_HospiceMember] FOREIGN KEY ([HospiceMemberId]) REFERENCES [core].[HospiceMember]([Id]),
)
