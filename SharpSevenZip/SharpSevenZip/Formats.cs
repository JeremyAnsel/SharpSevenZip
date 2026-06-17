namespace SharpSevenZip;

/// <summary>
/// Readable archive format enumeration.
/// </summary>
public enum InArchiveFormat
{
    /// <summary>
    /// Open 7-zip archive format.
    /// </summary>  
    /// <remarks><a href="http://en.wikipedia.org/wiki/7-zip">Wikipedia information</a></remarks> 
    SevenZip,
    /// <summary>
    /// Proprietary Arj archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/ARJ">Wikipedia information</a></remarks>
    Arj,
    /// <summary>
    /// Open Bzip2 archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Bzip2">Wikipedia information</a></remarks>
    BZip2,
    /// <summary>
    /// Microsoft cabinet archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Cabinet_(file_format)">Wikipedia information</a></remarks>
    Cab,
    /// <summary>
    /// Microsoft Compiled HTML Help file format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Microsoft_Compiled_HTML_Help">Wikipedia information</a></remarks>
    Chm,
    /// <summary>
    /// Microsoft Compound file format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Compound_File_Binary_Format">Wikipedia information</a></remarks>
    Compound,
    /// <summary>
    /// Open Cpio archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Cpio">Wikipedia information</a></remarks>
    Cpio,
    /// <summary>
    /// Open Debian software package format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Deb_(file_format)">Wikipedia information</a></remarks>
    Deb,
    /// <summary>
    /// Open Gzip archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Gzip">Wikipedia information</a></remarks>
    GZip,
    /// <summary>
    /// Open ISO disk image format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/ISO_image">Wikipedia information</a></remarks>
    Iso,
    /// <summary>
    /// Open Lzh archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Lzh">Wikipedia information</a></remarks>
    Lzh,
    /// <summary>
    /// Open core 7-zip Lzma raw archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Lzma">Wikipedia information</a></remarks>
    Lzma,
    /// <summary>
    /// Nullsoft installation package format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/NSIS">Wikipedia information</a></remarks>
    Nsis,
    /// <summary>
    /// GUID Partition Table.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/GUID_Partition_Table">Wikipedia information</a></remarks>
    Gpt,
    /// <summary>
    /// RarLab Rar archive format, version 5.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Rar">Wikipedia information</a></remarks>
    Rar,
    /// <summary>
    /// RarLab Rar archive format, version 4 or older.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Rar">Wikipedia information</a></remarks>
    Rar4,
    /// <summary>
    /// Open Rpm software package format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/RPM_Package_Manager">Wikipedia information</a></remarks>
    Rpm,
    /// <summary>
    /// Open split file format.
    /// </summary>
    /// <remarks><a href="?">Wikipedia information</a></remarks>
    Split,
    /// <summary>
    /// Open Tar archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Tar_(file_format)">Wikipedia information</a></remarks>
    Tar,
    /// <summary>
    /// Microsoft Windows Imaging disk image format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Windows_Imaging_Format">Wikipedia information</a></remarks>
    Wim,
    /// <summary>
    /// Open LZW archive format; implemented in "compress" program; also known as "Z" archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Compress">Wikipedia information</a></remarks>
    Lzw,
    /// <summary>
    /// Open Zip archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/ZIP_(file_format)">Wikipedia information</a></remarks>
    Zip,
    /// <summary>
    /// Open Udf disk image format.
    /// </summary>
    Udf,
    /// <summary>
    /// Xar open source archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Xar_(archiver)">Wikipedia information</a></remarks>
    Xar,
    /// <summary>
    /// Mub
    /// </summary>
    Mub,
    /// <summary>
    /// Macintosh Disk Image on CD.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/HFS_Plus">Wikipedia information</a></remarks>
    Hfs,
    /// <summary>
    /// Apple Mac OS X Disk Copy Disk Image format.
    /// </summary>
    Dmg,
    /// <summary>
    /// Open Xz archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Xz">Wikipedia information</a></remarks>        
    XZ,
    /// <summary>
    /// MSLZ archive format.
    /// </summary>
    Mslz,
    /// <summary>
    /// Flash video format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Flv">Wikipedia information</a></remarks>
    Flv,
    /// <summary>
    /// Shockwave Flash format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Swf">Wikipedia information</a></remarks>         
    Swf,
    /// <summary>
    /// Windows PE executable format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Portable_Executable">Wikipedia information</a></remarks>
    PE,
    /// <summary>
    /// Linux executable Elf format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Executable_and_Linkable_Format">Wikipedia information</a></remarks>
    Elf,
    /// <summary>
    /// Windows Installer Database.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Windows_Installer">Wikipedia information</a></remarks>
    Msi,
    /// <summary>
    /// Microsoft virtual hard disk file format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/VHD_%28file_format%29">Wikipedia information</a></remarks>
    Vhd,
    /// <summary>
    /// SquashFS file system format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/SquashFS">Wikipedia information</a></remarks>
    SquashFS,
    /// <summary>
    /// Lzma86 file format.
    /// </summary>
    Lzma86,
    /// <summary>
    /// Prediction by Partial Matching by Dmitry algorithm.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Prediction_by_partial_matching">Wikipedia information</a></remarks>
    Ppmd,
    /// <summary>
    /// TE format.
    /// </summary>
    TE,
    /// <summary>
    /// UEFIc format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Unified_Extensible_Firmware_Interface">Wikipedia information</a></remarks>
    UEFIc,
    /// <summary>
    /// UEFIs format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Unified_Extensible_Firmware_Interface">Wikipedia information</a></remarks>
    UEFIs,
    /// <summary>
    /// Compressed ROM file system format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Cramfs">Wikipedia information</a></remarks>
    CramFS,
    /// <summary>
    /// APM format.
    /// </summary>
    APM,
    /// <summary>
    /// Swfc format.
    /// </summary>
    Swfc,
    /// <summary>
    /// NTFS file system format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/NTFS">Wikipedia information</a></remarks>
    Ntfs,
    /// <summary>
    /// FAT file system format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/File_Allocation_Table">Wikipedia information</a></remarks>
    Fat,
    /// <summary>
    /// MBR format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Master_boot_record">Wikipedia information</a></remarks>
    Mbr,
    /// <summary>
    /// Mach-O file format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Mach-O">Wikipedia information</a></remarks>
    MachO,
    /// <summary>
    /// Apple File System Format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Apple_File_System">Wikipedia information</a></remarks>
    Apfs,

