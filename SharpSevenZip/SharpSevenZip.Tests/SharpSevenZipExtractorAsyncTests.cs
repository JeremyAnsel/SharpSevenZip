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

    [Test]
    public async Task ExtractFileAsync_TwoFiles_LeaveOpen_True_NotThrow()
    {
        var buffer = new byte[1024];
        var random = new Random(0);
        random.NextBytes(buffer);

        using var archiveStream = new MemoryStream();

        using (var file1Stream = new MemoryStream())
        using (var file2Stream = new MemoryStream())
        {
            file1Stream.Write(buffer, 0, buffer.Length);
            file1Stream.Position = 0;

            file2Stream.Write(buffer, 0, buffer.Length);
            file2Stream.Position = 0;

            var filesDictionary = new Dictionary<string, StreamWithAttributes>()
            {
                {"file1.bin", new StreamWithAttributes(file1Stream) },
                {"file2.bin", new StreamWithAttributes(file2Stream) },
            };

            var compressor = new SharpSevenZipCompressor();
            compressor.CompressStreamDictionary(filesDictionary, archiveStream);
            archiveStream.Position = 0;
        }

        using var fileStream = new MemoryStream();
        using var extractor = new SharpSevenZipExtractor(archiveStream, true);

        uint filesCount = extractor.FilesCount;
        Assert.That(filesCount, Is.EqualTo(2));

        var buffer2 = new byte[1024];

        await extractor.ExtractFileAsync("file1.bin", fileStream);
        fileStream.Position = 0;
        fileStream.Read(buffer2, 0, buffer2.Length);
        Assert.That(buffer2, Is.EquivalentTo(buffer));

        await extractor.ExtractFileAsync("file2.bin", fileStream);
        fileStream.Position = 0;
        fileStream.Read(buffer2, 0, buffer2.Length);
        Assert.That(buffer2, Is.EquivalentTo(buffer));
    }

    [Test]
    public async Task ExtractFileAsync_TwoFiles_LeaveOpen_False_NotThrow()
    {
        var buffer = new byte[1024];
        var random = new Random(0);
        random.NextBytes(buffer);

        using var archiveStream = new MemoryStream();

        using (var file1Stream = new MemoryStream())
        using (var file2Stream = new MemoryStream())
        {
            file1Stream.Write(buffer, 0, buffer.Length);
            file1Stream.Position = 0;

            file2Stream.Write(buffer, 0, buffer.Length);
            file2Stream.Position = 0;

            var filesDictionary = new Dictionary<string, StreamWithAttributes>()
            {
                {"file1.bin", new StreamWithAttributes(file1Stream) },
                {"file2.bin", new StreamWithAttributes(file2Stream) },
            };

            var compressor = new SharpSevenZipCompressor();
            compressor.CompressStreamDictionary(filesDictionary, archiveStream);
            archiveStream.Position = 0;
        }

        using var fileStream = new MemoryStream();
        using var extractor = new SharpSevenZipExtractor(archiveStream, false);

        uint filesCount = extractor.FilesCount;
        Assert.That(filesCount, Is.EqualTo(2));

        var buffer2 = new byte[1024];

        await extractor.ExtractFileAsync("file1.bin", fileStream);
        fileStream.Position = 0;
        fileStream.Read(buffer2, 0, buffer2.Length);
        Assert.That(buffer2, Is.EquivalentTo(buffer));

        await extractor.ExtractFileAsync("file2.bin", fileStream);
        fileStream.Position = 0;
        fileStream.Read(buffer2, 0, buffer2.Length);
        Assert.That(buffer2, Is.EquivalentTo(buffer));

    }

    [Test]
    public async Task ExtractFileAsync_InnerArchive_TwoFiles_NotThrow()
    {
        var buffer = new byte[1024];
        var random = new Random(0);
        random.NextBytes(buffer);

        using var archiveStream = new MemoryStream();

        using (var file1Stream = new MemoryStream())
        using (var file2Stream = new MemoryStream())
        {
            file1Stream.Write(buffer, 0, buffer.Length);
            file1Stream.Position = 0;

            file2Stream.Write(buffer, 0, buffer.Length);
            file2Stream.Position = 0;

            var filesDictionary = new Dictionary<string, StreamWithAttributes>()
            {
                {"file1.bin", new StreamWithAttributes(file1Stream) },
                {"file2.bin", new StreamWithAttributes(file2Stream) },
            };

            using var archive2Stream = new MemoryStream();

            var compressor = new SharpSevenZipCompressor();
            compressor.CompressStreamDictionary(filesDictionary, archive2Stream);
            archive2Stream.Position = 0;

            compressor.DefaultItemName = "archive.7z";
            compressor.CompressStream(archive2Stream, archiveStream);
            archiveStream.Position = 0;
        }

        using var outerFileStream = new MemoryStream();
        using var outter = new SharpSevenZipExtractor(archiveStream, true);
        await outter.ExtractFileAsync("archive.7z", outerFileStream);

        outerFileStream.Position = 0;
        using var inner = new SharpSevenZipExtractor(outerFileStream, true);

        uint filesCount = inner.FilesCount;
        Assert.That(filesCount, Is.EqualTo(2));

        using var innerFileStream = new MemoryStream();
        var buffer2 = new byte[1024];

        await inner.ExtractFileAsync("file1.bin", innerFileStream);
        innerFileStream.Position = 0;
        innerFileStream.Read(buffer2, 0, buffer2.Length);
        Assert.That(buffer2, Is.EquivalentTo(buffer));

        await inner.ExtractFileAsync("file2.bin", innerFileStream);
        innerFileStream.Position = 0;
        innerFileStream.Read(buffer2, 0, buffer2.Length);
        Assert.That(buffer2, Is.EquivalentTo(buffer));
    }
}
