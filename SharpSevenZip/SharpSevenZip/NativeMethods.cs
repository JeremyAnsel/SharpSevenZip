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

#if NET8_0_OR_GREATER
    [LibraryImport("kernel32.dll", SetLastError = true)]
    [return:MarshalAs(UnmanagedType.Bool)]
    private static partial bool QueryThreadCycleTime(IntPtr hThread, out ulong cycles);

    private static readonly IntPtr PseudoHandle = (IntPtr)(-2);

    [LibraryImport("kernel32.dll", EntryPoint = "LoadLibraryA")]
    public static partial IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);

    [LibraryImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FreeLibrary(IntPtr hModule);

    [LibraryImport("kernel32.dll")]
    public static partial IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);
#else
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool QueryThreadCycleTime(IntPtr hThread, out ulong cycles);

    private static readonly IntPtr PseudoHandle = (IntPtr)(-2);

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate int CreateObjectDelegate(
        [In] ref Guid classID,
        [In] ref Guid interfaceID,
        [MarshalAs(UnmanagedType.Interface)] out object outObject);

    [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool FreeLibrary(IntPtr hModule);

    [DllImport("kernel32.dll", BestFitMapping = false, ThrowOnUnmappableChar = true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)] string procName);
#endif

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
