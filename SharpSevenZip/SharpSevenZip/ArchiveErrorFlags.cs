namespace SharpSevenZip;

/// <summary>
/// Problems 7-Zip reports for an archive it has opened. Mirrors 7-Zip's
/// <c>kpv_ErrorFlags_*</c> values.
/// </summary>
[Flags]
public enum ArchiveErrorFlags
{
    /// <summary>
    /// No problem was reported.
    /// </summary>
    None = 0,

    /// <summary>
    /// The data was not recognized as an archive of this format.
    /// </summary>
    IsNotArchive = 1 << 0,

    /// <summary>
    /// The headers are damaged.
    /// </summary>
    HeadersError = 1 << 1,

    /// <summary>
    /// The encrypted headers could not be decoded.
    /// </summary>
    EncryptedHeadersError = 1 << 2,

    /// <summary>
    /// The archive begins before the start of the available data.
    /// </summary>
    UnavailableStart = 1 << 3,

    /// <summary>
    /// The start of the archive could not be confirmed.
    /// </summary>
    UnconfirmedStart = 1 << 4,

    /// <summary>
    /// The archive is truncated.
    /// </summary>
    UnexpectedEnd = 1 << 5,

    /// <summary>
    /// The archive is followed by data that is not part of it.
    /// </summary>
    DataAfterEnd = 1 << 6,

    /// <summary>
    /// The archive uses a compression method this build cannot decode.
    /// </summary>
    UnsupportedMethod = 1 << 7,

    /// <summary>
    /// The archive uses a format feature this build does not implement.
    /// </summary>
    UnsupportedFeature = 1 << 8,

    /// <summary>
    /// The archive contains damaged data.
    /// </summary>
    DataError = 1 << 9,

    /// <summary>
    /// A checksum stored in the archive does not match.
    /// </summary>
    CrcError = 1 << 10
}