    /// <summary>
    /// Linux ext2/ext3/ext4 file system format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Extended_file_system">Wikipedia information</a></remarks>
    Ext,
    /// <summary>
    /// Zstandard compressed data format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Zstandard">Wikipedia information</a></remarks>
    Zstd,
    /// <summary>
    /// Microsoft Virtual Hard Disk v2 format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Virtual_Hard_Disk">Wikipedia information</a></remarks>
    Vhdx,
    /// <summary>
    /// VirtualBox Disk Image format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/VirtualBox">Wikipedia information</a></remarks>
    Vdi,
    /// <summary>
    /// VMware virtual disk format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Virtual_Machine_Disk">Wikipedia information</a></remarks>
    Vmdk,
    /// <summary>
    /// QEMU copy-on-write disk image format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Copy-on-write">Wikipedia information</a></remarks>
    QCow,
    /// <summary>
    /// Intel Hex encoded format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Intel_HEX">Wikipedia information</a></remarks>
    IHex,
    /// <summary>
    /// Microsoft Help 2.x Collection format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Microsoft_Compiled_HTML_Help">Wikipedia information</a></remarks>
    Hxs,
    /// <summary>
    /// Android logical partition image format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Logical_Volume_Manager_(Linux)">Wikipedia information</a></remarks>
    Lp,
    /// <summary>
    /// Android sparse image format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Sparse_image">Wikipedia information</a></remarks>
    Sparse,
    /// <summary>
    /// COFF object file format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/COFF">Wikipedia information</a></remarks>
    Coff,
    /// <summary>
    /// Base64 encoded format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Base64">Wikipedia information</a></remarks>
    Base64,
    /// <summary>
    /// Android Verified Boot image format.
    /// </summary>
    /// <remarks><a href="https://source.android.com/docs/security/features/verifiedboot">Android documentation</a></remarks>
    Avb,
    /// <summary>
    /// Linux Logical Volume Manager format.
    /// </summary>
    /// <remarks><a href="https://en.wikipedia.org/wiki/Logical_Volume_Manager_(Linux)">Wikipedia information</a></remarks>
    Lvm,
}

