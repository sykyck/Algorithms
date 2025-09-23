IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL
    SELECT * FROM dbo.Departments;
ELSE
    SELECT CAST(NULL AS INT) AS DepartmentId, 
           CAST(NULL AS NVARCHAR(100)) AS Name, 
           CAST(NULL AS DATETIME) AS ModifiedDate
    WHERE 1 = 0; -- return empty result set
