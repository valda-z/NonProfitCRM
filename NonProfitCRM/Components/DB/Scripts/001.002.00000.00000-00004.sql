ALTER TABLE [dbo].[Event]
    ADD [TypeId] INT DEFAULT 0 NOT NULL;
GO

ALTER TABLE [dbo].[Event] WITH NOCHECK
    ADD CONSTRAINT [FK_Event_EventType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[EventType] ([Id]);
GO

ALTER TABLE [dbo].[Event] WITH CHECK CHECK CONSTRAINT [FK_Event_EventType];
GO

ALTER TABLE [dbo].[NonProfitOrg]
    ADD [TypeId] INT DEFAULT 0 NOT NULL;
GO

ALTER TABLE [dbo].[NonProfitOrg] WITH NOCHECK
    ADD CONSTRAINT [FK_NonProfitOrg_NonProfitOrgType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[NonProfitOrgType] ([Id]);
GO

ALTER TABLE [dbo].[NonProfitOrg] WITH CHECK CHECK CONSTRAINT [FK_NonProfitOrg_NonProfitOrgType];
GO

