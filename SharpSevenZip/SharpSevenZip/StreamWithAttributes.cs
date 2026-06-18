namespace SharpSevenZip;

/// <summary>
/// Associates a <see cref="System.IO.Stream"/> with optional file-time attributes for use in archive compression.
/// </summary>
/// <param name="Stream">The stream containing the file data to compress.</param>
/// <param name="CreationTime">The creation time to store in the archive, or <c>null</c> to omit.</param>
/// <param name="LastWriteTime">The last-write time to store in the archive, or <c>null</c> to omit.</param>
/// <param name="LastAccessTime">The last-access time to store in the archive, or <c>null</c> to omit.</param>
public record StreamWithAttributes(Stream Stream, DateTime? CreationTime = null, DateTime? LastWriteTime = null, DateTime? LastAccessTime = null);
