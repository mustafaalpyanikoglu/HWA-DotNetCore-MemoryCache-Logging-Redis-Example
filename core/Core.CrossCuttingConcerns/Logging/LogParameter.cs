/// <summary>
/// Represents a parameter logged along with its name, value, and type information.
/// </summary>
public class LogParameter
{
    /// <summary>
    /// Gets or sets the name of the parameter.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the parameter.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets the type information of the parameter.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the LogParameter class with default values.
    /// </summary>
    public LogParameter()
    {
        Name = string.Empty;
        Value = string.Empty;
        Type = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the LogParameter class with specified values.
    /// </summary>
    /// <param name="name">The name of the parameter.</param>
    /// <param name="value">The value of the parameter.</param>
    /// <param name="type">The type information of the parameter.</param>
    public LogParameter(string name, object value, string type)
    {
        Name = name;
        Value = value;
        Type = type;
    }
}