USE [R1103_ContentCentre]
GO
/****** Object:  StoredProcedure [dbo].[getAllAllergens]    Script Date: 11/24/2014 2:33:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter Procedure [dbo].[getAllAllergens]
as 
BEGIN
	SELECT AllergenID, Allergen FROM Allergens
END