/// <summary>
/// Writable archive format enumeration.
/// </summary>    
public enum OutArchiveFormat
{
    /// <summary>
    /// Open 7-zip archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/7-zip">Wikipedia information</a></remarks>
    SevenZip,
    /// <summary>
    /// Open Zip archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/ZIP_(file_format)">Wikipedia information</a></remarks>
    Zip,
    /// <summary>
    /// Open Gzip archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Gzip">Wikipedia information</a></remarks>
    GZip,
    /// <summary>       
    /// Open Bzip2 archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Bzip2">Wikipedia information</a></remarks>
    BZip2,
    /// <summary>
    /// Microsoft Windows Imaging disk image format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Windows_Imaging_Format">Wikipedia information</a></remarks>
    Wim,
    /// <summary>
    /// Open Tar archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Tar_(file_format)">Wikipedia information</a></remarks>
    Tar,
    /// <summary>
    /// Open Xz archive format.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Xz">Wikipedia information</a></remarks>        
    XZ
}

/// <summary>
/// Compression level enumeration
/// </summary>
public enum CompressionLevel
{
    /// <summary>
    /// No compression
    /// </summary>
    None,
    /// <summary>
    /// Very low compression level
    /// </summary>
    Fast,
    /// <summary>
    /// Low compression level
    /// </summary>
    Low,
    /// <summary>
    /// Normal compression level (default)
    /// </summary>
    Normal,
    /// <summary>
    /// High compression level
    /// </summary>
    High,
    /// <summary>
    /// The best compression level (slow)
    /// </summary>
    Ultra
}

/// <summary>
/// Compression method enumeration.
/// </summary>
/// <remarks>Some methods are applicable only to Zip format, some - only to 7-zip.</remarks>
public enum CompressionMethod
{
    /// <summary>
    /// Zip or 7-zip|no compression method.
    /// </summary>
    Copy,
    /// <summary>
    /// Zip|Deflate method.
    /// </summary>
    Deflate,
    /// <summary>
    /// Zip|Deflate64 method.
    /// </summary>
    Deflate64,
    /// <summary>
    /// Zip or 7-zip|Bzip2 method.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Cabinet_(file_format)">Wikipedia information</a></remarks>
    BZip2,
    /// <summary>
    /// Zip or 7-zip|LZMA method based on Lempel-Ziv algorithm, it is default for 7-zip.
    /// </summary>
    Lzma,
    /// <summary>
    /// 7-zip|LZMA version 2, LZMA with improved multithreading and usually slight archive size decrease.
    /// </summary>
    Lzma2,
    /// <summary>
    /// Zip or 7-zip|PPMd method based on Dmitry Shkarin's PPMdH source code, very efficient for compressing texts.
    /// </summary>
    /// <remarks><a href="http://en.wikipedia.org/wiki/Prediction_by_Partial_Matching">Wikipedia information</a></remarks>
    Ppmd,
    /// <summary>
    /// No method change.
    /// </summary>
    Default
}

