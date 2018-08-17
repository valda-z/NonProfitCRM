ALTER TABLE [dbo].[Company]
    ADD [Insurance] BIT DEFAULT 0 NOT NULL;

GO

ALTER TABLE [dbo].[Event]
    ADD [Insurance] BIT DEFAULT 0 NOT NULL;

GO
