use R1103_ContentCentre

declare @sID int = 126500; --126500
declare @sIID int = 11739; --11739

select * from AllergenSpecItem where SpecificationItemID > @sID
select * from SpecificationItem where SpecificationItemID > @sID 
select SpecificationID, Code, ValidDate, Rotation, Station, Class, StatusID from Specification where SpecificationID > @sIID order by SpecificationID
