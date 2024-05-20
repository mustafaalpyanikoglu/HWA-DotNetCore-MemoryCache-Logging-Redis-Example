using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.Exceptions.Extensions
{
    /// <summary>
    /// Extension class to serialize ProblemDetails to JSON.
    /// </summary>
    internal static class ProblemDetailsExtensions
    {
        /// <summary>
        /// Serializes the specified ProblemDetails object to JSON.
        /// </summary>
        /// <typeparam name="TProblemDetail">The type of the ProblemDetails to be serialized.</typeparam>
        /// <param name="details">The ProblemDetails object to be serialized.</param>
        /// <returns>The JSON representation of the ProblemDetails object.</returns>
        public static string AsJson<TProblemDetail>(this TProblemDetail details)
            where TProblemDetail : ProblemDetails => JsonSerializer.Serialize(details);
    }
}
