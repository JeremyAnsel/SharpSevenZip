using SharpSevenZip.Exceptions;

namespace SharpSevenZip.Tests;

[TestFixture]
public class SharpSevenZipCompressorAsynchronousTests : TestBase
{
    [Test]
    public void AsynchronousCompressDirectoryAndEventsTest()
    {
        var filesFoundInvoked = 0;
        var fileCompressionStartedInvoked = 0;
        var fileCompressionFinishedInvoked = 0;
        var compressingInvoked = 0;
        var compressionFinishedInvoked = 0;

        var compressor = new SharpSevenZipCompressor();

        compressor.FilesFound += (o, e) => filesFoundInvoked++;
        compressor.FileCompressionStarted += (o, e) => fileCompressionStartedInvoked++;
        compressor.FileCompressionFinished += (o, e) => fileCompressionFinishedInvoked++;
        compressor.Compressing += (o, e) => compressingInvoked++;
        compressor.CompressionFinished += (o, e) => compressionFinishedInvoked++;

        compressor.BeginCompressDirectory(@"TestData", TemporaryFile);

        var timeToWait = 10000;
        while (compressionFinishedInvoked == 0)
        {
            if (timeToWait <= 0)
            {
                break;
            }

            Thread.Sleep(25);
            //timeToWait -= 25;
        }

        var numberOfTestDataFiles = Directory.GetFiles("TestData").Length;

        Assert.Multiple(() =>
        {
            Assert.That(filesFoundInvoked, Is.EqualTo(1));
            Assert.That(fileCompressionStartedInvoked, Is.EqualTo(numberOfTestDataFiles));
            Assert.That(fileCompressionFinishedInvoked, Is.EqualTo(numberOfTestDataFiles));
            Assert.That(compressingInvoked, Is.EqualTo(numberOfTestDataFiles));
            Assert.That(compressionFinishedInvoked, Is.EqualTo(1));

            Assert.That(File.Exists(TemporaryFile), Is.True);
        });
    }

    [Test]
    public void AsynchronousCompressFilesTest()
    {
        var compressionFinishedInvoked = false;

        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        compressor.CompressionFinished += (o, e) => compressionFinishedInvoked = true;

        compressor.BeginCompressFiles(TemporaryFile, @"TestData\zip.zip", @"TestData\tar.tar");

        var timeToWait = 10000;
        while (!compressionFinishedInvoked)
        {
            if (timeToWait <= 0)
            {
                break;
            }

            Thread.Sleep(25);
            //timeToWait -= 25;
        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);

        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });
    }

    [Test]
    public void AsynchronousCompressStreamTest()
    {
        var compressionFinishedInvoked = false;

        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        compressor.CompressionFinished += (o, e) => compressionFinishedInvoked = true;

        using (var inputStream = File.OpenRead(@"TestData\zip.zip"))
        {
            using var outputStream = new FileStream(TemporaryFile, FileMode.Create);
            compressor.BeginCompressStream(inputStream, outputStream);

            var timeToWait = 10000;
            while (!compressionFinishedInvoked)
            {
                if (timeToWait <= 0)
                {
                    break;
                }

                Thread.Sleep(25);
                //timeToWait -= 25;
            }
        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.That(extractor.FilesCount, Is.EqualTo(1));
    }

    [Test]
    public void AsynchronousModifyArchiveTest()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };

        compressor.CompressFiles(TemporaryFile, @"TestData\tar.tar");

        var compressionFinishedInvoked = false;
        compressor.CompressionFinished += (o, e) => compressionFinishedInvoked = true;

        compressor.BeginModifyArchive(TemporaryFile, new Dictionary<int, string?> { { 0, @"tartar" } });

        var timeToWait = 10000;
        while (!compressionFinishedInvoked)
        {
            if (timeToWait <= 0)
            {
                break;
            }

            Thread.Sleep(25);
            //timeToWait -= 25;
        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("tartar"));
        });
    }

    [Test]
    public void AsynchronousCompressFilesEncryptedTest()
    {
        var compressionFinishedInvoked = false;

        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        compressor.CompressionFinished += (o, e) => compressionFinishedInvoked = true;

        compressor.BeginCompressFilesEncrypted(TemporaryFile, "secure", @"TestData\zip.zip", @"TestData\tar.tar");

        var timeToWait = 10000;
        while (!compressionFinishedInvoked)
        {
            if (timeToWait <= 0)
            {
                break;
            }

            Thread.Sleep(25);
            //timeToWait -= 25;
        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });

        Assert.Throws<ExtractionFailedException>(() => extractor.ExtractArchive(OutputDirectory));
    }

    [Test]
    public async Task CompressFilesAsync()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        await compressor.CompressFilesAsync(TemporaryFile, @"TestData\zip.zip", @"TestData\tar.tar");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
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
        Assert.Multiple(() =>
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
        await compressor.CompressFilesEncryptedAsync(TemporaryFile, "secure", @"TestData\zip.zip", @"TestData\tar.tar");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile, "insecure");
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("zip.zip"));
            Assert.That(extractor.ArchiveFileNames, Does.Contain("tar.tar"));
        });

        Assert.Throws<ExtractionFailedException>(() => extractor.ExtractArchive(OutputDirectory));
    }
}
