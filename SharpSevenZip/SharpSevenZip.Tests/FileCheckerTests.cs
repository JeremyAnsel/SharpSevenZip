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
            new(@"TestData\arj.arj", InArchiveFormat.Arj),
            new(@"TestData\bzip2.bz2", InArchiveFormat.BZip2),
            new(@"TestData\", InArchiveFormat.Cab),
            new(@"TestData\", InArchiveFormat.Chm),
            new(@"TestData\", InArchiveFormat.Compound),
            new(@"TestData\", InArchiveFormat.Cpio),
            new(@"TestData\", InArchiveFormat.Deb),
            new(@"TestData\", InArchiveFormat.Dmg),
            new(@"TestData\", InArchiveFormat.Elf),
            new(@"TestData\", InArchiveFormat.Flv),
            new(@"TestData\gzip.gz", InArchiveFormat.GZip),
            new(@"TestData\", InArchiveFormat.Hfs),
            new(@"TestData\", InArchiveFormat.Iso),
            new(@"TestData\", InArchiveFormat.Lzh),
            new(@"TestData\", InArchiveFormat.Lzma),
            new(@"TestData\", InArchiveFormat.Lzw),
            new(@"TestData\", InArchiveFormat.Msi),
            new(@"TestData\", InArchiveFormat.Mslz),
            new(@"TestData\", InArchiveFormat.Mub),
            new(@"TestData\", InArchiveFormat.Nsis),
            new(@"TestData\", InArchiveFormat.PE),
            new(@"TestData\rar5.rar", InArchiveFormat.Rar),
            new(@"TestData\rar4.rar", InArchiveFormat.Rar4),
            new(@"TestData\", InArchiveFormat.Rpm),
            new(@"TestData\7z_LZMA2.7z", InArchiveFormat.SevenZip),
            new(@"TestData\", InArchiveFormat.Split),
            new(@"TestData\", InArchiveFormat.Swf),
            new(@"TestData\tar.tar", InArchiveFormat.Tar),
            new(@"TestData\", InArchiveFormat.Udf),
            new(@"TestData\", InArchiveFormat.Vhd),
            new(@"TestData\wim.wim", InArchiveFormat.Wim),
            new(@"TestData\xz.xz", InArchiveFormat.XZ),
            new(@"TestData\", InArchiveFormat.Xar),
            new(@"TestData\zip.zip", InArchiveFormat.Zip)
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
}
