namespace AFS.Core.Exceptions;

/// <summary>
/// Exception thrown when AI operations fail.
/// Used for Chrome AI (Gemini Nano) and OpenAI service errors.
/// </summary>
public class AIServiceException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AIServiceException"/> class.
    /// </summary>
    public AIServiceException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AIServiceException"/> class with a message.
    /// </summary>
    /// <param name="message">The error message.</param>
    public AIServiceException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AIServiceException"/> class with a message and inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">The inner exception.</param>
    public AIServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
