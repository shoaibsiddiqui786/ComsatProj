CREATE PROCEDURE sp_GroupUsers
	@UserID int
as
/*
DECLARE @UserID int
SET @UserID=1
*/
DECLARE @GroupID int, @Checked int, @GroupName Varchar(50), @GroupUserID int
SET @GroupID=0

SELECT groupid, 0 AS Checked, GroupName INTO #TmpTable FROM Groups WHERE Disable=1

DECLARE cur_GUsers CURSOR FOR 
SELECT groupid, 0 AS Checked, GroupName  FROM Groups WHERE Disable=1

OPEN cur_GUsers
FETCH next FROM cur_GUsers INTO @GroupID, @Checked, @GroupName
WHILE @@FETCH_STATUS=0
BEGIN
	SET @GroupUserID=0
	
	SELECT @GroupUserID=GroupUserID 
	  FROM GroupUsers WHERE GroupID=@GroupID AND UserID=@UserID
	
	IF @GroupUserID>0 
	BEGIN
		UPDATE #TmpTable SET Checked = 1 WHERE groupID = @GroupID
	END
	
	FETCH next FROM cur_GUsers INTO @GroupID, @Checked, @GroupName
END
CLOSE cur_GUsers
DEALLOCATE cur_GUsers

SELECT * FROM #TmpTable
DROP TABLE #TmpTable
