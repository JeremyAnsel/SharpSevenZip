using SharpSevenZip.Lzma;

namespace SharpSevenZip.Tests;

[TestFixture]
public class MiscellaneousTests : TestBase
{
    //[Test]
    //public void SerializationTest()
    //{
    //    var argumentException = new ArgumentException("blahblah");
    //    var binaryFormatter = new BinaryFormatter();

    //    using var ms = new MemoryStream();
    //    using var fileStream = File.Create(TemporaryFile);
    //    binaryFormatter.Serialize(ms, argumentException);
    //    var compressor = new SharpSevenZipCompressor();
    //    compressor.CompressStream(ms, fileStream);
    //}

    [Test]
    public void CreateSfxArchiveTest([Values] SfxModule sfxModule)
    {
        if (sfxModule.HasFlag(SfxModule.Custom))
        {
            Assert.Ignore("No idea how to use SfxModule \"Custom\".");
        }

        var sfxFile = Path.Combine(OutputDirectory, "sfx.exe");
        var sfx = new SharpSevenZipSfx(sfxModule);
        var compressor = new SharpSevenZipCompressor { DirectoryStructure = false };

        compressor.CompressFiles(TemporaryFile, @"TestData\zip.zip");

        sfx.MakeSfx(TemporaryFile, sfxFile);

        Assert.That(File.Exists(sfxFile), Is.True);

        using (var extractor = new SharpSevenZipExtractor(sfxFile))
        {
            Assert.Multiple(() =>
            {
                Assert.That(extractor.FilesCount, Is.EqualTo(1));
                Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("zip.zip"));
            });
        }

        if (sfxModule == SfxModule.Installer)
        {
            // Installer modules need to be run with elevation.
            Assert.Pass("Assume SFX installer works...");
            return;
        }

        //Assert.DoesNotThrow(() =>
        //{
        //    var process = Process.Start(sfxFile);
        //    process?.Kill();
        //});
    }

    [Test]
    public void LzmaEncodeDecodeTest()
    {
        using (var output = new FileStream(TemporaryFile, FileMode.Create))
        {
            var encoder = new LzmaEncodeStream(output);
            using (var inputSample = new FileStream(@"TestData\zip.zip", FileMode.Open))
            {
                int bufSize = 24576, count;
                var buf = new byte[bufSize];

                while ((count = inputSample.Read(buf, 0, bufSize)) > 0)
                {
                    encoder.Write(buf, 0, count);
                }
            }

            encoder.Close();
        }

        var newZip = Path.Combine(OutputDirectory, "new.zip");

        using (var input = new FileStream(TemporaryFile, FileMode.Open))
        {
            var decoder = new LzmaDecodeStream(input);
            using var output = new FileStream(newZip, FileMode.Create);

            int bufSize = 24576, count;
            var buf = new byte[bufSize];

            while ((count = decoder.Read(buf, 0, bufSize)) > 0)
            {
                output.Write(buf, 0, count);
            }
        }

        Assert.That(File.Exists(newZip), Is.True);

        using var extractor = new SharpSevenZipExtractor(newZip);

        Assert.Multiple(() =>
        {
            Assert.That(extractor.FilesCount, Is.EqualTo(1));
            Assert.That(extractor.ArchiveFileNames[0], Is.EqualTo("zip.txt"));
        });
    }
}
