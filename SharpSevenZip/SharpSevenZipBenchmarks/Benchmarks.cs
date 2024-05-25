using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Jobs;
using SharpCompress.Archives;
using SharpSevenZip;
using System.IO.Compression;

namespace SharpSevenZipBenchmarks;

[SimpleJob(RuntimeMoniker.Net48, 1, 1, 1, id: nameof(RuntimeMoniker.Net48), baseline: true)]
[SimpleJob(RuntimeMoniker.Net60, 1, 1, 1, id: nameof(RuntimeMoniker.Net60))]
[SimpleJob(RuntimeMoniker.Net80, 1, 1, 1, id: nameof(RuntimeMoniker.Net80))]
[Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.SlowestToFastest)]
[HideColumns(Column.Runtime)]
[MemoryDiagnoser(false)]
public class Benchmarks
{
    public const string Test1Archive = "test.zip";

    [GlobalSetup]
    public void GlobalSetup()
    {
        CreateEmptyZip();
        CreateTestZip();
    }

    private static void CreateEmptyZip()
    {
        File.Delete("empty.zip");

        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.Deflate,
            CompressionLevel = SharpSevenZip.CompressionLevel.High,
            DirectoryStructure = false,
        };

        compressor.CompressFiles("empty.zip");
    }

    private static void CreateTestZip()
    {
        File.Delete("test.zip");

        var compressor = new SharpSevenZipCompressor
        {
            ArchiveFormat = OutArchiveFormat.Zip,
            CompressionMethod = CompressionMethod.Deflate,
            CompressionLevel = SharpSevenZip.CompressionLevel.High,
            DirectoryStructure = false,
        };

        using var fileStream = new MemoryStream();
        var random = new Random(0);
        var bytes = new byte[1 << 20];
        random.NextBytes(bytes);
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Seek(0, SeekOrigin.Begin);

        Dictionary<string, Stream> files = new()
        {
            {"test.bin", fileStream },
        };

        compressor.CompressStreamDictionary(files, "test.zip");
    }

    [Benchmark]
    public long Decompress_DotNetFramework_Empty()
    {
        long length = 0;

        using (ZipArchive zip = ZipFile.OpenRead("empty.zip"))
        {
            foreach (var entry in zip.Entries)
            {
                length += entry.Length;
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_SharpCompress_Empty()
    {
        long length = 0;

        using (IArchive zip = ArchiveFactory.Open("empty.zip"))
        {
            foreach (var entry in zip.Entries)
            {
                length += entry.Size;
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_SevenZipSharp_Empty()
    {
        long length = 0;

        using (SevenZip.SevenZipExtractor zip = new("empty.zip"))
        {
            foreach (var entry in zip.ArchiveFileData)
            {
                length += (long)entry.Size;
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_SharpSevenZip_Empty()
    {
        long length = 0;

        using (SharpSevenZipExtractor zip = new("empty.zip"))
        {
            foreach (var entry in zip.ArchiveFileData)
            {
                length += (long)entry.Size;
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_DotNetFramework_Sum1()
    {
        long length = 0;

        using (ZipArchive zip = ZipFile.OpenRead(Test1Archive))
        {
            foreach (var entry in zip.Entries)
            {
                length += entry.Length;

                using var file = new MemoryStream((int)entry.Length);
                using var entryStream = entry.Open();
                entryStream.CopyTo(file);
                file.Seek(0, SeekOrigin.Begin);
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_SharpCompress_Sum1()
    {
        long length = 0;

        using (IArchive zip = ArchiveFactory.Open(Test1Archive))
        {
            foreach (var entry in zip.Entries)
            {
                length += entry.Size;

                using var file = new MemoryStream((int)entry.Size);
                entry.WriteTo(file);
                file.Seek(0, SeekOrigin.Begin);
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_SevenZipSharp_Sum1()
    {
        long length = 0;

        using (SevenZip.SevenZipExtractor zip = new(Test1Archive))
        {
            foreach (var entry in zip.ArchiveFileData)
            {
                length += (long)entry.Size;

                using var file = new MemoryStream((int)entry.Size);
                zip.ExtractFile(entry.Index, file);
                file.Seek(0, SeekOrigin.Begin);
            }
        }

        return length;
    }

    [Benchmark]
    public long Decompress_SharpSevenZip_Sum1()
    {
        long length = 0;

        using (SharpSevenZipExtractor zip = new(Test1Archive))
        {
            foreach (var entry in zip.ArchiveFileData)
            {
                length += (long)entry.Size;

                using var file = new MemoryStream((int)entry.Size);
                zip.ExtractFile(entry.Index, file);
                file.Seek(0, SeekOrigin.Begin);
            }
        }

        return length;
    }
}
