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
    /// <param name="containerFormat">The format the input's own header identifies, when the
    /// detected format was found embedded inside it.</param>
    internal ArchiveFormatInfo(InArchiveFormat format, int offset, bool isExecutable,
        InArchiveFormat containerFormat = InArchiveFormat.None)
    {
        Format = format;
        Offset = offset;
        IsExecutable = isExecutable;
        ContainerFormat = containerFormat;
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
    /// Gets the format the input's own header identifies, when <see cref="Format"/> was found
    /// embedded inside it; otherwise <see cref="InArchiveFormat.None"/>. A signature can turn
    /// up inside a container's content by chance, so this is what the reader falls back to
    /// when the embedded candidate will not open.
    /// </summary>
    public InArchiveFormat ContainerFormat { get; }

    /// <summary>
    /// Gets a value indicating whether a recognised archive format was found.
    /// </summary>
    public bool IsArchive => Format != InArchiveFormat.None;
}
