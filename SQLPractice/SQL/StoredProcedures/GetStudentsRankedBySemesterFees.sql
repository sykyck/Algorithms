SELECT 
    s.StudentId,
    s.FirstName,
    s.LastName,
    d.Name AS DepartmentName,
    s.SemesterFees,
    DENSE_RANK() OVER (ORDER BY s.SemesterFees DESC) AS FeeDenseRank,
    RANK() OVER (ORDER BY s.SemesterFees DESC) AS FeeRank,
    ROW_NUMBER () OVER (ORDER BY s.SemesterFees DESC) AS RowNumber
FROM dbo.Students s
INNER JOIN dbo.Departments d
    ON s.DepartmentID = d.DepartmentID
ORDER BY FeeRank, s.StudentId;