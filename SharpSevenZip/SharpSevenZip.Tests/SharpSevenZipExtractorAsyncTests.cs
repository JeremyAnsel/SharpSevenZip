using System;

namespace SharpSevenZip.Tests;

[TestFixture]
public class SharpSevenZipExtractorAsyncTests
{
    [Test]
    public async Task ExtractFileAsyncLeaveOpen_True_NotThrow()
    {
        using var archiveStream = new MemoryStream();

        var compressor = new SharpSevenZipCompressor();

        var buffer = new byte[1024];
        using (var ms = new MemoryStream())
        {
            var random = new Random(0);
            random.NextBytes(buffer);
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            compressor.DefaultItemName = "file.bin";
            compressor.CompressStream(ms, archiveStream);
        }

        archiveStream.Position = 0;
        using var fileStream = new MemoryStream();
        using (var extractor = new SharpSevenZipExtractor(archiveStream, true))
        {
            await extractor.ExtractFileAsync("file.bin", fileStream);
        }

        fileStream.Position = 0;
        var buffer2 = new byte[1024];
        fileStream.Read(buffer2, 0, buffer2.Length);

        Assert.That(buffer2, Is.EquivalentTo(buffer));
    }

    [Test]
    public async Task ExtractFileAsyncLeaveOpen_False_NotThrow()
    {
        using var archiveStream = new MemoryStream();

        var compressor = new SharpSevenZipCompressor();

        var buffer = new byte[1024];
        using (var ms = new MemoryStream())
        {
            var random = new Random(0);
            random.NextBytes(buffer);
            ms.Write(buffer, 0, buffer.Length);
            ms.Position = 0;
            compressor.DefaultItemName = "file.bin";
            compressor.CompressStream(ms, archiveStream);
        }

        archiveStream.Position = 0;
        using var fileStream = new MemoryStream();
        using (var extractor = new SharpSevenZipExtractor(archiveStream, false))
        {
            await extractor.ExtractFileAsync("file.bin", fileStream);
        }

        fileStream.Position = 0;
        var buffer2 = new byte[1024];
        fileStream.Read(buffer2, 0, buffer2.Length);

        Assert.That(buffer2, Is.EquivalentTo(buffer));
    }
}
