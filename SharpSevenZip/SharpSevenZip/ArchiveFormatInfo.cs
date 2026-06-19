namespace SharpSevenZip;

/// <summary>
/// The result of probing a file or stream for its archive format. Carries everything
/// <see cref="SharpSevenZipExtractor"/> needs to open the archive, so a caller that has
/// already probed the format does not pay for signature detection a second time.
/// </summary>
public readonly record struct ArchiveFormatInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArchiveFormatInfo"/> struct.
    /// </summary>
    /// <param name="format">The detected archive format.</param>
    /// <param name="offset">The byte offset at which the archive begins.</param>
    /// <param name="isExecutable">True when the input is a PE executable.</param>
    internal ArchiveFormatInfo(InArchiveFormat format, int offset, bool isExecutable)
    {
        Format = format;
        Offset = offset;
        IsExecutable = isExecutable;
    }

    /// <summary>
    /// Gets the detected archive format, or <see cref="InArchiveFormat.None"/> when the
    /// input is not a recognised archive.
    /// </summary>
    public InArchiveFormat Format { get; }

    /// <summary>
    /// Gets the byte offset at which the archive content begins. Non-zero for
    /// self-extracting or otherwise embedded archives.
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Gets a value indicating whether the input is a PE executable (which may, but need
    /// not, be a self-extracting archive).
    /// </summary>
    public bool IsExecutable { get; }

    /// <summary>
    /// Gets a value indicating whether a recognised archive format was found.
    /// </summary>
    public bool IsArchive => Format != InArchiveFormat.None;
}
