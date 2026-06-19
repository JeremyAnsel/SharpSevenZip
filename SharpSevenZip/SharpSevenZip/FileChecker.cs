namespace SharpSevenZip;

/// <summary>
/// The signature checker class. Original code by Siddharth Uppal, adapted by Markhor.
/// </summary>
/// <remarks>Based on the code at http://blog.somecreativity.com/2008/04/08/how-to-check-if-a-file-is-compressed-in-c/#</remarks>
internal static class FileChecker
{
    private const int SIGNATURE_SIZE = 21;
    private const int SFX_SCAN_LENGTH = 256 * 1024;

    /// <summary>
    /// Reads exactly <paramref name="count"/> bytes from <paramref name="stream"/> into
    /// <paramref name="buffer"/> starting at <paramref name="offset"/>.
    /// </summary>
    private static void ReadFully(Stream stream, byte[] buffer, int offset, int count)
    {
#if NET8_0_OR_GREATER
        stream.ReadExactly(buffer, offset, count);
#else
        while (count > 0)
        {
            var read = stream.Read(buffer, offset, count);
            if (read == 0)
                throw new EndOfStreamException();
            offset += read;
            count -= read;
        }
#endif
    }

    private static bool SpecialDetect(Stream stream, int offset, InArchiveFormat expectedFormat)
    {
        if (stream.Length <= offset + SIGNATURE_SIZE)
            return false;

        var signature = new byte[SIGNATURE_SIZE];
        stream.Seek(offset, SeekOrigin.Begin);
        ReadFully(stream, signature, 0, SIGNATURE_SIZE);

        var actualSignature = BitConverter.ToString(signature);

        foreach (var expectedSignature in Formats.InSignatureFormats)
        {
            if (expectedSignature.Value != expectedFormat)
                continue;

            if (actualSignature.AsSpan().StartsWith(expectedSignature.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Validates the 512-byte tar header at the start of the stream by recomputing its
    /// checksum. This recognises every tar variant (v7, ustar, GNU, pax) without relying
    /// on the "ustar" magic and, crucially, rejects zero-padded non-tar binaries that merely
    /// end in zero bytes.
    /// </summary>
    private static bool IsValidTarHeader(Stream stream)
    {
        const int BlockSize = 512;
        const int ChecksumOffset = 148;
        const int ChecksumLength = 8;

        if (stream.Length < BlockSize)
        {
            return false;
        }

        var header = new byte[BlockSize];
        stream.Seek(0, SeekOrigin.Begin);
        ReadFully(stream, header, 0, BlockSize);

        var storedChecksum = ParseOctal(header, ChecksumOffset, ChecksumLength);
        if (storedChecksum < 0)
        {
            return false;
        }

        // The stored checksum is computed with the checksum field itself read as spaces.
        // Compare against both the unsigned and signed byte sums, since historic writers
        // disagreed on the signedness of bytes >= 0x80.
        long unsignedSum = 0;
        long signedSum = 0;
        for (var i = 0; i < BlockSize; i++)
        {
            var b = i is >= ChecksumOffset and < ChecksumOffset + ChecksumLength ? (byte)0x20 : header[i];
            unsignedSum += b;
            signedSum += (sbyte)b;
        }

        return storedChecksum == unsignedSum || storedChecksum == signedSum;
    }

    /// <summary>
    /// Parses a NUL/space padded octal ASCII number from a header field.
    /// Returns -1 when the field holds no valid octal digits.
    /// </summary>
    private static long ParseOctal(byte[] buffer, int offset, int length)
    {
        var i = offset;
        var end = offset + length;

        while (i < end && (buffer[i] == (byte)' ' || buffer[i] == 0))
        {
            i++;
        }

        long value = 0;
        var any = false;
        for (; i < end; i++)
        {
            var b = buffer[i];
            if (b == (byte)' ' || b == 0)
            {
                break;
            }

            if (b is < (byte)'0' or > (byte)'7')
            {
                return -1;
            }

            value = (value << 3) + (b - '0');
            any = true;
        }

        return any ? value : -1;
    }

    /// <summary>
    /// Gets the InArchiveFormat for a specific extension.
    /// </summary>
    /// <param name="stream">The stream to identify.</param>
    /// <param name="offset">The archive beginning offset.</param>
    /// <param name="isExecutable">True if the original format of the stream is PE; otherwise, false.</param>
    /// <returns>Corresponding InArchiveFormat.</returns>
    public static InArchiveFormat CheckSignature(Stream stream, out int offset, out bool isExecutable)
    {
        offset = 0;
        isExecutable = false;

        if (!stream.CanRead || !stream.CanSeek)
        {
            throw new ArgumentException("The stream must be readable and seekable.");
        }

        if (stream.Length < SIGNATURE_SIZE)
        {
            throw new ArgumentException("The stream is invalid.");
        }

        var signature = new byte[SIGNATURE_SIZE];
        stream.Seek(0, SeekOrigin.Begin);
        ReadFully(stream, signature, 0, SIGNATURE_SIZE);

        var actualSignature = BitConverter.ToString(signature);

        InArchiveFormat? suspectedFormat = null;
        isExecutable = false;

        foreach (var expectedSignature in Formats.InSignatureFormats)
        {
            InArchiveFormat expectedFormat = expectedSignature.Value;

            if (actualSignature.AsSpan().StartsWith(expectedSignature.Key.AsSpan(), StringComparison.OrdinalIgnoreCase) ||
                (expectedFormat == InArchiveFormat.Lzh && actualSignature.AsSpan()[6..].StartsWith(expectedSignature.Key.AsSpan(), StringComparison.OrdinalIgnoreCase))
                )
            {
                if (expectedFormat == InArchiveFormat.PE)
                {
                    suspectedFormat = InArchiveFormat.PE;
                    isExecutable = true;
                }
                else
                {
                    return expectedFormat;
                }
            }
        }

        // Many Microsoft formats
        if (actualSignature.StartsWith("D0-CF-11-E0-A1-B1-1A-E1", StringComparison.OrdinalIgnoreCase))
        {
            suspectedFormat = InArchiveFormat.Compound;
        }

        #region SpecialDetect

        // PE/SFX and OLE2/Compound files must not be short-circuited as TAR even if "ustar"
        // bytes appear at offset 257 inside their binary content. Only check for TAR when
        // no other format has been suspected yet.
        if (suspectedFormat == null && SpecialDetect(stream, 257, InArchiveFormat.Tar))
        {
            return InArchiveFormat.Tar;
        }

        // UDF BEA01 at 0x8000; check before ISO so pure-UDF (no CD001 bridge) is detected correctly
        if (SpecialDetect(stream, 0x8000, InArchiveFormat.Udf))
        {
            return InArchiveFormat.Udf;
        }

        if (SpecialDetect(stream, 0x8001, InArchiveFormat.Iso) ||
            SpecialDetect(stream, 0x8801, InArchiveFormat.Iso) ||
            SpecialDetect(stream, 0x9001, InArchiveFormat.Iso))
        {
            return InArchiveFormat.Iso;
        }

        if (SpecialDetect(stream, 0x200, InArchiveFormat.Gpt))
        {
            return InArchiveFormat.Gpt;
        }

        if (SpecialDetect(stream, 0x400, InArchiveFormat.Hfs))
        {
            return InArchiveFormat.Hfs;
        }

        // Android LP (Logical Partitions): magic is at LP_PARTITION_RESERVED_BYTES = 0x1000
        // cf. https://github.com/ip7z/7zip/blob/main/CPP/7zip/Archive/LpHandler.cpp#L58
        if (SpecialDetect(stream, 0x1000, InArchiveFormat.Lp))
        {
            return InArchiveFormat.Lp;
        }

        // VDI: magic (k_Signature) is at offset 0x40
        // cf. https://github.com/ip7z/7zip/blob/main/CPP/7zip/Archive/VdiHandler.cpp#L287
        if (SpecialDetect(stream, 0x40, InArchiveFormat.Vdi))
        {
            return InArchiveFormat.Vdi;
        }

        #region Last resort for tar

        // Tars whose first header lacks the POSIX "ustar" magic (old v7 / pre-POSIX GNU
        // tars) are not caught by the SpecialDetect("ustar") check above. Validate the
        // 512-byte header checksum instead of guessing from trailing zero padding: the
        // former heuristic ("the last 1024 bytes are all zero") also matches zero-padded
        // binaries – SquashFS images, Linux bzImage kernels, EFI stubs and firmware blobs –
        // which were misdetected as TAR and then failed on open with the misleading
        // "decided it is TAR by mistake" error. A valid checksum is present in every real
        // tar header (all variants) and absent from those binaries.
        if (suspectedFormat == null && IsValidTarHeader(stream))
        {
            return InArchiveFormat.Tar;
        }

        #endregion

        #endregion

        #region Check if it is an SFX archive or a file with an embedded archive.

        if (suspectedFormat != null)
        {

            var scanLength = Math.Min(stream.Length, SFX_SCAN_LENGTH);
            signature = new byte[scanLength];
            stream.Seek(0, SeekOrigin.Begin);
            ReadFully(stream, signature, 0, (int)scanLength);

            actualSignature = BitConverter.ToString(signature);

            foreach (var format in new[]
            {
                    InArchiveFormat.Zip,
                    InArchiveFormat.SevenZip,
                    InArchiveFormat.Rar4,
                    InArchiveFormat.Rar,
                    InArchiveFormat.Cab,
                    InArchiveFormat.Arj
                })
            {
                var pos = actualSignature.IndexOf(Formats.InSignatureFormatsReversed[format], StringComparison.InvariantCulture);

                if (pos > -1)
                {
                    offset = pos / 3;
                    return format;
                }
            }

            // Nothing
            if (suspectedFormat == InArchiveFormat.PE)
            {
                return InArchiveFormat.PE;
            }
        }

        #endregion

        throw new ArgumentException("The stream is invalid or no corresponding signature was found.");
    }

    /// <summary>
    /// Gets the InArchiveFormat for a specific file name.
    /// </summary>
    /// <param name="fileName">The archive file name.</param>
    /// <param name="offset">The archive beginning offset.</param>
    /// <param name="isExecutable">True if the original format of the file is PE; otherwise, false.</param>
    /// <returns>Corresponding InArchiveFormat.</returns>
    /// <exception cref="System.ArgumentException"/>
    public static InArchiveFormat CheckSignature(string fileName, out int offset, out bool isExecutable)
    {
        using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, SIGNATURE_SIZE);

        try
        {
            return CheckSignature(fs, out offset, out isExecutable);
        }
        catch (ArgumentException)
        {
            offset = 0;
            isExecutable = false;
            return Formats.FormatByFileName(fileName, true);
        }
    }

    /// <summary>
    /// Probes a file for its archive format without throwing when it is not an archive.
    /// </summary>
    /// <param name="fileName">The file name to identify.</param>
    /// <param name="info">The full detection result; <see cref="ArchiveFormatInfo.Format"/>
    /// is <see cref="InArchiveFormat.None"/> when nothing was recognised.</param>
    /// <returns>True when a recognised archive format was found; otherwise, false.</returns>
    public static bool TryCheckSignature(string fileName, out ArchiveFormatInfo info)
    {
        try
        {
            var format = CheckSignature(fileName, out var offset, out var isExecutable);
            info = new ArchiveFormatInfo(format, offset, isExecutable);
            return true;
        }
        catch (ArgumentException)
        {
            info = default;
            return false;
        }
    }

    /// <summary>
    /// Probes a stream for its archive format without throwing when it is not an archive.
    /// </summary>
    /// <param name="stream">The stream to identify.</param>
    /// <param name="info">The full detection result; <see cref="ArchiveFormatInfo.Format"/>
    /// is <see cref="InArchiveFormat.None"/> when nothing was recognised.</param>
    /// <returns>True when a recognised archive format was found; otherwise, false.</returns>
    public static bool TryCheckSignature(Stream stream, out ArchiveFormatInfo info)
    {
        try
        {
            var format = CheckSignature(stream, out var offset, out var isExecutable);
            info = new ArchiveFormatInfo(format, offset, isExecutable);
            return true;
        }
        catch (ArgumentException)
        {
            info = default;
            return false;
        }
    }
}
