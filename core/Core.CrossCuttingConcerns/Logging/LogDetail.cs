namespace Core.CrossCuttingConcerns.Logging;

/// <summary>
/// Represents detailed information for a log entry.
/// </summary>
public class LogDetail
{
    /// <summary>
    /// Gets or sets the full name of the class where the log is generated.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets the name of the method or function where the log is generated.
    /// </summary>
    public string MethodName { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the log entry.
    /// </summary>
    public string User { get; set; }

    /// <summary>
    /// Gets or sets the list of log parameters, if any, associated with the log entry.
    /// </summary>
    public List<LogParameter> Parameters { get; set; }

    public LogDetail()
    {
        FullName = string.Empty;
        MethodName = string.Empty;
        User = string.Empty;
        Parameters = new List<LogParameter>();
    }

    public LogDetail(string fullName, string methodName, string user, List<LogParameter> parameters)
    {
        FullName = fullName;
        MethodName = methodName;
        User = user;
        Parameters = parameters;
    }
}