
SharpSevenZip
======

SharpSevenZip is a 7-zip native library wrapper. Managed 7-zip library written in C# that provides data (self-)extraction and compression (all 7-zip formats are supported). Wraps 7z.dll or any compatible one and makes use of LZMA SDK, includes self-extraction functionality.

This is a fork from [Squid-Box.SevenZipSharp](https://github.com/squid-box/SevenZipSharp) which is a fork from [tomap's fork](https://github.com/tomap/SevenZipSharp) of the [original CodePlex project](https://archive.codeplex.com/?p=sevenzipsharp).

SevenZipSharp is an open source wrapper for 7-zip. It exploits the native 7zip dynamic link library through its COM interface and exports classes to work with various file archives. The project appeared as an improvement of http://www.codeproject.com/KB/DLL/cs_interface_7zip.aspx.


Build Status
------------

[![Build status](https://ci.appveyor.com/api/projects/status/u6ki6smclwffstjy/branch/main?svg=true)](https://ci.appveyor.com/project/JeremyAnsel/sharpsevenzip/branch/main)
[![NuGet Version](https://img.shields.io/nuget/v/SharpSevenZip)](https://www.nuget.org/packages/SharpSevenZip)
![License](https://img.shields.io/github/license/JeremyAnsel/SharpSevenZip)


Description     | Value
----------------|----------------
License         | https://github.com/JeremyAnsel/SharpSevenZip/blob/main/LICENSE
Documentation   | http://jeremyansel.github.io/SharpSevenZip
Source code     | https://github.com/JeremyAnsel/SharpSevenZip
Nuget           | https://www.nuget.org/packages/SharpSevenZip
Build           | https://ci.appveyor.com/project/JeremyAnsel/sharpsevenzip/branch/main


Changes from original project
------------

As required by the GNU GPL 3.0 license, here's a rough list of what has changed since the original CodePlex project, including changes made in tomap's fork.

- Target .NET version changed from .NET Framework 2.0 to .NET Standard 2.0, .NET Framework 4.8, .NET 6.0 and .NET 8.0.
- Continous Integration added, both building and deploying.
- Tests re-written to NUnit 3 test cases.
- General code cleanup.

As well as a number of improvements and bug fixes.


Quick start
------------

SharpSevenZip exports three main classes - SharpSevenZipExtractor, SharpSevenZipCompressor and SharpSevenZipSfx.
SharpSevenZipExtractor is a 7-zip unpacking front-end, it allows either to extract archives or LZMA-compressed byte arrays.
SharpSevenZipCompressor is a 7-zip pack ingfront-end, it allows either to compress archives or byte arrays.
SharpSevenZipSfx is a special class to create self-extracting archives. It uses the embedded sfx module by Oleg Scherbakov .
LzmaEncodeStream/LzmaDecodeStream are special fully managed classes derived from Stream to store data compressed with LZMA and extract it.


Native libraries
------------

SharpSevenZip requires a 7-zip native library to function. You can specify the path to a 7-zip dll (7z.dll, 7za.dll, etc.) in LibraryManager.cs at compile time, your app.config or via SetLibraryPath() method at runtime. <Path to SharpSevenZip.dll> + ("x86" or "x64") + "7z.dll" is the default path. For 64-bit systems, you must use the 64-bit versions of those libraries.
7-zip ships with 7z.dll, which is used for all archive operations (usually it is "Program Files\7-Zip\7z.dll"). 7za.dll is a light version of 7z.dll, it supports only 7zip archives. You may even build your own library with formats you want from 7-zip sources. SharpSevenZip will work with them all.


Main features
------------

- Encryption and passwords are supported.
- Archive properties are supported.
- Multi-threading is supported.
- Streaming is supported.
- Setting the compression level and method is supported.
- Archive volumes are supported.
- Archive updates are supported.
- Extraction from SFX archives, as well as some other formats with embedded archives is supported.

Extraction is supported from any archive format in InArchiveFormat - such as 7-zip itself, zip, rar or cab and the format is automatically guessed by the archive signature.
You can compress streams, files or whole directories in OutArchiveFormat - 7-zip, Xz, Zip, GZip, BZip2 and Tar.
Please note that GZip and BZip2 compresses only one file at a time.


Self-extracting archives
------------
SharpSevenZipSfx supports custom sfx modules. The most powerful one is embedded in the assembly, the other lie in SharpSevenZip/sfx directory. Apart from usual sfx, you can make even small installations with the help of SfxSettings scenarios. Refer to the "configuration file parameters" for the complete command list.


Advanced work with SharpSevenZipCompressor
------------

SharpSevenZipCompressor.CustomParameters is a special property to set compression switches, compatible with command line switches of 7z.exe. The complete list of those switches is in 7-zip.chm of 7-Zip installation. For example, to turn on multi-threaded compression, code
<SharpSevenZipCompressor Instance>.CustomParameters.Add("mt", "on");
For the complete switches list, refer to SevenZipDoc.chm in the 7-zip installation.


Benchmarks
------------

Here is benchmarks to compare the performance of this library.
The benchmarks use these libraries:
- .Net Framework with System.IO.Compression
- SharpCompress
- SevenZipSharp
- SharpSevenZip

| Method                               | Job   | Mean         | Error | Ratio | Allocated   | Alloc Ratio |
|------------------------------------- |------ |-------------:|------:|------:|------------:|------------:|
| Decompress_DotNetFramework_Empty     | Net80 |   2,506.4 탎 |    NA |  3.62 |     51.3 KB |        0.92 |
| Decompress_DotNetFramework_Empty     | Net60 |   2,153.1 탎 |    NA |  3.11 |    51.55 KB |        0.92 |
| Decompress_DotNetFramework_Empty     | Net48 |     691.6 탎 |    NA |  1.00 |       56 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SharpCompress_Empty       | Net60 |   5,246.6 탎 |    NA |  2.38 |    132.4 KB |        0.87 |
| Decompress_SharpCompress_Empty       | Net80 |   4,402.2 탎 |    NA |  2.00 |   131.76 KB |        0.87 |
| Decompress_SharpCompress_Empty       | Net48 |   2,201.4 탎 |    NA |  1.00 |      152 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SevenZipSharp_Empty       | Net60 |  12,238.4 탎 |    NA |  1.49 |  1437.94 KB |        1.00 |
| Decompress_SevenZipSharp_Empty       | Net80 |  11,731.7 탎 |    NA |  1.42 |  1437.23 KB |        1.00 |
| Decompress_SevenZipSharp_Empty       | Net48 |   8,236.1 탎 |    NA |  1.00 |  1440.08 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SharpSevenZip_Empty       | Net80 |   5,164.0 탎 |    NA |  2.59 |    72.64 KB |        1.01 |
| Decompress_SharpSevenZip_Empty       | Net60 |   3,822.6 탎 |    NA |  1.91 |    73.34 KB |        1.02 |
| Decompress_SharpSevenZip_Empty       | Net48 |   1,996.8 탎 |    NA |  1.00 |       72 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_DotNetFramework_Sum1      | Net60 |   7,581.0 탎 |    NA |  1.54 |    59.84 KB |        0.94 |
| Decompress_DotNetFramework_Sum1      | Net80 |   7,033.6 탎 |    NA |  1.43 |    59.75 KB |        0.93 |
| Decompress_DotNetFramework_Sum1      | Net48 |   4,921.8 탎 |    NA |  1.00 |       64 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SharpCompress_Sum1        | Net80 |  10,215.2 탎 |    NA |  1.84 |   797.77 KB |        0.97 |
| Decompress_SharpCompress_Sum1        | Net60 |   6,530.2 탎 |    NA |  1.18 |   800.67 KB |        0.97 |
| Decompress_SharpCompress_Sum1        | Net48 |   5,539.4 탎 |    NA |  1.00 |    824.7 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SevenZipSharp_Sum1        | Net80 |  30,720.4 탎 |    NA |  1.29 | 28370.96 KB |        1.00 |
| Decompress_SevenZipSharp_Sum1        | Net60 |  28,560.4 탎 |    NA |  1.20 | 28356.42 KB |        1.00 |
| Decompress_SevenZipSharp_Sum1        | Net48 |  23,728.1 탎 |    NA |  1.00 | 28333.16 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SharpSevenZip_Sum1        | Net80 |  13,632.1 탎 |    NA |  1.33 |   105.06 KB |        0.94 |
| Decompress_SharpSevenZip_Sum1        | Net60 |  13,624.2 탎 |    NA |  1.33 |   104.67 KB |        0.93 |
| Decompress_SharpSevenZip_Sum1        | Net48 |  10,225.2 탎 |    NA |  1.00 |      112 KB |        1.00 |
|                                      |       |              |       |       |             |             |
| Decompress_SharpSevenZip_Sum1_Stream | Net48 | 153,302.2 탎 |    NA |  1.00 |      176 KB |        1.00 |
| Decompress_SharpSevenZip_Sum1_Stream | Net60 | 151,244.0 탎 |    NA |  0.99 |    111.2 KB |        0.63 |
| Decompress_SharpSevenZip_Sum1_Stream | Net80 | 148,351.9 탎 |    NA |  0.97 |   111.59 KB |        0.63 |
