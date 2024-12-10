using SharpSevenZip.Exceptions;

namespace SharpSevenZip.Tests;

[TestFixture]
public class SharpSevenZipCompressorTests : TestBase
{
    /// <summary>
    /// TestCaseSource for CompressDifferentFormatsTest
    /// </summary>
    public static List<CompressionMethod> CompressionMethods
    {
        get
        {
            var result = new List<CompressionMethod>();
            foreach (CompressionMethod format in Enum.GetValues(typeof(CompressionMethod)))
            {
                result.Add(format);
            }

            return result;
        }
    }

    [Test]
    public void CompressDirectory_WithSfnPath()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            PreserveDirectoryRoot = true
        };

        compressor.CompressDirectory("TESTDA~1", TemporaryFile);
        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0].StartsWith("TestData_LongerDirectoryName", StringComparison.OrdinalIgnoreCase), Is.True);
        });
    }

    [Test]
    public void CompressDirectory_NonExistentDirectory()
    {
        var compressor = new SharpSevenZipCompressor();

        Assert.Throws<ArgumentException>(() => compressor.CompressDirectory("nonexistent", TemporaryFile));
        Assert.Throws<ArgumentException>(() => compressor.CompressDirectory("", TemporaryFile));
    }

    [Test]
    public void CompressFile_WithSfnPath()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip
        };

        compressor.CompressFiles(TemporaryFile, @"TESTDA~1\emptyfile.txt");
        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("emptyfile.txt"));
        });
    }

    [Test]
    public void CompressFileTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            DirectoryStructure = false
        };

        compressor.CompressFiles(TemporaryFile, @"Testdata\7z_LZMA2.7z");
        Assert.That(File.Exists(TemporaryFile), Is.True);

        using (var extractor = new SharpSevenZipExtractor(TemporaryFile))
        {
            extractor.ExtractArchive(OutputDirectory);
        }

        Assert.That(File.Exists(Path.Combine(OutputDirectory, "7z_LZMA2.7z")), Is.True);
    }

    [Test]
    public void CompressDirectoryTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            DirectoryStructure = false
        };

        compressor.CompressDirectory("TestData", TemporaryFile);
        Assert.That(File.Exists(TemporaryFile), Is.True);

        using (var extractor = new SharpSevenZipExtractor(TemporaryFile))
        {
            extractor.ExtractArchive(OutputDirectory);
        }

        File.Delete(TemporaryFile);

        Assert.That(Directory.GetFiles(OutputDirectory).Select(Path.GetFileName).ToArray(), Is.EqualTo(Directory.GetFiles("TestData").Select(Path.GetFileName).ToArray()));
    }

    [Test]
    public void CompressWithAppendModeTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            DirectoryStructure = false
        };

        compressor.CompressFiles(TemporaryFile, @"Testdata\7z_LZMA2.7z");
        Assert.That(File.Exists(TemporaryFile), Is.True);

        using (var extractor = new SharpSevenZipExtractor(TemporaryFile))
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
        }

        compressor.CompressionMode = CompressionMode.Append;

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");

        using (var extractor = new SharpSevenZipExtractor(TemporaryFile))
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(2));
        }
    }

    [Test]
    public void ModifyProtectedArchiveTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            DirectoryStructure = false,
            EncryptHeaders = true
        };

        compressor.CompressFilesEncrypted(TemporaryFile, "password", @"TestData\7z_LZMA2.7z", @"TestData\zip.zip");

        var modificationList = new Dictionary<int, string?>
            {
                {0, "changed.zap"},
                {1, null }
            };

        compressor.ModifyArchive(TemporaryFile, modificationList, "password");

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile, "password");
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("changed.zap"));
        });
    }

    [Test]
    public void ModifyNonArchiveTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            DirectoryStructure = false
        };

        File.WriteAllText(TemporaryFile, "I'm not an archive.");

        var modificationList = new Dictionary<int, string?> { { 0, "" } };

        Assert.Throws<SharpSevenZipArchiveException>(() => compressor.ModifyArchive(TemporaryFile, modificationList));
    }

    [Test]
    public void CompressWithModifyModeRenameTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            DirectoryStructure = false
        };

        compressor.CompressFiles(TemporaryFile, @"Testdata\7z_LZMA2.7z");
        Assert.That(File.Exists(TemporaryFile), Is.True);

        compressor.ModifyArchive(TemporaryFile, new Dictionary<int, string?> { { 0, "renamed.7z" } });

        using (var extractor = new SharpSevenZipExtractor(TemporaryFile))
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            extractor.ExtractArchive(OutputDirectory);
        }

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(Path.Combine(OutputDirectory, "renamed.7z")), Is.True);
            Assert.That(File.Exists(Path.Combine(OutputDirectory, "7z_LZMA2.7z")), Is.False);
        });
    }

    [Test]
    public void CompressWithModifyModeDeleteTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            DirectoryStructure = false
        };

        compressor.CompressFiles(TemporaryFile, @"Testdata\7z_LZMA2.7z");
        Assert.That(File.Exists(TemporaryFile), Is.True);

        compressor.ModifyArchive(TemporaryFile, new Dictionary<int, string?> { { 0, null } });

        using (var extractor = new SharpSevenZipExtractor(TemporaryFile))
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(0));
            extractor.ExtractArchive(OutputDirectory);
        }

        Assert.That(File.Exists(Path.Combine(OutputDirectory, "7z_LZMA2.7z")), Is.False);
    }

    [Test]
    public void MultiVolumeCompressionTest()
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            DirectoryStructure = false,
            VolumeSize = 100
        };

        compressor.CompressFiles(TemporaryFile, @"Testdata\7z_LZMA2.7z");

        Assert.Multiple(() =>
        {
            Assert.That(Directory.GetFiles(OutputDirectory), Has.Length.EqualTo(3));
            Assert.That(File.Exists($"{TemporaryFile}.003"), Is.True);
        });
    }

    [Test]
    public void CompressToStreamTest()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };

        using (var stream = File.Create(TemporaryFile))
        {
            compressor.CompressFiles(stream, @"TestData\zip.zip");
        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("zip.zip"));
        });
    }

    [Test]
    public void CompressFromStreamTest()
    {
        using (var input = File.OpenRead(@"TestData\zip.zip"))
        {
            using var output = File.Create(TemporaryFile);
            var compressor = new SharpSevenZipCompressor
            {
                DirectoryStructure = false
            };

            compressor.CompressStream(input, output);

        }

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileData[0].Size, Is.EqualTo(new FileInfo(@"TestData\zip.zip").Length));
        });
    }

    [Test]
    public void CompressFileDictionaryTest()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };

        var fileDict = new Dictionary<string, string>
            {
                {"zip.zip", @"TestData\zip.zip"}
            };

        compressor.CompressFileDictionary(fileDict, TemporaryFile);

        Assert.That(File.Exists(TemporaryFile), Is.True);

        using var extractor = new SharpSevenZipExtractor(TemporaryFile);
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("zip.zip"));
        });
    }

    [Test]
    public void CompressStreamDictionaryTest()
    {
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };
        
        // Add creation and access time to the archive (write time is added automatically)
        compressor.CustomParameters.Add("tc", "on"); // Add creation time to archive files
        compressor.CustomParameters.Add("ta", "on"); // Add last access time to archive files

        DateTime creationTime = DateTime.Now.AddYears(-1);
        DateTime lastWriteTime = DateTime.Now.AddMonths(-1);
        DateTime lastAccessTime = DateTime.Now.AddDays(-1);

        var fileDict = new Dictionary<string, StreamWithAttributes>
            {
                {"zip.zip", new (new MemoryStream(), creationTime, lastWriteTime, lastAccessTime)}
            };

        compressor.CompressStreamDictionary(fileDict, TemporaryFile);

        Assert.That(File.Exists(TemporaryFile), Is.True);
        {
            using var extractor = new SharpSevenZipExtractor(TemporaryFile);
            Assert.Multiple(() =>
            {
                Assert.That(extractor.FilesCount, Is.EqualTo(1));
                Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("zip.zip"));
                Assert.That(extractor.ArchiveFileData[0].CreationTime, Is.EqualTo(creationTime));
                Assert.That(extractor.ArchiveFileData[0].LastWriteTime, Is.EqualTo(lastWriteTime));
                Assert.That(extractor.ArchiveFileData[0].LastAccessTime, Is.EqualTo(lastAccessTime));
            });
        }

        // Test new compressor in append mode
        compressor = new SharpSevenZipCompressor { DirectoryStructure = false, CompressionMode = CompressionMode.Append };
        compressor.CustomParameters.Add("tc", "on"); // Add creation time to archive files
        compressor.CustomParameters.Add("ta", "on"); // Add access
        
        creationTime = creationTime.AddYears(-1);
        lastWriteTime = lastWriteTime.AddMonths(-1);
        lastAccessTime = lastAccessTime.AddDays(-1);
        fileDict = new Dictionary<string, StreamWithAttributes>
            {
                {"zip2.zip", new (new MemoryStream(), creationTime, lastWriteTime, lastAccessTime)}
            };
        compressor.CompressStreamDictionary(fileDict, TemporaryFile);
        {
            using var extractor = new SharpSevenZipExtractor(TemporaryFile);
            Assert.Multiple(() =>
            {
                Assert.That(extractor.FilesCount, Is.EqualTo(2));
                Assert.That(extractor.ArchiveFileNames[1], Is.EqualTo("zip2.zip"));
                Assert.That(extractor.ArchiveFileData[1].CreationTime, Is.EqualTo(creationTime));
                Assert.That(extractor.ArchiveFileData[1].LastWriteTime, Is.EqualTo(lastWriteTime));
                Assert.That(extractor.ArchiveFileData[1].LastAccessTime, Is.EqualTo(lastAccessTime));
            });
        }
    }

    [Test]
    public void ThreadedCompressionTest()
    {
        var tempFile1 = Path.Combine(OutputDirectory, "t1.7z");
        var tempFile2 = Path.Combine(OutputDirectory, "t2.7z");

        var t1 = new Thread(() =>
        {
            var tmp = new SharpSevenZipCompressor();
            tmp.CompressDirectory("TestData", tempFile1);
        });

        var t2 = new Thread(() =>
        {
            var tmp = new SharpSevenZipCompressor();
            tmp.CompressDirectory("TestData", tempFile2);
        });

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();

        Assert.Multiple(() =>
        {
            Assert.That(File.Exists(tempFile1), Is.True);
            Assert.That(File.Exists(tempFile2), Is.True);
        });
    }

    [Test, TestCaseSource(nameof(CompressionMethods))]
    public void CompressDifferentFormatsTest(CompressionMethod method)
    {
        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            CompressionMethod = method
        };

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");

        Assert.That(File.Exists(TemporaryFile), Is.True);
    }

    [Test]
    public void AppendToArchiveWithEncryptedHeadersTest()
    {
        var compressor = new SharpSevenZipCompressor()
        {
            ArchiveFormat = OutArchiveFormat.SevenZip,
            CompressionMethod = CompressionMethod.Lzma2,
            CompressionLevel = CompressionLevel.Normal,
            EncryptHeaders = true,
        };
        compressor.CompressDirectory(@"TestData", TemporaryFile, "password");

        compressor = new SharpSevenZipCompressor
        {
            CompressionMode = CompressionMode.Append
        };

        compressor.CompressFilesEncrypted(TemporaryFile, "password", @"TestData\zip.zip");
    }

    [Test]
    public void AppendEncryptedFileToStreamTest()
    {
        using (var fileStream = new FileStream(TemporaryFile, FileMode.Create))
        {
            var compressor = new SharpSevenZipCompressor
            {
                ArchiveFormat = OutArchiveFormat.SevenZip,
                CompressionMethod = CompressionMethod.Lzma2,
                CompressionMode = CompressionMode.Append,
                ZipEncryptionMethod = ZipEncryptionMethod.Aes256,
                CompressionLevel = CompressionLevel.Normal,
                EncryptHeaders = true
            };

            compressor.CompressFilesEncrypted(fileStream, "password", @"TestData\zip.zip");
        }

        using var extractor = new SharpSevenZipExtractor(TemporaryFile, "password");
        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("zip.zip"));
        });
    }
}
