using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Logging.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.MSSqlServer;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Logger;

/// <summary>
/// Represents a logger that writes log entries to a Microsoft SQL Server database using Serilog.
/// </summary>
public class MsSqlLogger : LoggerServiceBase
{
    /// <summary>
    /// Initializes a new instance of the MsSqlLogger class with the specified configuration.
    /// </summary>
    /// <param name="configuration">The IConfiguration object used to read Serilog configurations.</param>
    public MsSqlLogger(IConfiguration configuration)
    {
        // Get the MsSqlConfiguration from the appsettings.json configuration section "SeriLogConfigurations:MsSqlConfiguration".
        MsSqlConfiguration logConfiguration =
            configuration.GetSection("SeriLogConfigurations:MsSqlConfiguration").Get<MsSqlConfiguration>()
            ?? throw new Exception(SerilogMessages.NullOptionsMessage);

        // Create sink and column options for the MSSqlServerSink.
        MSSqlServerSinkOptions sinkOptions =
            new() { TableName = logConfiguration.TableName, AutoCreateSqlTable = logConfiguration.AutoCreateSqlTable };
        ColumnOptions columnOptions = new();

        // Configure Serilog to write log entries to the specified Microsoft SQL Server database table.
        global::Serilog.Core.Logger serilogConfig = new LoggerConfiguration().WriteTo
            .MSSqlServer(logConfiguration.ConnectionString, sinkOptions, columnOptions: columnOptions)
            .CreateLogger();

        Logger = serilogConfig;
    }
}