/// <summary>
/// Archive format routines
/// </summary>
public static class Formats
{
    /*/// <summary>
    /// Gets the max value of the specified enum type.
    /// </summary>
    /// <param name="type">Type of the enum</param>
    /// <returns>Max value</returns>
    internal static int GetMaxValue(Type type)
    {
        List<int> enumList = new List<int>((IEnumerable<int>)Enum.GetValues(type));
        enumList.Sort();
        return enumList[enumList.Count - 1];
    }*/

    /// <summary>
    /// List of readable archive format interface guids for 7-zip COM interop.
    /// </summary>
    internal static readonly Dictionary<InArchiveFormat, Guid> InFormatGuids =
        new()
        #region InFormatGuids initialization

        {
                {InArchiveFormat.SevenZip,  new Guid("23170f69-40c1-278a-1000-000110070000")},
                {InArchiveFormat.Arj,       new Guid("23170f69-40c1-278a-1000-000110040000")},
                {InArchiveFormat.BZip2,     new Guid("23170f69-40c1-278a-1000-000110020000")},
                {InArchiveFormat.Cab,       new Guid("23170f69-40c1-278a-1000-000110080000")},
                {InArchiveFormat.Chm,       new Guid("23170f69-40c1-278a-1000-000110e90000")},
                {InArchiveFormat.Compound,  new Guid("23170f69-40c1-278a-1000-000110e50000")},
                {InArchiveFormat.Cpio,      new Guid("23170f69-40c1-278a-1000-000110ed0000")},
                {InArchiveFormat.Deb,       new Guid("23170f69-40c1-278a-1000-000110ec0000")},
                {InArchiveFormat.GZip,      new Guid("23170f69-40c1-278a-1000-000110ef0000")},
                {InArchiveFormat.Iso,       new Guid("23170f69-40c1-278a-1000-000110e70000")},
                {InArchiveFormat.Lzh,       new Guid("23170f69-40c1-278a-1000-000110060000")},
                {InArchiveFormat.Lzma,      new Guid("23170f69-40c1-278a-1000-0001100a0000")},
                {InArchiveFormat.Nsis,      new Guid("23170f69-40c1-278a-1000-000110090000")},
                {InArchiveFormat.Gpt,       new Guid("23170f69-40c1-278a-1000-000110cb0000")},
                {InArchiveFormat.Rar,       new Guid("23170f69-40c1-278a-1000-000110CC0000")},
                {InArchiveFormat.Rar4,      new Guid("23170f69-40c1-278a-1000-000110030000")},
                {InArchiveFormat.Rpm,       new Guid("23170f69-40c1-278a-1000-000110eb0000")},
                {InArchiveFormat.Split,     new Guid("23170f69-40c1-278a-1000-000110ea0000")},
                {InArchiveFormat.Tar,       new Guid("23170f69-40c1-278a-1000-000110ee0000")},
                {InArchiveFormat.Wim,       new Guid("23170f69-40c1-278a-1000-000110e60000")},
                {InArchiveFormat.Lzw,       new Guid("23170f69-40c1-278a-1000-000110050000")},
                {InArchiveFormat.Zip,       new Guid("23170f69-40c1-278a-1000-000110010000")},
                {InArchiveFormat.Udf,       new Guid("23170f69-40c1-278a-1000-000110E00000")},
                {InArchiveFormat.Xar,       new Guid("23170f69-40c1-278a-1000-000110E10000")},
                {InArchiveFormat.Mub,       new Guid("23170f69-40c1-278a-1000-000110E20000")},
                {InArchiveFormat.Hfs,       new Guid("23170f69-40c1-278a-1000-000110E30000")},
                {InArchiveFormat.Dmg,       new Guid("23170f69-40c1-278a-1000-000110E40000")},
                {InArchiveFormat.XZ,        new Guid("23170f69-40c1-278a-1000-0001100C0000")},
                {InArchiveFormat.Mslz,      new Guid("23170f69-40c1-278a-1000-000110D50000")},
                {InArchiveFormat.PE,        new Guid("23170f69-40c1-278a-1000-000110DD0000")},
                {InArchiveFormat.Elf,       new Guid("23170f69-40c1-278a-1000-000110DE0000")},
                {InArchiveFormat.Swf,       new Guid("23170f69-40c1-278a-1000-000110D70000")},
                {InArchiveFormat.Vhd,       new Guid("23170f69-40c1-278a-1000-000110DC0000")},
                {InArchiveFormat.Flv,       new Guid("23170f69-40c1-278a-1000-000110D60000")},
                {InArchiveFormat.SquashFS,  new Guid("23170f69-40c1-278a-1000-000110D20000")},
                {InArchiveFormat.Lzma86,    new Guid("23170f69-40c1-278a-1000-0001100B0000")},
                {InArchiveFormat.Ppmd,      new Guid("23170f69-40c1-278a-1000-0001100D0000")},
                {InArchiveFormat.TE,        new Guid("23170f69-40c1-278a-1000-000110CF0000")},
                {InArchiveFormat.UEFIc,     new Guid("23170f69-40c1-278a-1000-000110D00000")},
                {InArchiveFormat.UEFIs,     new Guid("23170f69-40c1-278a-1000-000110D10000")},
                {InArchiveFormat.CramFS,    new Guid("23170f69-40c1-278a-1000-000110D30000")},
                {InArchiveFormat.APM,       new Guid("23170f69-40c1-278a-1000-000110D40000")},
                {InArchiveFormat.Swfc,      new Guid("23170f69-40c1-278a-1000-000110D80000")},
                {InArchiveFormat.Ntfs,      new Guid("23170f69-40c1-278a-1000-000110D90000")},
                {InArchiveFormat.Fat,       new Guid("23170f69-40c1-278a-1000-000110DA0000")},
                {InArchiveFormat.Mbr,       new Guid("23170f69-40c1-278a-1000-000110DB0000")},
                {InArchiveFormat.MachO,     new Guid("23170f69-40c1-278a-1000-000110DF0000")},
                {InArchiveFormat.Apfs,      new Guid("23170f69-40c1-278a-1000-000110C30000")},
                {InArchiveFormat.Ext,       new Guid("23170f69-40c1-278a-1000-000110C70000")},
                {InArchiveFormat.Zstd,      new Guid("23170f69-40c1-278a-1000-0001100E0000")},
                {InArchiveFormat.Vhdx,      new Guid("23170f69-40c1-278a-1000-000110C40000")},
                {InArchiveFormat.Vdi,       new Guid("23170f69-40c1-278a-1000-000110C90000")},
                {InArchiveFormat.Vmdk,      new Guid("23170f69-40c1-278a-1000-000110C80000")},
                {InArchiveFormat.QCow,      new Guid("23170f69-40c1-278a-1000-000110CA0000")},
                {InArchiveFormat.IHex,      new Guid("23170f69-40c1-278a-1000-000110CD0000")},
                {InArchiveFormat.Hxs,       new Guid("23170f69-40c1-278a-1000-000110CE0000")},
                {InArchiveFormat.Lp,        new Guid("23170f69-40c1-278a-1000-000110C10000")},
                {InArchiveFormat.Sparse,    new Guid("23170f69-40c1-278a-1000-000110C20000")},
                {InArchiveFormat.Coff,      new Guid("23170f69-40c1-278a-1000-000110C60000")},
                {InArchiveFormat.Base64,    new Guid("23170f69-40c1-278a-1000-000110C50000")},
                {InArchiveFormat.Msi,       new Guid("23170f69-40c1-278a-1000-000110e50000")},
                {InArchiveFormat.Avb,       new Guid("23170f69-40c1-278a-1000-000110C00000")},
                {InArchiveFormat.Lvm,       new Guid("23170f69-40c1-278a-1000-000110BF0000")},
        };

