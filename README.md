# SharpSevenZip

A managed C# wrapper around the native 7-Zip library. SharpSevenZip drives `7z.dll` through its COM interface and adds an LZMA SDK implementation and self-extracting archive support on top.

[![Build status](https://ci.appveyor.com/api/projects/status/u6ki6smclwffstjy/branch/main?svg=true)](https://ci.appveyor.com/project/JeremyAnsel/sharpsevenzip/branch/main)
[![NuGet Version](https://img.shields.io/nuget/v/SharpSevenZip)](https://www.nuget.org/packages/SharpSevenZip)
![License](https://img.shields.io/github/license/JeremyAnsel/SharpSevenZip)

| Resource | Link |
| --- | --- |
| Source code | [github.com/JeremyAnsel/SharpSevenZip](https://github.com/JeremyAnsel/SharpSevenZip) |
| Documentation | [jeremyansel.github.io/SharpSevenZip](http://jeremyansel.github.io/SharpSevenZip) |
| NuGet | [SharpSevenZip](https://www.nuget.org/packages/SharpSevenZip) |
| NuGet (strong-named) | [SharpSevenZip.StrongName](https://www.nuget.org/packages/SharpSevenZip.StrongName) |
| Build | [AppVeyor](https://ci.appveyor.com/project/JeremyAnsel/sharpsevenzip/branch/main) |
| License | [LGPL-3.0-or-later](https://github.com/JeremyAnsel/SharpSevenZip/blob/main/LICENSE) |

## Installation

```shell
dotnet add package SharpSevenZip
```

Target frameworks are `net8.0`, `net48` and `netstandard2.0`. The package ships the `x86` and `x64` builds of `7z.dll` and copies them into the corresponding subfolders of the output directory; set the MSBuild property `ExcludeSevenZipAssemblies` to `true` to suppress that.

## Quick start

The library exposes three main types:

| Type | Purpose |
| --- | --- |
| `SharpSevenZipExtractor` | Extracts archives, single entries or LZMA-compressed byte arrays |
| `SharpSevenZipCompressor` | Creates and updates archives from files, directories or streams |
| `SharpSevenZipSfx` | Builds self-extracting archives |

`LzmaEncodeStream` and `LzmaDecodeStream` are fully managed `Stream` implementations for raw LZMA data and need no native library.

### Extracting

```csharp
using SharpSevenZip;

using var extractor = new SharpSevenZipExtractor(@"C:\archive.7z");

foreach (var entry in extractor.ArchiveFileData)
{
    Console.WriteLine($"{entry.FileName} ({entry.Size} bytes)");
}

extractor.ExtractArchive(@"C:\output");
```

The input format is detected from the archive signature, so every format in `InArchiveFormat` — 7z, zip, rar, cab, iso and many more — can be read. Extraction from SFX archives and other formats with embedded archives is supported as well.

### Compressing

```csharp
using SharpSevenZip;

var compressor = new SharpSevenZipCompressor
{
    ArchiveFormat = OutArchiveFormat.SevenZip,
    CompressionLevel = CompressionLevel.High
};

compressor.CompressDirectory(@"C:\input", @"C:\archive.7z");
```

Output is limited to the formats 7-Zip can write: `SevenZip`, `Zip`, `Tar`, `GZip`, `BZip2` and `XZ`. GZip and BZip2 compress a single file at a time.

## Native library

A native 7-Zip library is required at runtime. By default SharpSevenZip loads `<directory of SharpSevenZip.dll>\x86\7z.dll` or `…\x64\7z.dll`, matching the process architecture.

Another location can be configured in `app.config` or at runtime:

```csharp
SharpSevenZipBase.SetLibraryPath(@"C:\Program Files\7-Zip\7z.dll");
```

`7z.dll` covers all archive operations. `7za.dll` is a smaller variant restricted to 7z archives, and a custom build of the 7-Zip sources containing only the formats you need works too.

## Features

- All 7-Zip archive formats for reading, all writable formats for compression
- Encryption, passwords and encrypted headers
- Archive properties, archive updates and multi-volume archives
- Streaming, multi-threading, configurable compression level and method
- Self-extracting archives

## Self-extracting archives

`SharpSevenZipSfx` uses the SFX module by Oleg Scherbakov, which is embedded in the assembly.

Alternative modules are available in `SharpSevenZip/sfx`. Combined with `SfxSettings` scenarios this can produce small installers; the available directives are listed under "configuration file parameters" of the SFX module.

## Custom compression switches

`SharpSevenZipCompressor.CustomParameters` accepts the switches of the `7z.exe` command line, for example to enable multi-threaded compression:

```csharp
compressor.CustomParameters.Add("mt", "on");
```

The complete list of switches is documented in `7-zip.chm` and `SevenZipDoc.chm` of a 7-Zip installation.

## Strong naming

Two packages are published from the same sources:

| Package | Assembly identity |
| --- | --- |
| `SharpSevenZip` | not strong-named |
| `SharpSevenZip.StrongName` | strong-named, public key token `7b9e68449741c4c5` |

Use `SharpSevenZip.StrongName` when your own application or library is strong-named and therefore requires strong-named references. Both packages contain an assembly named `SharpSevenZip`, so a project can reference only one of them.

The key pair used for signing, `SharpSevenZip/SharpSevenZip/SharpSevenZip.snk`, is part of the repository. Anyone can build an assembly with that identity; strong naming provides an identity, not a security guarantee.

## Benchmarks

Decompression compared against `System.IO.Compression`, [SharpCompress](https://github.com/adamhathcock/sharpcompress) and [SevenZipSharp](https://github.com/squid-box/SevenZipSharp). `Empty` decompresses an empty archive, `Sum1` a small payload; the `stream` variants read from a `Stream` instead of a file.

| Scenario | Library | Mean (net48) | Alloc (net48) | Mean (net8.0) | Alloc (net8.0) |
| --- | --- | ---: | ---: | ---: | ---: |
| Empty | System.IO.Compression | 1.077 ms | 56 KB | 2.990 ms | 51.3 KB |
| Empty | SharpCompress | 2.902 ms | 152 KB | 7.448 ms | 131.8 KB |
| Empty | SevenZipSharp | 7.412 ms | 1440.1 KB | 14.186 ms | 1437.2 KB |
| Empty | **SharpSevenZip** | 2.238 ms | 72 KB | 7.723 ms | 83.7 KB |
| Sum1 | System.IO.Compression | 5.875 ms | 64 KB | 8.799 ms | 59.8 KB |
| Sum1 | SharpCompress | 5.156 ms | 824.7 KB | 9.976 ms | 797.8 KB |
| Sum1 | SharpCompress (stream) | 8.936 ms | 816.7 KB | 12.757 ms | 797.0 KB |
| Sum1 | SevenZipSharp | 24.214 ms | 28333.2 KB | 32.895 ms | 28359.7 KB |
| Sum1 | **SharpSevenZip** | 9.725 ms | 112 KB | 19.595 ms | 123.6 KB |
| Sum1 | **SharpSevenZip (stream)** | 34.122 ms | 171.6 KB | 18.431 ms | 129.2 KB |

The benchmark project is `SharpSevenZipBenchmarks`.

## History

SharpSevenZip is a fork of [Squid-Box.SevenZipSharp](https://github.com/squid-box/SevenZipSharp), which forked [tomap's fork](https://github.com/tomap/SevenZipSharp) of the [original CodePlex project](https://archive.codeplex.com/?p=sevenzipsharp) by Markovtsev Vadim.

As required by the license, the notable changes since the CodePlex project, including those made in tomap's fork, are:

- Target frameworks moved from .NET Framework 2.0 to .NET Standard 2.0, .NET Framework 4.8 and .NET 8.0.
- Continuous integration added for both building and publishing.
- Tests rewritten as NUnit 3 test cases.
- General code cleanup, along with a number of improvements and bug fixes.

## License

Licensed under the [GNU Lesser General Public License v3.0 or later](https://github.com/JeremyAnsel/SharpSevenZip/blob/main/LICENSE).
