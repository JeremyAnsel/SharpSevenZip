﻿using System.Runtime.Serialization;

namespace SharpSevenZip.Exceptions;

/// <summary>
/// Exception class for fail to create an archive in SharpSevenZipCompressor.
/// </summary>
[Serializable]
public class SharpSevenZipCompressionFailedException : SharpSevenZipException
{
    /// <summary>
    /// Exception default message which is displayed if no extra information is specified
    /// </summary>
    public const string DEFAULT_MESSAGE = "The compression has failed for an unknown reason with code ";

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipCompressionFailedException class
    /// </summary>
    public SharpSevenZipCompressionFailedException()
        : base(DEFAULT_MESSAGE) { }

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipCompressionFailedException class
    /// </summary>
    /// <param name="message">Additional detailed message</param>
    public SharpSevenZipCompressionFailedException(string message)
        : base(DEFAULT_MESSAGE, message) { }

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipCompressionFailedException class
    /// </summary>
    /// <param name="message">Additional detailed message</param>
    /// <param name="inner">Inner exception occurred</param>
    public SharpSevenZipCompressionFailedException(string message, Exception inner)
        : base(DEFAULT_MESSAGE, message, inner) { }

    /// <summary>
    /// Initializes a new instance of the SharpSevenZipCompressionFailedException class
    /// </summary>
    /// <param name="info">All data needed for serialization or deserialization</param>
    /// <param name="context">Serialized stream descriptor</param>
    protected SharpSevenZipCompressionFailedException(
        SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}
