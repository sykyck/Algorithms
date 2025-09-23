BEGIN TRANSACTION;

BEGIN TRY
    -- Drop existing table if it exists
    IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL
        DROP TABLE dbo.Departments;

    -- Create Departments table
    CREATE TABLE dbo.Departments
    (
        DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        ModifiedDate DATETIME NOT NULL DEFAULT GETDATE()
    );

    -- Insert seed data
    INSERT INTO dbo.Departments (Name, ModifiedDate)
    VALUES
        ('Computer Science', GETDATE()),
        ('Mechanical', GETDATE()),
        ('Electronics', GETDATE()),
        ('Electrical', GETDATE()),
        ('Robotics', GETDATE());

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
