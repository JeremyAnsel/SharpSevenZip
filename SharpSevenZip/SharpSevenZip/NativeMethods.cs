using System.Runtime.InteropServices;
using System.Security;

namespace SharpSevenZip;

[SecurityCritical, SuppressUnmanagedCodeSecurity]
internal static partial class NativeMethods
{
    public static ulong GetThreadCycles()
    {
        if (!QueryThreadCycleTime(PseudoHandle, out ulong cycles))
        {
            return ulong.MaxValue;
        }

        return cycles;
    }

    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static partial bool QueryThreadCycleTime(IntPtr hThread, out ulong cycles);

    private static readonly IntPtr PseudoHandle = (IntPtr)(-2);

    [LibraryImport("kernel32.dll")]
    public static partial IntPtr LoadLibraryA([MarshalAs(UnmanagedType.LPStr)] string fileName);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FreeLibrary(IntPtr hModule);

    [LibraryImport("kernel32.dll")]
    public static partial IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);

    public static T? SafeCast<T>(PropVariant var, T? def)
    {
        object? obj;

        try
        {
            obj = var.Object;
        }
        catch (Exception)
        {
            return def;
        }

        if (obj is T expected)
        {
            return expected;
        }

        return def;
    }
}
