use R1103_ContentCentre

select SpecificationItem.SpecificationID, SpecificationItem.Value, SPecificationiTem.SpecificationitemID 
from AllergenSpecItem
join SpecificationItem
on AllergenSpecItem.SpecificationItemID = SpecificationItem.SpecificationItemID where AllergenSpecItem.AllergenID = -1
order by Specificationitem.SpecificationItemID ASC