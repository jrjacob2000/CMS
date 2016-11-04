USE [CMS]
GO

/****** Object:  Table [dbo].[Ent_Members]    Script Date: 03/03/2015 22:05:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Ent_Members](
	[Id] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](250) NULL,
	[MiddleName] [nvarchar](250) NULL,
	[LastName] [nvarchar](250) NULL,
	[Age] [int] NULL,
	[Gender] [nvarchar](50) NULL,
	[Birthday] [date] NULL,
	[MobilePhone] [nvarchar](50) NULL,
	[LandLine] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[MaritalStatus] [nvarchar](50) NULL,
	[NameOfSpouse] [nvarchar](50) NULL,
	[SpouseContact] [nvarchar](50) NULL,
	[ChildrenCount] [int] NULL,
	[MemberStatus] [nvarchar](50) NULL,
	[BaptizedDate] [date] NULL,
	[BaptizedPlace] [nvarchar](250) NULL,
	[BaptizedMinister] [nvarchar](250) NULL,
	[BelongsToGroups] [nvarchar](max) NULL,
	[Positions] [nvarchar](250) NULL,
 CONSTRAINT [PK_T_Members] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

