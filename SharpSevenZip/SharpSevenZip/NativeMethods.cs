using System.Runtime.InteropServices;
using System.Security;

namespace SharpSevenZip;

[SecurityCritical, SuppressUnmanagedCodeSecurity]
internal static partial class NativeMethods
{
#if NET8_0_OR_GREATER
    public static IntPtr LoadLibrary(string fileName)
    {
        return NativeLibrary.Load(fileName);
    }

    public static bool FreeLibrary(IntPtr hModule)
    {
        NativeLibrary.Free(hModule);
        return true;
    }

    public static IntPtr GetProcAddress(IntPtr hModule, string procName)
    {
        return NativeLibrary.GetExport(hModule, procName);
    }
#else
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
