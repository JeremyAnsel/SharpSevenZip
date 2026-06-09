using System.Runtime.Serialization;

namespace SharpSevenZip.Exceptions;

/// <summary>
/// Exception class for failed configuration in SharpSevenZipExtractor or SharpSevenZipCompressor.
/// </summary>
[Serializable]
public class SharpSevenZipConfigurationException : SharpSevenZipException
{
    /// <summary>
    /// Exception default message which is displayed if no extra information is specified
    /// </summary>
    public const string DEFAULT_MESSAGE = "The instance could not be initialized due to invalid configuration. Please double check parameters.";

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipExtractionFailedException class
    /// </summary>
    public SharpSevenZipConfigurationException()
        : base(DEFAULT_MESSAGE) { }

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipExtractionFailedException class
    /// </summary>
    /// <param name="message">Additional detailed message</param>
    public SharpSevenZipConfigurationException(string message)
        : base(DEFAULT_MESSAGE, message) { }

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipExtractionFailedException class
    /// </summary>
    /// <param name="message">Additional detailed message</param>
    /// <param name="inner">Inner exception occurred</param>
    public SharpSevenZipConfigurationException(string message, Exception inner)
        : base(DEFAULT_MESSAGE, message, inner) { }

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipExtractionFailedException class
    /// </summary>
    /// <param name="info">All data needed for serialization or deserialization</param>
    /// <param name="context">Serialized stream descriptor</param>
    protected SharpSevenZipConfigurationException(
        SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
