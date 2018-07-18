CREATE TABLE [dbo].[Tag2Event]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [TagId] INT NOT NULL, 
    [EventId] INT NOT NULL, 
    CONSTRAINT [FK_Tag2Event_Tag] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id]),
    CONSTRAINT [FK_Tag2Event_Event] FOREIGN KEY ([EventId]) REFERENCES [Event]([Id])
)

GO

CREATE INDEX [IX_Tag2Event_Tag] ON [dbo].[Tag2Event] ([TagId])

GO

CREATE INDEX [IX_Tag2Event_Event] ON [dbo].[Tag2Event] ([EventId])

GO

CREATE TABLE [dbo].[Event2CompanySub]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [EventId] INT NOT NULL, 
    [CompanySubId] INT NOT NULL, 
    CONSTRAINT [FK_Event2CompanySub_Event] FOREIGN KEY ([EventId]) REFERENCES [Event]([Id]),
    CONSTRAINT [FK_Event2CompanySub_CompanySub] FOREIGN KEY ([CompanySubId]) REFERENCES [CompanySub]([Id])
)

GO

CREATE INDEX [IX_Event2CompanySub_Event] ON [dbo].[Event2CompanySub] ([EventId])

GO

CREATE INDEX [IX_Event2CompanySub_CompanySub] ON [dbo].[Event2CompanySub] ([CompanySubId])

GO
