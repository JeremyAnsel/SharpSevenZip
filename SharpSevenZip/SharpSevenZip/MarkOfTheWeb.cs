using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace SharpSevenZip;

/// <summary>
/// Reads and writes the NTFS <c>Zone.Identifier</c> alternate data stream that carries a
/// file's Mark-of-the-Web.
/// </summary>
internal static partial class MarkOfTheWeb
{
    private const string ZoneIdentifierStream = ":Zone.Identifier";

    private const uint GenericRead = 0x80000000;
    private const uint GenericWrite = 0x40000000;
    private const uint FileShareRead = 0x00000001;
    private const uint FileShareWrite = 0x00000002;
    private const uint CreateAlways = 2;
    private const uint OpenExisting = 3;
    private const uint FileAttributeNormal = 0x00000080;

    /// <summary>
    /// A zone stream holds a short INI section; the bound is the one 7-Zip applies.
    /// </summary>
    private const long MaxLength = 1 << 15;

    /// <summary>
    /// Reads the Mark-of-the-Web of a file.
    /// </summary>
    /// <param name="fileName">The file to read the zone stream of.</param>
    /// <returns>The raw zone stream, or <c>null</c> if the file carries none.</returns>
    public static byte[]? Read(string? fileName)
    {
        if (fileName is null || fileName.Length == 0 || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return null;
        }

        try
        {
            using var stream = Open(fileName + ZoneIdentifierStream, GenericRead,
                FileShareRead | FileShareWrite, OpenExisting, FileAccess.Read);

            if (stream is null || stream.Length == 0 || stream.Length >= MaxLength)
            {
                return null;
            }

            var zone = new byte[stream.Length];

            for (var offset = 0; offset < zone.Length;)
            {
                var count = stream.Read(zone, offset, zone.Length - offset);

                if (count == 0)
                {
                    return null;
                }

                offset += count;
            }

            return zone;
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
            return null;
        }
    }

    /// <summary>
    /// Writes a Mark-of-the-Web to a file, replacing any zone stream it already has.
    /// </summary>
    /// <param name="zone">The raw zone stream to write. Nothing happens when it is <c>null</c>.</param>
    /// <param name="fileName">The file to write the zone stream to.</param>
    public static void Apply(byte[]? zone, string? fileName)
    {
        if (zone is null || zone.Length == 0
            || fileName is null || fileName.Length == 0
            || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return;
        }

        try
        {
            // A FAT or exFAT target has no alternate data streams, so the zone is dropped.
            using var stream = Open(fileName + ZoneIdentifierStream, GenericWrite,
                0, CreateAlways, FileAccess.Write);

            stream?.Write(zone, 0, zone.Length);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException)
        {
        }
    }

    private static FileStream? Open(string streamName, uint access, uint shareMode, uint creationDisposition, FileAccess fileAccess)
    {
        var handle = CreateFile(streamName, access, shareMode, IntPtr.Zero, creationDisposition, FileAttributeNormal, IntPtr.Zero);

        if (handle.IsInvalid)
        {
            handle.Dispose();
            return null;
        }

        return new FileStream(handle, fileAccess);
    }

#if NET8_0_OR_GREATER
    [LibraryImport("kernel32.dll", EntryPoint = "CreateFileW", StringMarshalling = StringMarshalling.Utf16)]
    private static partial SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);
#else
    [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode)]
    private static extern SafeFileHandle CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        uint dwFlagsAndAttributes,
        IntPtr hTemplateFile);
#endif
}
