USE [R1103_ContentCentre]

delete from AllergenSpecItem where SpecificationItemID > 126500
delete from SpecificationItem where SpecificationID >= 11739
delete from Specification where SpecificationID > 11755