namespace SharpSevenZip.Tests;

/// <summary>
/// Test data to use for CheckFileSignatureTest.
/// </summary>
public readonly struct FileCheckerTestData
{
    public FileCheckerTestData(string testDataFilePath, InArchiveFormat expectedFormat)
    {
        TestDataFilePath = testDataFilePath;
        ExpectedFormat = expectedFormat;
    }

    /// <summary>
    /// Format this test expects to find.
    /// </summary>
    public InArchiveFormat ExpectedFormat { get; }

    /// <summary>
    /// Path to archive file to test against.
    /// </summary>
    public string TestDataFilePath { get; }

    public override string ToString()
    {
        // Used to get useful test results.
        return ExpectedFormat.ToString();
    }
}

[TestFixture]
public class FileCheckerTests
{
    /// <summary>
    /// Test data for CheckFileSignature test.
    /// </summary>
    private static readonly List<FileCheckerTestData> TestData = new()
    {
            new(@"TestData/arj.arj", InArchiveFormat.Arj),
            new(@"TestData/bzip2.bz2", InArchiveFormat.BZip2),
            new(@"TestData/", InArchiveFormat.Cab),
            new(@"TestData/", InArchiveFormat.Chm),
            new(@"TestData/", InArchiveFormat.Compound),
            new(@"TestData/", InArchiveFormat.Cpio),
            new(@"TestData/", InArchiveFormat.Deb),
            new(@"TestData/", InArchiveFormat.Dmg),
            new(@"TestData/", InArchiveFormat.Elf),
            new(@"TestData/", InArchiveFormat.Flv),
            new(@"TestData/gzip.gz", InArchiveFormat.GZip),
            new(@"TestData/", InArchiveFormat.Hfs),
            new(@"TestData/", InArchiveFormat.Iso),
            new(@"TestData/", InArchiveFormat.Lzh),
            new(@"TestData/", InArchiveFormat.Lzma),
            new(@"TestData/", InArchiveFormat.Lzw),
            new(@"TestData/", InArchiveFormat.Msi),
            new(@"TestData/", InArchiveFormat.Mslz),
            new(@"TestData/", InArchiveFormat.Mub),
            new(@"TestData/", InArchiveFormat.Nsis),
            new(@"TestData/", InArchiveFormat.PE),
            new(@"TestData/rar5.rar", InArchiveFormat.Rar),
            new(@"TestData/rar4.rar", InArchiveFormat.Rar4),
            new(@"TestData/", InArchiveFormat.Rpm),
            new(@"TestData/7z_LZMA2.7z", InArchiveFormat.SevenZip),
            new(@"TestData/", InArchiveFormat.Split),
            new(@"TestData/", InArchiveFormat.Swf),
            new(@"TestData/tar.tar", InArchiveFormat.Tar),
            new(@"TestData/", InArchiveFormat.Udf),
            new(@"TestData/", InArchiveFormat.Vhd),
            new(@"TestData/wim.wim", InArchiveFormat.Wim),
            new(@"TestData/xz.xz", InArchiveFormat.XZ),
            new(@"TestData/", InArchiveFormat.Xar),
            new(@"TestData/zip.zip", InArchiveFormat.Zip),
            new(@"TestData/zstd.zst", InArchiveFormat.Zstd),
            new(@"TestData/", InArchiveFormat.Vhdx),
            new(@"TestData/vdi.vdi", InArchiveFormat.Vdi),
            new(@"TestData/", InArchiveFormat.Vmdk),
            new(@"TestData/qcow.qcow2", InArchiveFormat.QCow),
            new(@"TestData/ihex.ihex", InArchiveFormat.IHex),
            new(@"TestData/", InArchiveFormat.Hxs),
            new(@"TestData/", InArchiveFormat.Lp),
            new(@"TestData/sparse.simg", InArchiveFormat.Sparse),
            new(@"TestData/", InArchiveFormat.Coff),
            new(@"TestData/base64.b64", InArchiveFormat.Base64)
        };

    [SetUp]
    public void SetUp()
    {
        // Ensures we're in the correct working directory (for test data files).
        Directory.SetCurrentDirectory(TestContext.CurrentContext.TestDirectory);
    }

