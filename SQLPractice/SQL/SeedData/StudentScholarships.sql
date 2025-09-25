BEGIN TRANSACTION;

BEGIN TRY
    -- Drop the table if it already exists
    IF OBJECT_ID('dbo.StudentScholarships', 'U') IS NOT NULL
        DROP TABLE dbo.StudentScholarships;

    CREATE TABLE dbo.StudentScholarships
    (
        StudentId INT NOT NULL,
        ScholarshipId INT NOT NULL,
        AwardedDate DATE NOT NULL DEFAULT GETDATE(),
        -- Composite Primary Key
        CONSTRAINT PK_StudentScholarships PRIMARY KEY (StudentId, ScholarshipId),

        -- Foreign Keys
        CONSTRAINT FK_StudentScholarships_Students FOREIGN KEY (StudentId)
            REFERENCES dbo.Students(StudentId) ON DELETE CASCADE,
        CONSTRAINT FK_StudentScholarships_Scholarships FOREIGN KEY (ScholarshipId)
            REFERENCES dbo.Scholarships(ScholarshipId) ON DELETE CASCADE
    );

     -- Insert Mapping (Student ↔ Scholarship)
    -- Assuming ScholarshipId = 1 = Merit, 2 = LowerCaste
    INSERT INTO dbo.StudentScholarships (StudentId, ScholarshipId)
    VALUES
    (1, 1), -- Alice → Merit
    (2, 2), -- Bob → LowerCaste
    (3, 1), -- Charlie → Merit
    (4, 1), -- John → Merit
    (5, 2); -- Michael → LowerCaste

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
