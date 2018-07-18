﻿CREATE TABLE [dbo].[Tag]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Tag] NVARCHAR(128) NOT NULL
)
GO

CREATE NONCLUSTERED INDEX [IX_Tag_Tag] ON [dbo].[Tag]
(
	[Tag] ASC
)
GO
