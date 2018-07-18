CREATE TABLE [dbo].[TaskStatus]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(128) NOT NULL,
	[Color] NVARCHAR(50) NOT NULL
)
GO

INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [Color]) VALUES (0, N'Nový', N'default')
INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [Color]) VALUES (1, N'Rozpracovaný', N'info')
INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [Color]) VALUES (1000, N'Dokončený', N'success')
INSERT INTO [dbo].[TaskStatus] ([Id], [Name], [Color]) VALUES (-1, N'Blokovaný', N'danger')
GO

CREATE TABLE [dbo].[Task]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
	[StatusId] INT NOT NULL,
    [EntityId] INT NOT NULL, 
    [Entity] VARCHAR(64) NOT NULL, 
	[DueDate] DATETIME NOT NULL,
    [Description] NTEXT NULL, 
    [Note] NTEXT NULL, 
    [AssignedTo] NVARCHAR(50) NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Updated] DATETIME NOT NULL,
	CONSTRAINT [FK_Task_Status] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[TaskStatus] ([Id])
)

GO

CREATE INDEX [IX_Task_EntityIdEntity] ON [dbo].[Task] ([EntityId], [Entity])

GO

CREATE INDEX [IX_Task_DueDate] ON [dbo].[Task] ([DueDate])

GO
