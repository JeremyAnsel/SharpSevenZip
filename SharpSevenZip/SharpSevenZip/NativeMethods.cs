using System.Runtime.InteropServices;
using System.Security;
using System.Text;

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

    public static IntPtr StringToHGlobalWChar(string? str)
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Marshal.StringToHGlobalUni(str);
        }
        else
        {
            // On non-Windows platforms, we can use UTF-32 encoding for wide characters.
            if (str == null)
            {
                return IntPtr.Zero;
            }

            int byteCount = (str.Length + 1) * sizeof(uint); // +1 for null terminator
            IntPtr ptr = Marshal.AllocHGlobal(byteCount);
            try
            {
                // Copy the string to the allocated memory as UTF-32
                Marshal.Copy(str.Select(c => (int)c).ToArray(), 0, ptr, str.Length);
                // Add null terminator
                Marshal.WriteInt32(ptr, str.Length * sizeof(uint), 0);
                return ptr;
            }
            catch
            {
                Marshal.FreeHGlobal(ptr);
                throw;
            }
        }
    }

    public static IntPtr StringToBSTR(string? str)
    {
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Marshal.StringToBSTR(str);
        }
        else
        {
            // On non-Windows platforms, we can use a simple UTF-32 encoding for BSTR.
            // Note: This is a simplified implementation and may not cover all edge cases of BSTR.
            if (str == null)
            {
                return IntPtr.Zero;
            }

            int byteCount = (str.Length + 1) * sizeof(uint) + sizeof(uint); // +1 for null terminator, *4 for UTF-32, +4 for additional length prefix
            IntPtr bstrPtr = Marshal.AllocHGlobal(byteCount);
            try
            {
                // Write the length prefix (in bytes)
                Marshal.WriteInt32(bstrPtr, str.Length * sizeof(uint));
                // Copy the string to the allocated memory as UTF-32
                Marshal.Copy(str.Select(c => (int)c).ToArray(), 0, bstrPtr + sizeof(uint), str.Length);
                // Add null terminator
                Marshal.WriteInt32(bstrPtr, str.Length * sizeof(uint) + sizeof(uint), 0);
                return bstrPtr;
            }
            catch
            {
                Marshal.FreeHGlobal(bstrPtr);
                throw;
            }
        }
    }

    public static string? PtrToStringBSTR(IntPtr bstr)
    {
        if (bstr == IntPtr.Zero)
        {
            return null;
        }

        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Marshal.PtrToStringBSTR(bstr);
        }
        else
        {
            // On non-Windows platforms, we can read the length prefix and then decode the UTF-32 string.
            int byteCount = Marshal.ReadInt32(bstr);
            if (byteCount == 0)
            {
                return string.Empty;
            }

            int charCount = byteCount / sizeof(uint);
            int[] charArray = new int[charCount];
            Marshal.Copy(bstr + sizeof(uint), charArray, 0, charCount);
            return new string(charArray.Select(c => (char)c).ToArray());
        }
    }
}
