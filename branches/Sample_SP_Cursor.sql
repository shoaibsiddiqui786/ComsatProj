

declare @ItemGroupID int
set @ItemGroupID=13

declare @ItemID int
set @ItemID=0
declare @Qty_In Float, @Qty_Out Float
set @Qty_In=0
set @Qty_Out=0

declare cur_Item cursor for 
select ItemId from Item where ItemGroupId=@ItemGroupID

open cur_Item
fetch next from cur_Item into @ItemID
while @@FETCH_STATUS=0 
begin
	 --print 'Item ID ' + Cast(@ItemID as Varchar(10))
	 
	 select @Qty_In= SUM(ISNULL(Qty_In,0)), @Qty_Out= SUM(ISNULL(Qty_Out,0))  
	 from Item_Sec where Code=@ItemID
	 
	 print 'Item ID : ' + Cast(@ItemID as Varchar(10)) + ' Qty In : ' + Cast(@Qty_In as Varchar(10))
		+ ' Qty Out : ' + Cast(@Qty_Out as Varchar(10))
	 
	 fetch next from cur_Item into @ItemID
end 

close cur_Item
deallocate cur_Item

--select * from Item_Sec
