-- Enable statistics to see performance details
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Run unoptimized query
    SELECT 
        s.StudentId, 
        s.FirstName, 
        s.LastName, 
        d.Name AS DepartmentName, 
        s.SemesterFees
    FROM dbo.Students s
    INNER JOIN dbo.Departments d ON s.DepartmentId = d.DepartmentId
    WHERE d.Name = 'Computer Science'
      AND s.SemesterFees > 59900;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT '❌ Error: ' + ERROR_MESSAGE();
END CATCH;
