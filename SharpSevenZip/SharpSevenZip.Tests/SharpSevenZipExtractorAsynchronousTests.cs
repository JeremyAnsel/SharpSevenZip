namespace SharpSevenZip.Tests;

[TestFixture]
public class SharpSevenZipExtractorAsynchronousTests : TestBase
{
    [Test]
    public void AsynchronousExtractArchiveEventsTest()
    {
        var extractingInvoked = 0;
        var extractionFinishedInvoked = 0;
        var fileExistsInvoked = 0;
        var fileExtractionStartedInvoked = 0;
        var fileExtractionFinishedInvoked = 0;

        using var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z");
        extractor.EventSynchronization = EventSynchronizationStrategy.AlwaysSynchronous;

        extractor.Extracting += (o, e) => extractingInvoked++;
        extractor.ExtractionFinished += (o, e) => extractionFinishedInvoked++;
        extractor.FileExists += (o, e) => fileExistsInvoked++;
        extractor.FileExtractionStarted += (o, e) => fileExtractionStartedInvoked++;
        extractor.FileExtractionFinished += (o, e) => fileExtractionFinishedInvoked++;

        extractor.BeginExtractArchive(OutputDirectory);

        var timeToWait = 10000;
        while (extractionFinishedInvoked == 0)
        {
            if (timeToWait <= 0)
            {
                break;
            }

            Thread.Sleep(25);
            //timeToWait -= 25;
        }

        Assert.Multiple(() =>
        {
            Assert.That(extractingInvoked, Is.EqualTo(3));
            Assert.That(extractionFinishedInvoked, Is.EqualTo(1));
            Assert.That(fileExistsInvoked, Is.EqualTo(0));
            Assert.That(fileExtractionStartedInvoked, Is.EqualTo(3));
            Assert.That(fileExtractionFinishedInvoked, Is.EqualTo(3));
        });

        extractionFinishedInvoked = 0;
        extractor.BeginExtractArchive(OutputDirectory);

        timeToWait = 10000;
        while (extractionFinishedInvoked == 0)
        {
            if (timeToWait <= 0)
            {
                break;
            }

            Thread.Sleep(25);
            //timeToWait -= 25;
        }

        Assert.That(fileExistsInvoked, Is.EqualTo(3));
    }

    [Test]
    public void AsynchronousExtractFileEventsTest()
    {
        var extractionFinishedInvoked = false;

        using (var fileStream = File.Create(TemporaryFile))
        {
            using var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z");
            extractor.EventSynchronization = EventSynchronizationStrategy.AlwaysSynchronous;
            extractor.ExtractionFinished += (o, e) => extractionFinishedInvoked = true;
            extractor.BeginExtractFile(0, fileStream);

            var maximumTimeToWait = 10000;

            while (!extractionFinishedInvoked)
            {
                if (maximumTimeToWait <= 0)
                {
                    break;
                }

                Thread.Sleep(25);
                //maximumTimeToWait -= 25;
            }

            Thread.Sleep(25);
        }

        Assert.Multiple(() =>
        {
            Assert.That(extractionFinishedInvoked, Is.True);
            Assert.That(File.ReadAllText(TemporaryFile), Is.EqualTo("file1"));
        });
    }

    [Test]
    public void AsynchronousExtractFilesEventsTest()
    {
        var extractionFinishedInvoked = false;

        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            extractor.EventSynchronization = EventSynchronizationStrategy.AlwaysSynchronous;

            extractor.ExtractionFinished += (o, e) => extractionFinishedInvoked = true;

            extractor.BeginExtractFiles(OutputDirectory, 0, 2);

            var timeToWait = 10000;
            while (!extractionFinishedInvoked)
            {
                if (timeToWait <= 0)
                {
                    break;
                }

                Thread.Sleep(25);
                //timeToWait -= 25;
            }

            Thread.Sleep(25);
        }

        Assert.Multiple(() =>
        {
            Assert.That(extractionFinishedInvoked, Is.True);
            Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
        });
    }

    [Test]
    public async Task ExtractArchiveAsync()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            await extractor.ExtractArchiveAsync(OutputDirectory);
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(3));
    }

    [Test]
    public async Task ExtractFileAsync_ByIndex()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            using var fileStream = File.Create(TemporaryFile);
            await extractor.ExtractFileAsync(0, fileStream);
        }

        Assert.Multiple(() =>
        {
            Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(1));
            Assert.That(File.ReadAllText(TemporaryFile), Is.EqualTo("file1"));
        });
    }

    [Test]
    public async Task ExtractFileAsync_ByFileName()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            using var fileStream = File.Create(TemporaryFile);
            await extractor.ExtractFileAsync("file1.txt", fileStream);
        }

        Assert.Multiple(() =>
        {
            Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(1));
            Assert.That(File.ReadAllText(TemporaryFile), Is.EqualTo("file1"));
        });
    }

    [Test]
    public async Task ExtractFilesAsync_ByCallback()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\zip.zip"))
        {
            await extractor.ExtractFilesAsync(args => { args.ExtractToFile = TemporaryFile; });
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(1));
    }

    [Test]
    public async Task ExtractFilesAsync_ByIndex()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            await extractor.ExtractFilesAsync(OutputDirectory, 0, 2);
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
    }

    [Test]
    public async Task ExtractFilesAsync_ByFileName()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            await extractor.ExtractFilesAsync(OutputDirectory, "file1.txt", "file3.txt");
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
    }
}
