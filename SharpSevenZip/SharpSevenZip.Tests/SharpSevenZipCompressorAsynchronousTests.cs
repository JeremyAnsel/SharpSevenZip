using SharpSevenZip.Exceptions;

namespace SharpSevenZip.Tests;

[TestFixture]
public class SharpSevenZipCompressorAsynchronousTests : TestBase
{
    private static readonly TimeSpan CompressionTimeout = TimeSpan.FromSeconds(10);

    [Test]
    public void AsynchronousCompressDirectoryAndEventsTest()
    {
        var filesFoundInvoked = 0;
        var fileCompressionStartedInvoked = 0;
        var fileCompressionFinishedInvoked = 0;
        var compressingInvoked = 0;
        using var compressionFinished = new ManualResetEventSlim(false);

        var compressor = new SharpSevenZipCompressor();

        compressor.FilesFound += (o, e) => Interlocked.Increment(ref filesFoundInvoked);
        compressor.FileCompressionStarted += (o, e) => Interlocked.Increment(ref fileCompressionStartedInvoked);
        compressor.FileCompressionFinished += (o, e) => Interlocked.Increment(ref fileCompressionFinishedInvoked);
        compressor.Compressing += (o, e) => Interlocked.Increment(ref compressingInvoked);
        compressor.CompressionFinished += (o, e) => compressionFinished.Set();

        compressor.BeginCompressDirectory(@"TestData", TemporaryFile);

        Assert.That(compressionFinished.Wait(CompressionTimeout), Is.True, "Compression did not finish in time.");

        var numberOfTestDataFiles = Directory.GetFiles("TestData").Length;

        Assert.Multiple((Action)delegate
        {
            Assert.That(Volatile.Read(ref filesFoundInvoked), Is.EqualTo(1));
            Assert.That(Volatile.Read(ref fileCompressionStartedInvoked), Is.EqualTo(numberOfTestDataFiles));
            Assert.That(Volatile.Read(ref fileCompressionFinishedInvoked), Is.EqualTo(numberOfTestDataFiles));
            // Assert.That(compressingInvoked, Is.EqualTo(numberOfTestDataFiles));

            Assert.That(File.Exists(TemporaryFile), Is.True);
        });
    }

    [Test]
    public void AsynchronousCompressFilesTest()
    {
        using var compressionFinished = new ManualResetEventSlim(false);

        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        compressor.CompressionFinished += (o, e) => compressionFinished.Set();

        compressor.BeginCompressFiles(TemporaryFile, @"TestData/zip.zip", @"TestData/tar.tar");

        Assert.That(compressionFinished.Wait(CompressionTimeout), Is.True, "Compression did not finish in time.");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);

        Assert.Multiple((Action)delegate
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });
    }

    [Test]
    public void AsynchronousCompressStreamTest()
    {
        using var compressionFinished = new ManualResetEventSlim(false);

        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        compressor.CompressionFinished += (o, e) => compressionFinished.Set();

        using (var inputStream = File.OpenRead(@"TestData/zip.zip"))
        {
            using var outputStream = new FileStream(TemporaryFile, FileMode.Create);
            compressor.BeginCompressStream(inputStream, outputStream);

            Assert.That(compressionFinished.Wait(CompressionTimeout), Is.True, "Compression did not finish in time.");
        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.That(extractor.FilesCount, Is.EqualTo(1));
    }

    [Test]
    public void AsynchronousModifyArchiveTest()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };

        compressor.CompressFiles(TemporaryFile, @"TestData/tar.tar");

        using var compressionFinished = new ManualResetEventSlim(false);
        compressor.CompressionFinished += (o, e) => compressionFinished.Set();

        compressor.BeginModifyArchive(TemporaryFile, new Dictionary<int, string?> { { 0, @"tartar" } });

        Assert.That(compressionFinished.Wait(CompressionTimeout), Is.True, "Compression did not finish in time.");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple((Action)delegate
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("tartar"));
        });
    }

    [Test]
    public void AsynchronousCompressFilesEncryptedTest()
    {
        using var compressionFinished = new ManualResetEventSlim(false);

        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        compressor.CompressionFinished += (o, e) => compressionFinished.Set();

        compressor.BeginCompressFilesEncrypted(TemporaryFile, "secure", @"TestData/zip.zip", @"TestData/tar.tar");

        Assert.That(compressionFinished.Wait(CompressionTimeout), Is.True, "Compression did not finish in time.");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple((Action)delegate
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });

        Assert.Throws<ExtractionFailedException>((Action)(() => extractor.ExtractArchive(OutputDirectory)));
    }

    [Test]
    public async Task CompressFilesAsync()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        await compressor.CompressFilesAsync(TemporaryFile, @"TestData/zip.zip", @"TestData/tar.tar");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple((Action)delegate
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });
    }

    [Test]
    public async Task CompressDirectoryAsync()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        await compressor.CompressDirectoryAsync("TestData", TemporaryFile);

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple((Action)delegate
        {
            Assert.That(Directory.GetFiles("TestData"), Has.Length.EqualTo(extractor.FilesCount));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });
    }

    [Test]
    public async Task CompressFilesEncryptedAsync()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        await compressor.CompressFilesEncryptedAsync(TemporaryFile, "secure", @"TestData/zip.zip", @"TestData/tar.tar");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile, "insecure");
        Assert.Multiple((Action)delegate
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });

        Assert.Throws<ExtractionFailedException>((Action)(() => extractor.ExtractArchive(OutputDirectory)));
    }
}
