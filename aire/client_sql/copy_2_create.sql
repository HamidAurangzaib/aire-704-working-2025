CREATE PROCEDURE dbo.PublishStagingToLive_Copy
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @stagingRows INT;
    SELECT @stagingRows = COUNT(*) FROM dbo.comprGOOGLCOPY_Staging;
    IF @stagingRows = 0
    BEGIN
        RAISERROR('comprGOOGLCOPY_Staging is empty - publish aborted to prevent data loss. Run Transfer first.', 16, 1);
        RETURN;
    END
    PRINT 'Safety check passed: ' + CAST(@stagingRows AS NVARCHAR) + ' rows in staging.';

    PRINT 'Phase 1: Building indexes on staging...';

    DECLARE @liveObjId   INT = OBJECT_ID('dbo.comprGOOGLCOPY');
    DECLARE @idxName     NVARCHAR(128);
    DECLARE @isUnique    BIT;
    DECLARE @isClustered BIT;
    DECLARE @keyCols     NVARCHAR(MAX);
    DECLARE @inclCols    NVARCHAR(MAX);
    DECLARE @sql         NVARCHAR(MAX);

    DECLARE idx_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT i.name, i.is_unique,
               CASE i.type WHEN 1 THEN 1 ELSE 0 END AS is_clustered
        FROM sys.indexes i
        WHERE i.object_id = @liveObjId
          AND i.type IN (1, 2)
          AND i.is_primary_key = 0
          AND i.is_unique_constraint = 0
        ORDER BY i.type;

    OPEN idx_cursor;
    FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        IF EXISTS (
            SELECT 1 FROM sys.indexes
            WHERE object_id = OBJECT_ID('dbo.comprGOOGLCOPY_Staging')
              AND name = @idxName
        )
        BEGIN
            FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;
            CONTINUE;
        END

        SET @keyCols = '';
        SELECT @keyCols = @keyCols +
            QUOTENAME(c.name) +
            CASE ic.is_descending_key WHEN 1 THEN ' DESC' ELSE ' ASC' END + ','
        FROM sys.index_columns ic
        JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
        WHERE ic.object_id = @liveObjId
          AND ic.index_id  = (SELECT index_id FROM sys.indexes WHERE object_id = @liveObjId AND name = @idxName)
          AND ic.key_ordinal > 0
        ORDER BY ic.key_ordinal;

        IF LEN(@keyCols) > 0
            SET @keyCols = LEFT(@keyCols, LEN(@keyCols) - 1);

        SET @inclCols = '';
        SELECT @inclCols = @inclCols + QUOTENAME(c.name) + ','
        FROM sys.index_columns ic
        JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
        WHERE ic.object_id        = @liveObjId
          AND ic.index_id         = (SELECT index_id FROM sys.indexes WHERE object_id = @liveObjId AND name = @idxName)
          AND ic.is_included_column = 1
        ORDER BY ic.index_column_id;

        IF LEN(@inclCols) > 0
            SET @inclCols = LEFT(@inclCols, LEN(@inclCols) - 1);

        SET @sql =
            'CREATE ' +
            CASE WHEN @isUnique    = 1 THEN 'UNIQUE '    ELSE '' END +
            CASE WHEN @isClustered = 1 THEN 'CLUSTERED ' ELSE 'NONCLUSTERED ' END +
            'INDEX ' + QUOTENAME(@idxName) +
            ' ON dbo.comprGOOGLCOPY_Staging (' + @keyCols + ')' +
            CASE WHEN LEN(@inclCols) > 0 THEN ' INCLUDE (' + @inclCols + ')' ELSE '' END;

        EXEC sp_executesql @sql;
        FETCH NEXT FROM idx_cursor INTO @idxName, @isUnique, @isClustered;
    END

    CLOSE idx_cursor;
    DEALLOCATE idx_cursor;
    PRINT 'Phase 1 complete.';

    IF OBJECT_ID('dbo.comprGOOGLCOPY_Old', 'U') IS NOT NULL
        DROP TABLE dbo.comprGOOGLCOPY_Old;

    EXEC sp_rename 'comprGOOGLCOPY',         'comprGOOGLCOPY_Old';
    EXEC sp_rename 'comprGOOGLCOPY_Staging', 'comprGOOGLCOPY';

    DROP TABLE dbo.comprGOOGLCOPY_Old;

    SELECT TOP 0 * INTO dbo.comprGOOGLCOPY_Staging FROM dbo.comprGOOGLCOPY;

    PRINT 'Publish complete.';
END
