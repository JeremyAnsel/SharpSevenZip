namespace SharpSevenZip.Tests;

[TestFixture]
public class SharpSevenZipExtractorTests : TestBase
{
    public static List<TestFile> TestFiles
    {
        get
        {
            var result = new List<TestFile>();

            foreach (var file in Directory.GetFiles(Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData")))
            {
                if (file.Contains("multi") || file.Contains("long_path"))
                {
                    continue;
                }

                result.Add(new TestFile(file));
            }

            return result;
        }
    }

    [Test]
    public void ExtractFilesTest()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            for (var i = 0; i < extractor.ArchiveFileData.Count; i++)
            {
                extractor.ExtractFiles(OutputDirectory, extractor.ArchiveFileData[i].Index);
            }
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(3));
    }

    [Test]
    public void ExtractSpecificFilesTest()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            extractor.ExtractFiles(OutputDirectory, 0, 2);
            Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
        Assert.That(Directory.GetFiles(OutputDirectory), Has.Some.Contain(Path.Combine(OutputDirectory, "file1.txt")));
        Assert.That(Directory.GetFiles(OutputDirectory), Has.Some.Contain(Path.Combine(OutputDirectory, "file3.txt")));
    }

    [Test]
    public void ExtractArchiveMultiVolumesTest()
    {
        using (var extractor = new SharpSevenZipExtractor(@"TestData\multivolume.part0001.rar"))
        {
            extractor.ExtractArchive(OutputDirectory);
        }

        Assert.Multiple(() =>
        {
            Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(1));
            Assert.That(File.ReadAllText(Directory.GetFiles(OutputDirectory)[0]), Does.StartWith("Lorem ipsum dolor sit amet"));
        });
    }

    [Test]
    public void ExtractionWithCancellationTest()
    {
        using (var tmp = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            tmp.FileExtractionStarted += (_, args) =>
            {
                if (args.FileInfo.Index == 2)
                {
                    args.Cancel = true;
                }
            };

            tmp.ExtractArchive(OutputDirectory);
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
    }

    [Test]
    public void ExtractionWithSkipTest()
    {
        using (var tmp = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            tmp.FileExtractionStarted += (_, args) =>
            {
                if (args.FileInfo.Index == 1)
                {
                    args.Skip = true;
                }
            };

            tmp.ExtractArchive(OutputDirectory);
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(2));
    }

    [Test]
    public void ExtractionFromStreamTest()
    {
        // TODO: Rewrite this to test against more/all TestData archives.

        using var tmp = new SharpSevenZipExtractor(File.OpenRead(@"TestData\multiple_files.7z"));
        tmp.ExtractArchive(OutputDirectory);
        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(3));
    }

    [Test]
    public void ExtractionFromStream_LeaveStreamOpenTest()
    {
        using var fileStream = new FileStream(@"TestData\multiple_files.7z", FileMode.Open);
        using (var extractor1 = new SharpSevenZipExtractor(fileStream, leaveOpen: true))
        {
            extractor1.ExtractArchive(OutputDirectory);

            Assert.That(fileStream.CanRead, Is.True);
        }

        using (var extractor2 = new SharpSevenZipExtractor(fileStream, leaveOpen: false))
        {
            extractor2.ExtractArchive(OutputDirectory);
        }

        Assert.That(fileStream.CanRead, Is.False);
    }

    [Test]
    public void ExtractionToStreamTest()
    {
        using (var tmp = new SharpSevenZipExtractor(@"TestData\multiple_files.7z"))
        {
            using var fileStream = new FileStream(Path.Combine(OutputDirectory, "streamed_file.txt"), FileMode.Create);
            tmp.ExtractFile(1, fileStream);
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(1));

        var extractedFile = Directory.GetFiles(OutputDirectory)[0];

        Assert.That(File.ReadAllText(extractedFile), Is.EqualTo("file2"));
    }

    [Test]
    public void DetectMultiVolumeIndexTest()
    {
        using (var tmp = new SharpSevenZipExtractor(@"TestData\multivolume.part0001.rar"))
        {
            Assert.Multiple(() =>
            {
                Assert.That(tmp.ArchiveProperties.Any(x => x.Name.Equals("IsVolume") && x.Value != null && x.Value.Equals(true)), Is.True);
                Assert.That(tmp.ArchiveProperties.Any(x => x.Name.Equals("VolumeIndex") && x.Value != null && Convert.ToInt32(x.Value) == 0), Is.True);
            });
        }

        using (var tmp = new SharpSevenZipExtractor(@"TestData\multivolume.part0002.rar"))
        {
            Assert.Multiple(() =>
            {
                Assert.That(tmp.ArchiveProperties.Any(x => x.Name.Equals("IsVolume") && x.Value != null && x.Value.Equals(true)), Is.True);
                Assert.That(tmp.ArchiveProperties.Any(x => x.Name.Equals("VolumeIndex") && x.Value != null && Convert.ToInt32(x.Value) == 0), Is.False);
            });
        }
    }

    [Test]
    public void ThreadedExtractionTest()
    {
        var destination1 = Path.Combine(OutputDirectory, "t1");
        var destination2 = Path.Combine(OutputDirectory, "t2");

        var t1 = new Thread(() =>
        {
            using var tmp = new SharpSevenZipExtractor(@"TestData\multiple_files.7z");
            tmp.ExtractArchive(destination1);
        });

        var t2 = new Thread(() =>
        {
            using var tmp = new SharpSevenZipExtractor(@"TestData\multiple_files.7z");
            tmp.ExtractArchive(destination2);
        });

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        Assert.Multiple(() =>
        {
            Assert.That(Directory.Exists(destination1), Is.True);
            Assert.That(Directory.Exists(destination2), Is.True);
            Assert.That(Directory.GetFiles(destination1), Has.Length.EqualTo(3));
            Assert.That(Directory.GetFiles(destination2), Has.Length.EqualTo(3));
        });
    }

    [Test, Ignore("Figure out why this fails, later.")]
    public void ExtractArchiveWithLongPath()
    {
        using var extractor = new SharpSevenZipExtractor(@"TestData\long_path.7z");
        Assert.Throws<PathTooLongException>(() => extractor.ExtractArchive(OutputDirectory));
    }

    [Test]
    public void ReadArchivedFileNames()
    {
        using var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z");
        var fileNames = extractor.ArchiveFileNames;
        Assert.That(fileNames, Has.Count.EqualTo(3));

        Assert.Multiple(() =>
        {
            Assert.That(fileNames[0], Is.EqualTo("file1.txt"));
            Assert.That(fileNames[1], Is.EqualTo("file2.txt"));
            Assert.That(fileNames[2], Is.EqualTo("file3.txt"));
        });
    }

    [Test]
    public void ReadArchivedFileData()
    {
        using var extractor = new SharpSevenZipExtractor(@"TestData\multiple_files.7z");
        var fileData = extractor.ArchiveFileData;
        Assert.That(fileData, Has.Count.EqualTo(3));

        Assert.Multiple(() =>
        {
            Assert.That(fileData[0].FileName, Is.EqualTo("file1.txt"));
            Assert.That(fileData[0].Encrypted, Is.False);
            Assert.That(fileData[0].IsDirectory, Is.False);
        });
    }

    [Test, TestCaseSource(nameof(TestFiles))]
    public void ExtractDifferentFormatsTest(TestFile file)
    {
        using (var extractor = new SharpSevenZipExtractor(file.FilePath))
        {
            extractor.ExtractArchive(OutputDirectory);
        }

        Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(1));
    }
}

/// <summary>
/// Simple wrapper to get better names for ExtractDifferentFormatsTest results.
/// </summary>
public class TestFile
{
    public string FilePath { get; }

    public TestFile(string filePath)
    {
        FilePath = filePath;
    }

    public override string ToString()
    {
        return Path.GetFileName(FilePath);
    }
}
