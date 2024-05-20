namespace Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;

/// <summary>
/// Represents configuration settings for logging to Microsoft SQL Server (MsSql) in Serilog.
/// </summary>
public class MsSqlConfiguration
{
    /// <summary>
    /// Gets or sets the connection string for the Microsoft SQL Server database.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the name of the table where log entries will be stored in the database.
    /// </summary>
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to automatically create the log table if it does not exist.
    /// </summary>
    public bool AutoCreateSqlTable { get; set; }

    /// <summary>
    /// Initializes a new instance of the MsSqlConfiguration class with default values.
    /// </summary>
    public MsSqlConfiguration()
    {
        ConnectionString = string.Empty;
        TableName = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the MsSqlConfiguration class with specified values.
    /// </summary>
    /// <param name="connectionString">The connection string for the Microsoft SQL Server database.</param>
    /// <param name="tableName">The name of the table where log entries will be stored in the database.</param>
    /// <param name="autoCreateSqlTable">A value indicating whether to automatically create the log table if it does not exist.</param>
    public MsSqlConfiguration(string connectionString, string tableName, bool autoCreateSqlTable)
    {
        ConnectionString = connectionString;
        TableName = tableName;
        AutoCreateSqlTable = autoCreateSqlTable;
    }
}