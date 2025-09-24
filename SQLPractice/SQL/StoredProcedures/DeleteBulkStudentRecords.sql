BEGIN TRANSACTION;

BEGIN TRY
    -- Delete all bulk-inserted students (keep first 5)
    DECLARE @DeletedRows INT;

    DELETE FROM Students
    WHERE StudentId > 5;

    SET @DeletedRows = @@ROWCOUNT;

    DBCC CHECKIDENT ('Students', RESEED, 5);

    SELECT @DeletedRows AS RowsDeleted;

    COMMIT TRANSACTION;
    PRINT '✅ Bulk test rows deleted successfully.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    PRINT '❌ Error while deleting bulk rows: ' + ERROR_MESSAGE();
END CATCH;
