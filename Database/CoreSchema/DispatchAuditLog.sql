CREATE TABLE [core].[DispatchAuditLog]
(
    [AuditUuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	[AuditData] NVARCHAR(MAX) NULL,
    [AuditAction] VARCHAR(50) NOT NULL,
	[UserId] INT NULL DEFAULT NULL,
    [EntityId] INT NOT NULL,
    [AuditDate] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
    [ClientIPAddress] VARCHAR(50) NULL,
    [PatientUuid] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_DispatchAuditLog_User] FOREIGN KEY ([UserId]) REFERENCES [core].[Users]([Id]),
    CONSTRAINT [PK_DispatchAuditLog] PRIMARY KEY ([AuditUuid])
)
