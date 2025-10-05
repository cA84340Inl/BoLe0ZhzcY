// 代码生成时间: 2025-10-06 02:13:25
// <summary>
// Represents a service for managing database version control.
// </summary>
using System;
using System.Data;
using System.Data.Common;
using System.IO;

namespace DatabaseVersionControlApp
{
    public class DatabaseVersionControlService
    {
        private readonly string _connectionString;
        private readonly string _migrationFolder;

        public DatabaseVersionControlService(string connectionString, string migrationFolder)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _migrationFolder = migrationFolder ?? throw new ArgumentNullException(nameof(migrationFolder));
        }

        // <summary>
        // Applies all pending migrations to the database.
        // </summary>
        public void ApplyMigrations()
        {
            if (!Directory.Exists(_migrationFolder))
            {
                throw new DirectoryNotFoundException($"Migration folder not found: {_migrationFolder}");
            }

            var migrations = Directory.GetFiles(_migrationFolder, "*.sql").OrderBy(f => f).ToArray();
            foreach (var migration in migrations)
            {
                ApplyMigration(migration);
            }
        }

        // <summary>
        // Applies a single migration to the database.
        // </summary>
        private void ApplyMigration(string migrationFilePath)
        {
            using (var connection = CreateDbConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();
                var sql = File.ReadAllText(migrationFilePath);
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        // <summary>
        // Creates a database connection based on the provided connection string.
        // </summary>
        private DbConnection CreateDbConnection()
        {
            var factory = DbProviderFactories.GetFactory();
            return factory.CreateConnection();
        }
    }
}
