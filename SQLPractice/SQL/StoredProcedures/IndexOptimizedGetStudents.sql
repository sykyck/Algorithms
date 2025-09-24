-- Enable statistics to see performance details
SET STATISTICS IO ON;
SET STATISTICS TIME ON;

BEGIN TRY
    BEGIN TRANSACTION;

    -- Step 1: Create indexes if they don't exist
    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Departments_Name')
        CREATE NONCLUSTERED INDEX IX_Departments_Name ON dbo.Departments(Name);

    IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Students_DeptId_Fees')
        CREATE NONCLUSTERED INDEX IX_Students_DeptId_Fees 
        ON dbo.Students(DepartmentId, SemesterFees);

    -- Step 2: Run optimized query
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

    -- Step 3: Drop the indexes to reset environment
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Departments_Name')
        DROP INDEX IX_Departments_Name ON dbo.Departments;

    IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Students_DeptId_Fees')
        DROP INDEX IX_Students_DeptId_Fees ON dbo.Students;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT '❌ Error: ' + ERROR_MESSAGE();
END CATCH;
