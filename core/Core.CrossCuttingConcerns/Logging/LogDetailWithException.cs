namespace Core.CrossCuttingConcerns.Logging;

/// <summary>
/// Represents detailed information for a log entry that includes an exception message.
/// It is derived from the base class LogDetail.
/// </summary>
public class LogDetailWithException : LogDetail
{
    /// <summary>
    /// Gets or sets the exception message associated with the log entry, if applicable.
    /// </summary>
    public string ExceptionMessage { get; set; }

    /// <summary>
    /// Initializes a new instance of the LogDetailWithException class with default values.
    /// </summary>
    public LogDetailWithException()
    {
        ExceptionMessage = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the LogDetailWithException class with specified values.
    /// </summary>
    /// <param name="fullName">The full name of the class where the log is generated.</param>
    /// <param name="methodName">The name of the method or function where the log is generated.</param>
    /// <param name="user">The user associated with the log entry.</param>
    /// <param name="parameters">The list of log parameters, if any, associated with the log entry.</param>
    /// <param name="exceptionMessage">The exception message associated with the log entry.</param>
    public LogDetailWithException(string fullName, string methodName, string user, List<LogParameter> parameters, string exceptionMessage)
        : base(fullName, methodName, user, parameters)
    {
        ExceptionMessage = exceptionMessage;
    }
}