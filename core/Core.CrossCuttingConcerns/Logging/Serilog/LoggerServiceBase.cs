﻿using Serilog;

namespace Core.CrossCuttingConcerns.Logging.Serilog;

/// <summary>
/// Represents a base abstract class for implementing logging services using Serilog.
/// </summary>
public abstract class LoggerServiceBase
{
    /// <summary>
    /// Gets or sets the Serilog ILogger instance used for logging.
    /// </summary>
    protected ILogger Logger { get; set; }

    /// <summary>
    /// Initializes a new instance of the LoggerServiceBase class with a null logger.
    /// </summary>
    protected LoggerServiceBase()
    {
        Logger = null!;
    }

    /// <summary>
    /// Initializes a new instance of the LoggerServiceBase class with a specified Serilog ILogger instance.
    /// </summary>
    /// <param name="logger">The Serilog ILogger instance to use for logging.</param>
    protected LoggerServiceBase(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Writes a verbose log message.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void Verbose(string message) => Logger.Verbose(message);

    /// <summary>
    /// Writes a fatal log message.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void Fatal(string message) => Logger.Fatal(message);

    /// <summary>
    /// Writes an information log message.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void Info(string message) => Logger.Information(message);

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void Warn(string message) => Logger.Warning(message);

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void Debug(string message) => Logger.Debug(message);

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The message to be logged.</param>
    public void Error(string message) => Logger.Error(message);
}
