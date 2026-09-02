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
        using var stream = NonArchiveStream(new byte[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 });

        var format = FileChecker.CheckSignature(stream, out var offset, out var isExecutable);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(format, Is.EqualTo(InArchiveFormat.Compound));
            Assert.That(offset, Is.Zero);
            Assert.That(isExecutable, Is.False);
        }
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
}
