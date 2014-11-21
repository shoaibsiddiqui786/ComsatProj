SELECT d.*, h.Name AS CustName
FROM DO_Det d INNER JOIN Heads h ON d.Customer=h.Code
WHERE d.DO_No = '01-1-135-351-1'
