SELECT DO_No, Do_Date, Customer, h.Name AS CustName, Qty, Amount 
FROM DO_Det d INNER JOIN Heads h ON d.Customer=h.Code
