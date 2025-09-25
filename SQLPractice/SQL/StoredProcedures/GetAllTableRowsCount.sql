BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @ScholarshipCount INT = 0;
    DECLARE @StudentCount INT = 0;
    DECLARE @DepartmentCount INT = 0;
    DECLARE @StudentScholarshipsCount INT = 0;

    -- Count Scholarships if table exists
    IF OBJECT_ID('dbo.Scholarships', 'U') IS NOT NULL
        SELECT @ScholarshipCount = COUNT(*) FROM dbo.Scholarships;

    -- Count Students if table exists
    IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL
        SELECT @StudentCount = COUNT(*) FROM dbo.Students;

    -- Count Departments if table exists
    IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL
        SELECT @DepartmentCount = COUNT(*) FROM dbo.Departments;

    -- Count StudentScholarships if table exists
    IF OBJECT_ID('dbo.StudentScholarships', 'U') IS NOT NULL
        SELECT @StudentScholarshipsCount = COUNT(*) FROM dbo.StudentScholarships;

    -- Return results
    SELECT 
        @ScholarshipCount AS ScholarshipCount,
        @StudentCount AS StudentCount,
        @DepartmentCount AS DepartmentCount,
        @StudentScholarshipsCount AS StudentScholarshipsCount;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT '❌ Error: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
