using SharpSevenZip.Exceptions;

namespace SharpSevenZip.Tests;

/// <remarks>
/// See https://sevenzip.osdn.jp/chm/cmdline/switches/method.htm for parameter details.
/// </remarks>
[TestFixture]
public class SharpSevenZipCompressorCustomParameterTests : TestBase
{
    [Test]
    public void CompressWithCustomParameters_OnlyWorksWithCorrectMethod()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.Lzma
        };

        // Check parameters for PPMd compression.
        compressor.CustomParameters.Add("mem", "25");
        Assert.Throws<CompressionFailedException>(() => compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip"));
        compressor.CustomParameters.Remove("mem");
        compressor.CustomParameters.Add("o", "10");
        Assert.Throws<CompressionFailedException>(() => compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip"));
        compressor.CustomParameters.Remove("o");
    }

    [Test]
    public void InvalidCustomParameters_Throws()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip
        };

        compressor.CustomParameters.Add("x", "3");
        Assert.Throws<CompressionFailedException>(() => compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip"));
        compressor.CustomParameters.Remove("x");

        compressor.CustomParameters.Add("em", "AES128");
        Assert.Throws<CompressionFailedException>(() => compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip"));
    }

    [Test]
    public void Zip_Deflate_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.Deflate
        };

        compressor.CustomParameters.Add("fb", "4");
        compressor.CustomParameters.Add("pass", "4");
        compressor.CustomParameters.Add("mt", "off");
        compressor.CustomParameters.Add("tc", "off");
        compressor.CustomParameters.Add("cl", "on");
        compressor.CustomParameters.Add("cu", "on");
        compressor.CustomParameters.Add("cp", "866");

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }

    [Test]
    public void Zip_Deflate64_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.Deflate64
        };

        compressor.CustomParameters.Add("fb", "4");
        compressor.CustomParameters.Add("pass", "4");
        compressor.CustomParameters.Add("mt", "off");
        compressor.CustomParameters.Add("tc", "off");
        compressor.CustomParameters.Add("cl", "on");
        compressor.CustomParameters.Add("cu", "on");
        compressor.CustomParameters.Add("cp", "866");

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }

    [Test]
    public void Zip_PPMd_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.Ppmd
        };

        compressor.CustomParameters.Add("mem", "128m");
        compressor.CustomParameters.Add("o", "9");

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }

    [Test]
    public void Zip_BZip2_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.BZip2
        };

        compressor.CustomParameters.Add("d", "1048576b");
        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");

        compressor.CustomParameters.Remove("d");
        compressor.CustomParameters.Add("d", "16");
        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }

    [Test]
    public void SevenZip_Default_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            CompressionMethod = CompressionMethod.Default
        };

        compressor.CustomParameters.Add("yx", "7");
        compressor.CustomParameters.Add("s", "off");
        compressor.CustomParameters.Add("qs", "on");
        compressor.CustomParameters.Add("f", "off");
        compressor.CustomParameters.Add("hc", "off");
        compressor.CustomParameters.Add("he", "on");
        compressor.CustomParameters.Add("mt", "off");
        compressor.CustomParameters.Add("mtf", "off");
        compressor.CustomParameters.Add("tm", "off");
        compressor.CustomParameters.Add("tc", "on");
        compressor.CustomParameters.Add("ta", "on");
        compressor.CustomParameters.Add("tr", "off");

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }

    [Test]
    public void SevenZip_Lzma_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            CompressionMethod = CompressionMethod.Lzma
        };

        compressor.CustomParameters.Add("a", "0");
        compressor.CustomParameters.Add("d", "25");
        compressor.CustomParameters.Add("mf", "bt3");
        compressor.CustomParameters.Add("fb", "24");
        compressor.CustomParameters.Add("mc", "24");
        compressor.CustomParameters.Add("lc", "4");
        compressor.CustomParameters.Add("lp", "1");
        compressor.CustomParameters.Add("pb", "3");

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }

    [Test]
    public void SevenZip_Lzma2_WithCustomParameters()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            CompressionMethod = CompressionMethod.Lzma2
        };

        compressor.CustomParameters.Add("c", "512m");

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");
    }
}
