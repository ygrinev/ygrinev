USE [DYNCONTROLDB]
GO
/****** Object:  StoredProcedure [dbo].[spGetCtrlList]    Script Date: 11/19/2015 11:48:34 AM ******/
--IF EXISTS DROP PROCEDURE [dbo].[spGetCtrlList]
--GO
--ALTER TABLE [dbo].[CtrlOrder] DROP CONSTRAINT [FK_CtrlOrder_ToCtrlPages]
--GO
--/****** Object:  Table [dbo].[CtrlPages]    Script Date: 11/19/2015 11:48:34 AM ******/
--DROP TABLE [dbo].[CtrlPages]
--GO
--/****** Object:  Table [dbo].[CtrlOrder]    Script Date: 11/19/2015 11:48:34 AM ******/
--DROP TABLE [dbo].[CtrlOrder]
--GO
--/****** Object:  Table [dbo].[Controls]    Script Date: 11/19/2015 11:48:34 AM ******/
--DROP TABLE [dbo].[Controls]
--GO
/****** Object:  Table [dbo].[Controls]    Script Date: 11/19/2015 11:48:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Controls](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Controls] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CtrlOrder]    Script Date: 11/19/2015 11:48:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CtrlOrder](
	[Id] [int] NOT NULL,
	[PageId] [int] NOT NULL,
	[Order] [int] NOT NULL,
	[ControlId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CtrlPages]    Script Date: 11/19/2015 11:48:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CtrlPages](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Controls] ON 

INSERT [dbo].[Controls] ([Id], [Name]) VALUES (1, N'TransactionDetails')
INSERT [dbo].[Controls] ([Id], [Name]) VALUES (2, N'OtherLawyer')
INSERT [dbo].[Controls] ([Id], [Name]) VALUES (3, N'Property')
INSERT [dbo].[Controls] ([Id], [Name]) VALUES (4, N'Builder')
INSERT [dbo].[Controls] ([Id], [Name]) VALUES (5, N'Vendors')
INSERT [dbo].[Controls] ([Id], [Name]) VALUES (6, N'Purchasers')
INSERT [dbo].[Controls] ([Id], [Name]) VALUES (7, N'Morgagors')
SET IDENTITY_INSERT [dbo].[Controls] OFF
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (1, 1, 1, 1)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (2, 1, 2, 2)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (3, 1, 3, 3)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (4, 1, 4, 4)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (5, 2, 3, 7)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (6, 2, 1, 6)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (7, 2, 2, 5)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (8, 3, 4, 1)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (9, 3, 6, 2)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (10, 3, 3, 3)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (11, 3, 1, 4)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (12, 3, 2, 5)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (13, 3, 7, 6)
INSERT [dbo].[CtrlOrder] ([Id], [PageId], [Order], [ControlId]) VALUES (14, 3, 5, 7)
INSERT [dbo].[CtrlPages] ([Id], [Name]) VALUES (1, N'LLC       ')
INSERT [dbo].[CtrlPages] ([Id], [Name]) VALUES (2, N'EASYFUND  ')
INSERT [dbo].[CtrlPages] ([Id], [Name]) VALUES (3, N'LLC/EASYFUND')
ALTER TABLE [dbo].[CtrlOrder]  WITH CHECK ADD  CONSTRAINT [FK_CtrlOrder_ToCtrlPages] FOREIGN KEY([PageId])
REFERENCES [dbo].[CtrlPages] ([Id])
GO
ALTER TABLE [dbo].[CtrlOrder] CHECK CONSTRAINT [FK_CtrlOrder_ToCtrlPages]
GO
/****** Object:  StoredProcedure [dbo].[spGetCtrlList]    Script Date: 11/19/2015 11:48:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spGetCtrlList]
	@BusinessModel int = 1
AS
	SELECT c.Name FROM dbo.Controls c
	JOIN dbo.CtrlOrder o ON o.ControlId = c.Id
	WHERE o.PageId = @BusinessModel
GO
