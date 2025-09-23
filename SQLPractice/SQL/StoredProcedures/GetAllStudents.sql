IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL
    SELECT * FROM dbo.Students;
ELSE
    SELECT CAST(NULL AS INT) AS StudentId,
           CAST(NULL AS INT) AS SemesterFees,
           CAST(NULL AS NVARCHAR(50)) AS FirstName,
           CAST(NULL AS NVARCHAR(50)) AS LastName,
           CAST(NULL AS DATE) AS DateOfBirth,
           CAST(NULL AS NVARCHAR(100)) AS Email,
           CAST(NULL AS DATE) AS EnrollmentDate,
           CAST(NULL AS BIT) AS IsActive,
           CAST(NULL AS INT) AS DepartmentId
    WHERE 1 = 0;