CREATE TABLE [dbo].[EventTaskTemplate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Data] [ntext] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)
GO
