USE [R1103_ContentCentre]

declare @sID int;

declare @sIID int;

set @sIID = 0; -- 126500
set @sID = 0; -- 11730

delete from AllergenSpecItem where SpecificationItemID >= @sIID
delete from SpecificationItem where SpecificationID >= @sID
delete from Specification where SpecificationID >= @sID
--delete from SpecificationSearch where specificationID >= @sID