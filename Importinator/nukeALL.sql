USE [R1103_ContentCentre]

declare @sID int;

declare @sIID int;

set @sIID = -1; -- 126500
set @sID = -1; -- 11730

delete from AllergenSpecItem where SpecificationItemID >= @sIID
delete from SpecificationItem where SpecificationID >= @sID
delete from Specification where SpecificationID >= @sID
delete from SpecificationSearch where specificationID >= @sID