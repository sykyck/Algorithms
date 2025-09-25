BEGIN TRANSACTION;

BEGIN TRY
    IF OBJECT_ID('dbo.Scholarships', 'U') IS NOT NULL
        DROP TABLE dbo.Scholarships;

    CREATE TABLE dbo.Scholarships
    (
        ScholarshipId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(50) NOT NULL,
        Value INT NOT NULL
    );

    INSERT INTO dbo.Scholarships (Name, Value)
    VALUES
    ('Merit Scholarship', 10000),
    ('LowerCaste Help Scholarship', 5000);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
