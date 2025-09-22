BEGIN TRANSACTION;

BEGIN TRY
    IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL
        DROP TABLE dbo.Students;

    CREATE TABLE dbo.Students
    (
        StudentId INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        DateOfBirth DATE NOT NULL,
        Email NVARCHAR(100) UNIQUE,
        EnrollmentDate DATE NOT NULL DEFAULT GETDATE(),
        IsActive BIT NOT NULL DEFAULT 1
    );

    INSERT INTO dbo.Students (FirstName, LastName, DateOfBirth, Email, EnrollmentDate, IsActive)
    VALUES
    ('Alice', 'Johnson', '2002-05-14', 'alice.johnson@example.com', '2021-09-01', 1),
    ('Bob', 'Smith', '2001-11-22', 'bob.smith@example.com', '2020-09-01', 1),
    ('Charlie', 'Brown', '2003-01-10', 'charlie.brown@example.com', '2022-01-15', 1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
