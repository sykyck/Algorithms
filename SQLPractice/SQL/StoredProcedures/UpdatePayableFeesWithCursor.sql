BEGIN TRANSACTION;

BEGIN TRY
    DECLARE @StudentId INT;
    DECLARE @SemesterFees INT;
    DECLARE @ScholarshipAmount INT;

    -- Cursor for students
    DECLARE student_cursor CURSOR FOR
    SELECT StudentId, SemesterFees
    FROM Students;

    OPEN student_cursor;

    FETCH NEXT FROM student_cursor INTO @StudentId, @SemesterFees;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Get total scholarship for this student
        SELECT @ScholarshipAmount = ISNULL(SUM(sch.Value), 0)
        FROM StudentScholarships ss
        INNER JOIN Scholarships sch ON ss.ScholarshipId = sch.ScholarshipId
        WHERE ss.StudentId = @StudentId;

        -- Update payable fees
        UPDATE Students
        SET PayableFees = CASE 
                             WHEN @SemesterFees - @ScholarshipAmount < 0 
                             THEN 0 
                             ELSE @SemesterFees - @ScholarshipAmount 
                          END
        WHERE StudentId = @StudentId;

        -- Fetch next student
        FETCH NEXT FROM student_cursor INTO @StudentId, @SemesterFees;
    END;

    CLOSE student_cursor;
    DEALLOCATE student_cursor;

    -- Show results
    SELECT StudentId, FirstName, LastName, SemesterFees, PayableFees
    FROM Students;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