    #endregion

    /// <summary>
    /// List of writable archive format interface guids for 7-zip COM interop.
    /// </summary>
    internal static readonly Dictionary<OutArchiveFormat, Guid> OutFormatGuids =
        new()
        #region OutFormatGuids initialization

        {
                {OutArchiveFormat.SevenZip,     new Guid("23170f69-40c1-278a-1000-000110070000")},
                {OutArchiveFormat.Zip,          new Guid("23170f69-40c1-278a-1000-000110010000")},
                {OutArchiveFormat.BZip2,        new Guid("23170f69-40c1-278a-1000-000110020000")},
                {OutArchiveFormat.Wim,          new Guid("23170f69-40c1-278a-1000-000110E60000")},
                {OutArchiveFormat.GZip,         new Guid("23170f69-40c1-278a-1000-000110ef0000")},
                {OutArchiveFormat.Tar,          new Guid("23170f69-40c1-278a-1000-000110ee0000")},
                {OutArchiveFormat.XZ,           new Guid("23170f69-40c1-278a-1000-0001100C0000")}
        };

    #endregion

    internal static readonly Dictionary<CompressionMethod, string> MethodNames =
        new()
        #region MethodNames initialization

        {
                {CompressionMethod.Copy, "Copy"},
                {CompressionMethod.Deflate, "Deflate"},
                {CompressionMethod.Deflate64, "Deflate64"},
                {CompressionMethod.Lzma, "LZMA"},
                {CompressionMethod.Lzma2, "LZMA2"},
                {CompressionMethod.Ppmd, "PPMd"},
                {CompressionMethod.BZip2, "BZip2"}
        };

