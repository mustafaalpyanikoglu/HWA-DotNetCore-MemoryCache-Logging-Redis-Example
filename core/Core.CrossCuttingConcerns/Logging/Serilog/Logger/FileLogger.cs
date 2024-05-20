﻿using Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Core.CrossCuttingConcerns.Logging.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Logger
{
    /// <summary>
    /// Represents a logger that writes log entries to a file using Serilog.
    /// </summary>
    public class FileLogger : LoggerServiceBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the FileLogger class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The IConfiguration object used to read Serilog configurations.</param>
        public FileLogger(IConfiguration configuration)
        {
            _configuration = configuration;

            FileLogConfiguration logConfig =
                configuration.GetSection("SeriLogConfigurations:FileLogConfiguration").Get<FileLogConfiguration>()
                ?? throw new Exception(SerilogMessages.NullOptionsMessage);

            string logFilePath = string.Format(format: "{0}{1}", arg0: Directory.GetCurrentDirectory() + logConfig.FolderPath, arg1: ".txt");

            Logger = new LoggerConfiguration().WriteTo
                .File(
                    logFilePath,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: null,
                    fileSizeLimitBytes: 5000000,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
                )
                .CreateLogger();
        }
    }
}