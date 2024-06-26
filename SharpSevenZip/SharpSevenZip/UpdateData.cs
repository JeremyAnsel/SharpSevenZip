﻿namespace SharpSevenZip;

/// <summary>
/// Archive update data for UpdateCallback.
/// </summary>
internal struct UpdateData
{
    public uint FilesCount;
    public InternalCompressionMode Mode;

    public IDictionary<int, string?>? FileNamesToModify { get; set; }

    public List<ArchiveFileInfo>? ArchiveFileData { get; set; }
}
