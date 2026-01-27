using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuildingBlocks.Infrastructure.Migrations;

/// <summary>
/// Helper utilities for database migrations.
/// </summary>
public static class MigrationHelpers
{
    /// <summary>
    /// Drops an index if it exists.
    /// </summary>
    /// <param name="migrationBuilder">The migration builder.</param>
    /// <param name="indexName">The index name.</param>
    /// <param name="tableName">The table name.</param>
    /// <returns>The migration builder.</returns>
    public static MigrationBuilder DropIndexIfExists(
        this MigrationBuilder migrationBuilder,
        string indexName,
        string tableName)
    {
        migrationBuilder.Sql($@"
            IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('{tableName}') AND name = '{indexName}')
            BEGIN
                DROP INDEX {indexName} ON {tableName}
            END
        ");
        return migrationBuilder;
    }

    /// <summary>
    /// Drops a foreign key if it exists.
    /// </summary>
    /// <param name="migrationBuilder">The migration builder.</param>
    /// <param name="foreignKeyName">The foreign key name.</param>
    /// <param name="tableName">The table name.</param>
    /// <returns>The migration builder.</returns>
    public static MigrationBuilder DropForeignKeyIfExists(
        this MigrationBuilder migrationBuilder,
        string foreignKeyName,
        string tableName)
    {
        migrationBuilder.Sql($@"
            IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('{tableName}') AND name = '{foreignKeyName}')
            BEGIN
                ALTER TABLE {tableName} DROP CONSTRAINT {foreignKeyName}
            END
        ");
        return migrationBuilder;
    }

    /// <summary>
    /// Renames a column if it exists and the new name doesn't already exist.
    /// </summary>
    /// <param name="migrationBuilder">The migration builder.</param>
    /// <param name="tableName">The table name.</param>
    /// <param name="oldColumnName">The old column name.</param>
    /// <param name="newColumnName">The new column name.</param>
    /// <returns>The migration builder.</returns>
    public static MigrationBuilder RenameColumnIfExists(
        this MigrationBuilder migrationBuilder,
        string tableName,
        string oldColumnName,
        string newColumnName)
    {
        migrationBuilder.Sql($@"
            IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('{tableName}') AND name = '{oldColumnName}')
            AND NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('{tableName}') AND name = '{newColumnName}')
            BEGIN
                EXEC sp_rename '{tableName}.{oldColumnName}', '{newColumnName}', 'COLUMN'
            END
        ");
        return migrationBuilder;
    }

    /// <summary>
    /// Renames a table if it exists and the new name doesn't already exist.
    /// </summary>
    /// <param name="migrationBuilder">The migration builder.</param>
    /// <param name="oldTableName">The old table name.</param>
    /// <param name="newTableName">The new table name.</param>
    /// <returns>The migration builder.</returns>
    public static MigrationBuilder RenameTableIfExists(
        this MigrationBuilder migrationBuilder,
        string oldTableName,
        string newTableName)
    {
        migrationBuilder.Sql($@"
            IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('{oldTableName}') AND type = 'U')
            AND NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('{newTableName}') AND type = 'U')
            BEGIN
                EXEC sp_rename '{oldTableName}', '{newTableName}'
            END
        ");
        return migrationBuilder;
    }
}