    #endregion

    internal static readonly Dictionary<OutArchiveFormat, InArchiveFormat> InForOutFormats =
        new()
        #region InForOutFormats initialization

        {
                {OutArchiveFormat.SevenZip, InArchiveFormat.SevenZip},
                {OutArchiveFormat.GZip, InArchiveFormat.GZip},
                {OutArchiveFormat.BZip2, InArchiveFormat.BZip2},
                {OutArchiveFormat.Wim, InArchiveFormat.Wim},
                {OutArchiveFormat.Tar, InArchiveFormat.Tar},
                {OutArchiveFormat.XZ, InArchiveFormat.XZ},
                {OutArchiveFormat.Zip, InArchiveFormat.Zip}
        };

    #endregion

    /// <summary>
    /// List of archive formats corresponding to specific extensions
    /// </summary>
    private static readonly Dictionary<string, InArchiveFormat> InExtensionFormats =
        new()
        #region InExtensionFormats initialization

        {
             {"7z",     InArchiveFormat.SevenZip},
             {"gz",     InArchiveFormat.GZip},
             {"tar",    InArchiveFormat.Tar},
             {"rar",    InArchiveFormat.Rar},
             {"zip",    InArchiveFormat.Zip},
             {"lzma",   InArchiveFormat.Lzma},
             {"lzh",    InArchiveFormat.Lzh},
             {"arj",    InArchiveFormat.Arj},
             {"bz2",    InArchiveFormat.BZip2},
             {"cab",    InArchiveFormat.Cab},
             {"chm",    InArchiveFormat.Chm},
             {"deb",    InArchiveFormat.Deb},
             {"iso",    InArchiveFormat.Iso},
             {"rpm",    InArchiveFormat.Rpm},
             {"wim",    InArchiveFormat.Wim},
             {"udf",    InArchiveFormat.Udf},
             {"mub",    InArchiveFormat.Mub},
             {"xar",    InArchiveFormat.Xar},
             {"hfs",    InArchiveFormat.Hfs},
             {"dmg",    InArchiveFormat.Dmg},
             {"Z",      InArchiveFormat.Lzw},
             {"xz",     InArchiveFormat.XZ},
             {"flv",    InArchiveFormat.Flv},
             {"swf",    InArchiveFormat.Swf},
             {"exe",    InArchiveFormat.PE},
             {"dll",    InArchiveFormat.PE},
             {"vhd",    InArchiveFormat.Vhd},
             {"gpt",    InArchiveFormat.Gpt },
             {"mbr",    InArchiveFormat.Mbr },
             {"ext",    InArchiveFormat.Ext },
             {"ntfs",   InArchiveFormat.Ntfs },
             {"apfs",   InArchiveFormat.Apfs },
             {"fat",    InArchiveFormat.Fat },
             // Zstandard
             {"zst",    InArchiveFormat.Zstd },
             {"tzst",   InArchiveFormat.Zstd },
             // Virtual disk images
             {"vhdx",   InArchiveFormat.Vhdx },
             {"avhdx",  InArchiveFormat.Vhdx },
             {"vdi",    InArchiveFormat.Vdi },
             {"vmdk",   InArchiveFormat.Vmdk },
             // QCOW
             {"qcow",   InArchiveFormat.QCow },
             {"qcow2",  InArchiveFormat.QCow },
             {"qcow2c", InArchiveFormat.QCow },
             // Misc
             {"ihex",   InArchiveFormat.IHex },
             {"hxs",    InArchiveFormat.Hxs },
             {"hxi",    InArchiveFormat.Hxs },
             {"hxr",    InArchiveFormat.Hxs },
             {"hxq",    InArchiveFormat.Hxs },
             {"hxw",    InArchiveFormat.Hxs },
             {"lit",    InArchiveFormat.Hxs },
             {"lpimg",  InArchiveFormat.Lp },
             {"simg",   InArchiveFormat.Sparse },
             {"b64",    InArchiveFormat.Base64 },
             {"obj",    InArchiveFormat.Coff },
             // Ar/deb - same 7-zip handler (format 0xEC)
             {"ar",     InArchiveFormat.Deb },
             {"a",      InArchiveFormat.Deb },
             // MSI uses the same 7-zip Compound handler (format 0xE5)
             {"msi",    InArchiveFormat.Msi },
             {"msp",    InArchiveFormat.Msi },
             {"msm",    InArchiveFormat.Msi },
             // Virtual machine disk images
             {"avb",    InArchiveFormat.Avb },
             {"lvm",    InArchiveFormat.Lvm },
    };

