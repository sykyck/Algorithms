BEGIN TRANSACTION;

BEGIN TRY
    INSERT INTO Students (FirstName, LastName, DepartmentId, SemesterFees, Email, DateOfBirth, IsActive)
    SELECT TOP 100000 
        'First' + CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS NVARCHAR),
        'Last' + CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS NVARCHAR),
        (ABS(CHECKSUM(NEWID())) % 5) + 1,
        (ABS(CHECKSUM(NEWID())) % 50000) + 10000,
        'email' + CAST(ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS NVARCHAR) + '@example.com',
        '2002-05-14',
        1
    FROM sys.all_objects a 
    CROSS JOIN sys.all_objects b;

    SELECT @@ROWCOUNT AS RowsInserted; -- returns number of rows inserted

    -- Commit only if insert succeeds
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    -- Rollback in case of any error
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    -- Rethrow error for debugging/logging
    THROW;
END CATCH;
