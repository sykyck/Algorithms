BEGIN TRANSACTION;

BEGIN TRY
    IF OBJECT_ID('dbo.Students', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE dbo.Students DROP CONSTRAINT FK_Students_Departments;
        DROP TABLE dbo.Students;
    END

    CREATE TABLE dbo.Students
    (
        StudentId INT IDENTITY(1,1) PRIMARY KEY,
        SemesterFees INT NOT NULL,
        PayableFees INT NOT NULL,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        DateOfBirth DATE NOT NULL,
        Email NVARCHAR(100) UNIQUE,
        EnrollmentDate DATE NOT NULL DEFAULT GETDATE(),
        IsActive BIT NOT NULL DEFAULT 1,
        DepartmentId INT NOT NULL,
        CONSTRAINT FK_Students_Departments FOREIGN KEY (DepartmentID)
            REFERENCES dbo.Departments (DepartmentID)
    );

    INSERT INTO dbo.Students 
    (SemesterFees, PayableFees, FirstName, LastName, DateOfBirth, Email, EnrollmentDate, IsActive, DepartmentId)
    VALUES
    (50000, 50000, 'Alice', 'Johnson', '2002-05-14', 'alice.johnson@example.com', '2021-09-01', 1, 1),
    (25000, 25000, 'Bob', 'Smith', '2001-11-22', 'bob.smith@example.com', '2020-09-01', 1, 2),
    (30000, 30000, 'Charlie', 'Brown', '2003-01-10', 'charlie.brown@example.com', '2022-01-15', 1, 3),
    (35000, 35000, 'John', 'Smith', '2003-01-10', 'john.smith@example.com', '2022-01-15', 1, 4),
    (70000, 70000, 'Michael', 'Jackson', '2003-01-10', 'michael.jackson@example.com', '2022-01-15', 1, 5);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