    #endregion

    /// <summary>
    /// List of archive formats corresponding to specific signatures
    /// </summary>
    /// <remarks>Based on the information at <a href="http://www.garykessler.net/library/file_sigs.html">this site.</a></remarks>
    internal static readonly Dictionary<string, InArchiveFormat> InSignatureFormats =
        new()
        #region InSignatureFormats initialization

        {
            {"37-7A-BC-AF-27-1C",                                              InArchiveFormat.SevenZip},
            {"1F-8B-08",                                                        InArchiveFormat.GZip},
            {"75-73-74-61-72",                                                  InArchiveFormat.Tar},
            //257 byte offset
            {"52-61-72-21-1A-07-00",                                            InArchiveFormat.Rar4},
            {"52-61-72-21-1A-07-01-00",                                         InArchiveFormat.Rar},
            {"50-4B-03-04",                                                     InArchiveFormat.Zip},
            {"50-4B-05-06",                                                     InArchiveFormat.Zip},
            {"5D-00-00-40-00",                                                  InArchiveFormat.Lzma},
            {"2D-6C-68",                                                        InArchiveFormat.Lzh},
            //^ 2 byte offset
            {"1F-9D",                                                            InArchiveFormat.Lzw},
            {"60-EA",                                                           InArchiveFormat.Arj},
            {"42-5A-68",                                                        InArchiveFormat.BZip2},
            {"4D-53-43-46",                                                     InArchiveFormat.Cab},
            {"49-54-53-46",                                                     InArchiveFormat.Chm},
            {"21-3C-61-72-63-68-3E-0A-64-65-62-69-61-6E-2D-62-69-6E-61-72-79",  InArchiveFormat.Deb},
            {"30-37-30-37-30",                                                  InArchiveFormat.Cpio},
            {"43-44-30-30-31",                                                  InArchiveFormat.Iso},
            //^ 0x8001, 0x8801 or 0x9001 byte offset
            {"ED-AB-EE-DB",                                                     InArchiveFormat.Rpm},
            {"4D-53-57-49-4D-00-00-00",                                         InArchiveFormat.Wim},
            // UDF: Beginning Extended Area Descriptor (BEA01) at offset 0x8000
            {"00-42-45-41-30-31-01-00",                                         InArchiveFormat.Udf},

            {"78-61-72-21",                                                     InArchiveFormat.Xar},
            //0x400 byte offset
            {"48-2B",                                                           InArchiveFormat.Hfs},
            {"FD-37-7A-58-5A-00",                                               InArchiveFormat.XZ},
            {"46-4C-56",                                                        InArchiveFormat.Flv},
            {"46-57-53",                                                        InArchiveFormat.Swf},
            {"43-57-53",                                                        InArchiveFormat.Swfc},
            {"5A-57-53",                                                        InArchiveFormat.Swfc},
            {"4D-5A",                                                           InArchiveFormat.PE},
            {"7F-45-4C-46",                                                     InArchiveFormat.Elf},
            {"78",                                                              InArchiveFormat.Dmg},
            {"63-6F-6E-65-63-74-69-78",                                         InArchiveFormat.Vhd},
            // Mub (Mach-O fat binary): 7-byte sig avoids Java .class false positive (CA-FE-BA-BE alone)
            {"CA-FE-BA-BE-00-00-00",                                            InArchiveFormat.Mub},
            {"B9-FA-F1-0E",                                                     InArchiveFormat.Mub},
            {"45-46-49-20-50-41-52-54-00-00-01-00",                             InArchiveFormat.Gpt},
            {"28-B5-2F-FD",                                                     InArchiveFormat.Zstd},
            {"76-68-64-78-66-69-6C-65",                                         InArchiveFormat.Vhdx},
            {"7F-10-DA-BE",                                                     InArchiveFormat.Vdi},
            {"4B-44-4D-56",                                                     InArchiveFormat.Vmdk},
            {"51-46-49-FB",                                                     InArchiveFormat.QCow},
            {"49-54-4F-4C-49-54-4C-53",                                         InArchiveFormat.Hxs},
            {"67-44-6C-61-34-00-00-00",                                         InArchiveFormat.Lp},
            {"3A-FF-26-ED-01-00",                                               InArchiveFormat.Sparse}};
    #endregion

