CREATE TABLE [dbo].[Group](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Roles] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)
GO

CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](56) NOT NULL,
	[Created] [datetime] NOT NULL,
	[LogType] [varchar](16) NOT NULL,
	[Description] [nvarchar](512) NOT NULL,
	[Data] [ntext] NULL,
	[Entity] [varchar](128) NULL,
	[EntityID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)
)
GO

CREATE NONCLUSTERED INDEX [IX_Log_UserName] ON [dbo].[Log]
(
	[UserName] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Log_Entity] ON [dbo].[Log]
(
	[Entity] ASC,
	[EntityID] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Log_Created] ON [dbo].[Log]
(
	[Created] ASC
)
GO



