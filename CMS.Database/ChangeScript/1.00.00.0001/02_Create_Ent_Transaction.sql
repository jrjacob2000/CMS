USE [CMS]
GO

/****** Object:  Table [dbo].[Ent_Transaction]    Script Date: 03/13/2015 20:01:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ent_Transaction](
	[Id] [uniqueidentifier] NOT NULL,
	[Reference] [nvarchar](250) NULL,
	[Notes] [nvarchar](250) NULL,
	[TransactionType] [nvarchar](50) NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[Amount] [money] NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Ent_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