    internal static Dictionary<InArchiveFormat, string> InSignatureFormatsReversed;

    static Formats()
    {
        InSignatureFormatsReversed = new Dictionary<InArchiveFormat, string>(InSignatureFormats.Count);

        foreach (var pair in InSignatureFormats)
        {
            if (InSignatureFormatsReversed.ContainsKey(pair.Value))
            {
                continue;
            }

            InSignatureFormatsReversed.Add(pair.Value, pair.Key);
        }
    }

    /// <summary>
    /// Gets InArchiveFormat for specified archive file name
    /// </summary>
    /// <param name="fileName">Archive file name</param>
    /// <param name="reportErrors">Indicates whether to throw exceptions</param>
    /// <returns>InArchiveFormat recognized by the file name extension</returns>
    /// <exception cref="System.ArgumentException"/>
    public static InArchiveFormat FormatByFileName(string fileName, bool reportErrors)
    {
        if (string.IsNullOrEmpty(fileName) && reportErrors)
        {
            throw new ArgumentException("File name is null or empty string!");
        }
        var rawExt = Path.GetExtension(fileName);
        string extension = rawExt.Length > 0 ? rawExt[1..] : "";

        if (!InExtensionFormats.ContainsKey(extension) && reportErrors)
        {
            throw new ArgumentException("Extension \"" + extension + "\" is not a supported archive file name extension.");
        }

        return InExtensionFormats[extension];
    }
}
