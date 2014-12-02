USE [R1103_ContentCentre]
GO

/****** Object:  Table [dbo].[Allergens]    Script Date: 11/24/2014 4:28:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Allergens](
	[AllergenID] [int] IDENTITY(1,1) NOT NULL,
	[Allergen] [nvarchar](max) NULL,
 CONSTRAINT [PK_Allergens] PRIMARY KEY CLUSTERED 
(
	[AllergenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

USE [R1103_ContentCentre]
GO

/****** Object:  Table [dbo].[AllergenSpecItem]    Script Date: 11/24/2014 4:30:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AllergenSpecItem](
	[SpecificationItemID] [int] NULL,
	[AllergenID] [int] NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[AllergenSpecItem]  WITH CHECK ADD  CONSTRAINT [FK_Table_1_Allergens] FOREIGN KEY([AllergenID])
REFERENCES [dbo].[Allergens] ([AllergenID])
GO

ALTER TABLE [dbo].[AllergenSpecItem] CHECK CONSTRAINT [FK_Table_1_Allergens]
GO

ALTER TABLE [dbo].[AllergenSpecItem]  WITH CHECK ADD  CONSTRAINT [FK_Table_1_SpecificationItem] FOREIGN KEY([SpecificationItemID])
REFERENCES [dbo].[SpecificationItem] ([SpecificationItemID])
GO

ALTER TABLE [dbo].[AllergenSpecItem] CHECK CONSTRAINT [FK_Table_1_SpecificationItem]
GO

