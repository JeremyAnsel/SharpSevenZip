
SharpSevenZip
======

SharpSevenZip is a 7-zip native library wrapper. Managed 7-zip library written in C# that provides data (self-)extraction and compression (all 7-zip formats are supported). Wraps 7z.dll or any compatible one and makes use of LZMA SDK, includes self-extraction functionality.

This is a fork from [Squid-Box.SevenZipSharp](https://github.com/squid-box/SevenZipSharp) which is a fork from [tomap's fork](https://github.com/tomap/SevenZipSharp) of the [original CodePlex project](https://archive.codeplex.com/?p=sevenzipsharp).

SevenZipSharp is an open source wrapper for 7-zip. It exploits the native 7zip dynamic link library through its COM interface and exports classes to work with various file archives. The project appeared as an improvement of http://www.codeproject.com/KB/DLL/cs_interface_7zip.aspx.


Build Status
------------

[![NuGet Version](https://buildstats.info/nuget/SharpSevenZip)](https://www.nuget.org/packages/SharpSevenZip)
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

SharpSevenZip exports three main classes - SharSevenZipExtractor, SharSevenZipCompressor and SharSevenZipSfx.
SharSevenZipExtractor is a 7-zip unpacking front-end, it allows either to extract archives or LZMA-compressed byte arrays.
SharSevenZipCompressor is a 7-zip pack ingfront-end, it allows either to compress archives or byte arrays.
SharSevenZipSfx is a special class to create self-extracting archives. It uses the embedded sfx module by Oleg Scherbakov .
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

| Method                           | Job   | Mean        | Error | Ratio | Allocated  | Alloc Ratio |
|--------------------------------- |------ |------------:|------:|------:|-----------:|------------:|
| Decompress_DotNetFramework_Empty | Net60 |   126.59 us |    NA |  3.33 |     5.1 KB |        1.01 |
| Decompress_DotNetFramework_Empty | Net80 |   120.61 us |    NA |  3.18 |    5.07 KB |        1.00 |
| Decompress_DotNetFramework_Empty | Net48 |    37.97 us |    NA |  1.00 |    5.05 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_SharpCompress_Empty   | Net60 |   304.80 us |    NA |  1.76 |    10.8 KB |        0.97 |
| Decompress_SharpCompress_Empty   | Net80 |   286.74 us |    NA |  1.66 |   10.66 KB |        0.96 |
| Decompress_SharpCompress_Empty   | Net48 |   172.78 us |    NA |  1.00 |   11.09 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_SevenZipSharp_Empty   | Net60 | 1,065.18 us |    NA |  1.61 |  143.78 KB |        1.00 |
| Decompress_SevenZipSharp_Empty   | Net80 | 1,028.80 us |    NA |  1.56 |   143.7 KB |        1.00 |
| Decompress_SevenZipSharp_Empty   | Net48 |   660.38 us |    NA |  1.00 |  144.01 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_SharpSevenZip_Empty   | Net60 | 1,244.93 us |    NA |  1.89 |   15.67 KB |        0.99 |
| Decompress_SharpSevenZip_Empty   | Net80 | 1,019.56 us |    NA |  1.55 |   15.59 KB |        0.99 |
| Decompress_SharpSevenZip_Empty   | Net48 |   659.21 us |    NA |  1.00 |    15.8 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_DotNetFramework_Sum1  | Net60 |   652.54 us |    NA |  1.36 |  1030.2 KB |        0.93 |
| Decompress_DotNetFramework_Sum1  | Net80 |   571.87 us |    NA |  1.19 | 1030.26 KB |        0.93 |
| Decompress_DotNetFramework_Sum1  | Net48 |   481.07 us |    NA |  1.00 | 1112.07 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_SharpCompress_Sum1    | Net60 | 1,351.73 us |    NA |  1.09 | 1037.99 KB |        0.93 |
| Decompress_SharpCompress_Sum1    | Net80 | 1,267.01 us |    NA |  1.02 |  1037.9 KB |        0.93 |
| Decompress_SharpCompress_Sum1    | Net48 | 1,244.25 us |    NA |  1.00 | 1120.07 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_SevenZipSharp_Sum1    | Net60 | 3,308.70 us |    NA |  1.35 |    3861 KB |        1.00 |
| Decompress_SevenZipSharp_Sum1    | Net80 | 3,130.96 us |    NA |  1.27 | 3862.07 KB |        1.00 |
| Decompress_SevenZipSharp_Sum1    | Net48 | 2,459.18 us |    NA |  1.00 | 3863.09 KB |        1.00 |
|                                  |       |             |       |       |            |             |
| Decompress_SharpSevenZip_Sum1    | Net60 | 2,586.79 us |    NA |  1.17 | 1040.06 KB |        0.50 |
| Decompress_SharpSevenZip_Sum1    | Net48 | 2,213.65 us |    NA |  1.00 | 2068.41 KB |        1.00 |
| Decompress_SharpSevenZip_Sum1    | Net80 | 2,063.06 us |    NA |  0.93 | 1040.16 KB |        0.50 |

