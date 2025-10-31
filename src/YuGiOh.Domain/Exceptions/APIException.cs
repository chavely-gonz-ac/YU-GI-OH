using System.Net;

namespace YuGiOh.Domain.Exceptions
{
    /// <summary>
    /// Represents an error that occurs during the execution of an API request.
    /// Provides standardized structure for exception handling and responses.
    /// </summary>
    public class APIException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with this exception.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets a short, human-readable title describing the type of error.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets an optional additional detail message that may assist in debugging.
        /// </summary>
        public string? Detail { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code to return (default: 500).</param>
        /// <param name="message">The main message describing the error.</param>
        /// <param name="title">A short title for the error (default: "API Error").</param>
        /// <param name="detail">Optional additional detail message.</param>
        public APIException(
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError,
            string message = "An unexpected error occurred.",
            string title = "API Error",
            string? detail = null
        ) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
            Detail = detail;
        }

        /// <summary>
        /// Converts the exception into a standardized object that can be serialized as JSON.
        /// </summary>
        public object ToProblemDetails()
        {
            return new
            {
                status = (int)StatusCode,
                title = Title,
                message = Message,
                detail = Detail,
                timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Creates a Bad Request exception (400).
        /// </summary>
        public static APIException BadRequest(string message, string? detail = null)
            => new APIException(HttpStatusCode.BadRequest, message, "Bad Request", detail);

        /// <summary>
        /// Creates a Not Found exception (404).
        /// </summary>
        public static APIException NotFound(string message, string? detail = null)
            => new APIException(HttpStatusCode.NotFound, message, "Not Found", detail);

        /// <summary>
        /// Creates an Unauthorized exception (401).
        /// </summary>
        public static APIException Unauthorized(string message = "Unauthorized access.", string? detail = null)
            => new APIException(HttpStatusCode.Unauthorized, message, "Unauthorized", detail);

        /// <summary>
        /// Creates a Forbidden exception (403).
        /// </summary>
        public static APIException Forbidden(string message = "Access forbidden.", string? detail = null)
            => new APIException(HttpStatusCode.Forbidden, message, "Forbidden", detail);

        /// <summary>
        /// Creates an Internal Server Error exception (500).
        /// </summary>
        public static APIException Internal(string message = "An internal server error occurred.", string? detail = null)
            => new APIException(HttpStatusCode.InternalServerError, message, "Internal Server Error", detail);
    }
}
