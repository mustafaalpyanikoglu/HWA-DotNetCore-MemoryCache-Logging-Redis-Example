namespace Core.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels
{
    /// <summary>
    /// Represents configuration settings for File-based logging in Serilog.
    /// </summary>
    public class FileLogConfiguration
    {
        /// <summary>
        /// Gets or sets the folder path where log files will be stored.
        /// </summary>
        public string FolderPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the FileLogConfiguration class with default values.
        /// </summary>
        public FileLogConfiguration()
        {
            FolderPath = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the FileLogConfiguration class with the specified folder path.
        /// </summary>
        /// <param name="folderPath">The folder path where log files will be stored.</param>
        public FileLogConfiguration(string folderPath)
        {
            FolderPath = folderPath;
        }
    }
}