    [TestCaseSource(nameof(TestData))]
    public void CheckFileSignatureTest(FileCheckerTestData data)
    {
        if (!File.Exists(data.TestDataFilePath))
        {
            Assert.Ignore("No test data found for this format.");
        }
        else
        {
            Assert.That(FileChecker.CheckSignature(data.TestDataFilePath, out _, out _), Is.EqualTo(data.ExpectedFormat));
        }
    }

    /// <summary>
    /// An OLE2/Compound file (.doc, .xls, .msi) carries no embedded Zip or 7z signature, so
    /// the SFX scan finds nothing - it is still a container the Compound handler opens.
    /// </summary>
    [Test]
    public void CheckSignature_CompoundWithoutEmbeddedArchive_ReturnsCompound()
    {
        using var stream = NonArchiveStream(CompoundHeader);

        var format = FileChecker.CheckSignature(stream, out var offset, out var isExecutable);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(format, Is.EqualTo(InArchiveFormat.Compound));
            Assert.That(offset, Is.Zero);
            Assert.That(isExecutable, Is.False);
        }
    }

    /// <summary>
    /// The Compound header is only the last resort: a signature found at one of the probed
    /// offsets still decides.
    /// </summary>
    [Test]
    public void CheckSignature_CompoundHeaderOverVdiSignature_PrefersVdi()
    {
        var content = new byte[64 * 1024];
        CompoundHeader.CopyTo(content, 0);
        new byte[] { 0x7F, 0x10, 0xDA, 0xBE }.CopyTo(content, 0x40);
        using var stream = new MemoryStream(content, writable: false);

        Assert.That(FileChecker.CheckSignature(stream, out _, out _), Is.EqualTo(InArchiveFormat.Vdi));
    }

    /// <summary>
    /// An archive embedded in an OLE2 container still wins over the bare Compound fallback.
    /// </summary>
    [Test]
    public void CheckSignature_CompoundHeaderWithEmbeddedZip_PrefersZip()
    {
        var content = new byte[64 * 1024];
        CompoundHeader.CopyTo(content, 0);
        ZipLocalHeader().CopyTo(content, 0x2000);
        using var stream = new MemoryStream(content, writable: false);

        var format = FileChecker.CheckSignature(stream, out var offset, out _);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(format, Is.EqualTo(InArchiveFormat.Zip));
            Assert.That(offset, Is.EqualTo(0x2000));
        }
    }

    /// <summary>
    /// The smallest local file header ZipIn.cpp IsArc_Zip accepts: a named entry whose name
    /// holds no embedded NUL.
    /// </summary>
    private static byte[] ZipLocalHeader()
    {
        var name = System.Text.Encoding.ASCII.GetBytes("Test.txt");
        var header = new byte[30 + name.Length];
        header[0] = 0x50;
        header[1] = 0x4B;
        header[2] = 0x03;
        header[3] = 0x04;
        header[4] = 10;
        header[26] = (byte)name.Length;
        name.CopyTo(header, 30);

        return header;
    }

    [Test]
    public void CheckSignature_ExecutableWithoutEmbeddedArchive_ReturnsPE()
    {
        using var stream = NonArchiveStream(new byte[] { (byte)'M', (byte)'Z' });

        var format = FileChecker.CheckSignature(stream, out _, out var isExecutable);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(format, Is.EqualTo(InArchiveFormat.PE));
            Assert.That(isExecutable, Is.True);
        }
    }

    /// <summary>
    /// ARJ is recognised by two bytes, which arbitrary content reproduces about once per
    /// 64 KiB. Without the header test every larger OLE2 document or executable would be
    /// opened as an ARJ archive at a random offset, and fail.
    /// </summary>
    [TestCase(false)]
    [TestCase(true)]
    public void CheckSignature_ChanceArjBytes_KeepsTheContainerFormat(bool executable)
    {
        var content = new byte[64 * 1024];
        if (executable)
        {
            content[0] = (byte)'M';
            content[1] = (byte)'Z';
        }
        else
        {
            CompoundHeader.CopyTo(content, 0);
        }

        new byte[] { 0x60, 0xEA }.CopyTo(content, 0x3000);
        using var stream = new MemoryStream(content, writable: false);

        var format = FileChecker.CheckSignature(stream, out var offset, out _);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(format, Is.EqualTo(executable ? InArchiveFormat.PE : InArchiveFormat.Compound));
            Assert.That(offset, Is.Zero);
        }
    }

    /// <summary>
    /// A well-formed ARJ header behind a stub is still found.
    /// </summary>
    [Test]
    public void CheckSignature_EmbeddedArjHeader_PrefersArj()
    {
        var content = new byte[64 * 1024];
        content[0] = (byte)'M';
        content[1] = (byte)'Z';
        ArjHeader().CopyTo(content, 0x3000);
        using var stream = new MemoryStream(content, writable: false);

        var format = FileChecker.CheckSignature(stream, out var offset, out _);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(format, Is.EqualTo(InArchiveFormat.Arj));
            Assert.That(offset, Is.EqualTo(0x3000));
        }
    }

    /// <summary>
    /// A signature can turn up inside a container by chance, so the probe reports the format
    /// the header itself identified and the reader falls back to it when the embedded
    /// candidate will not open.
    /// </summary>
    [TestCase(false, InArchiveFormat.Compound)]
    [TestCase(true, InArchiveFormat.PE)]
    public void TryCheckFormat_EmbeddedArchive_ReportsTheContainer(bool executable, InArchiveFormat expected)
    {
        var content = new byte[64 * 1024];
        if (executable)
        {
            content[0] = (byte)'M';
            content[1] = (byte)'Z';
        }
        else
        {
            CompoundHeader.CopyTo(content, 0);
        }

        ZipLocalHeader().CopyTo(content, 0x2000);
        using var stream = new MemoryStream(content, writable: false);

        SharpSevenZipArchiveFormat.TryCheckFormat(stream, out var info);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(info.Format, Is.EqualTo(InArchiveFormat.Zip));
            Assert.That(info.ContainerFormat, Is.EqualTo(expected));
        }
    }

    /// <summary>
    /// Nothing was overridden, so there is nothing to fall back to.
    /// </summary>
    [Test]
    public void TryCheckFormat_FormatFromTheHeader_ReportsNoContainer()
    {
        var content = new byte[64 * 1024];
        CompoundHeader.CopyTo(content, 0);
        using var stream = new MemoryStream(content, writable: false);

        SharpSevenZipArchiveFormat.TryCheckFormat(stream, out var info);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(info.Format, Is.EqualTo(InArchiveFormat.Compound));
            Assert.That(info.ContainerFormat, Is.EqualTo(InArchiveFormat.None));
        }
    }

    /// <summary>
    /// The smallest header ArjHandler.cpp accepts: signature, a 30-byte block, the
    /// archive-header file type and a matching CRC.
    /// </summary>
    private static byte[] ArjHeader()
    {
        const int blockSize = 30;
        var header = new byte[4 + blockSize + 4];
        header[0] = 0x60;
        header[1] = 0xEA;
        header[2] = blockSize;
        header[4] = blockSize;
        header[4 + 6] = 2;

        var crc = new SharpSevenZip.Sdk.Common.Crc();
        crc.Update(header, 4, blockSize);
        BitConverter.GetBytes(crc.GetDigest()).CopyTo(header, 4 + blockSize);

        return header;
    }

    /// <summary>
    /// InArchiveFormat.None is -1 while SevenZip is 0, so a default-valued result would
    /// report SevenZip and make <see cref="ArchiveFormatInfo.IsArchive"/> true.
    /// </summary>
    [Test]
    public void TryCheckSignature_NotAnArchive_ReportsNone()
    {
        using var stream = NonArchiveStream(Array.Empty<byte>());

        var recognised = FileChecker.TryCheckSignature(stream, out var info);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(recognised, Is.False);
            Assert.That(info.Format, Is.EqualTo(InArchiveFormat.None));
            Assert.That(info.IsArchive, Is.False);
        }
    }

    // 64 KiB of zeroes behind the header: past every SpecialDetect offset and free of any
    // signature the SFX scan could latch onto.
    private static MemoryStream NonArchiveStream(byte[] header)
    {
        var content = new byte[64 * 1024];
        header.CopyTo(content, 0);
        return new MemoryStream(content, writable: false);
    }

    private static readonly byte[] CompoundHeader = { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
}
