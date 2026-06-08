using System.Runtime.InteropServices;

namespace SharpSevenZip;

internal static class Marshal
{
    public unsafe static IntPtr StringToHGlobalWChar(string? str)
    {
#if !NET8_0_OR_GREATER
        return System.Runtime.InteropServices.Marshal.StringToHGlobalUni(str);
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return System.Runtime.InteropServices.Marshal.StringToHGlobalUni(str);
        }
        else
        {
            // On non-Windows platforms, we can use UTF-32 encoding for wide characters.
            if (str == null)
            {
                return IntPtr.Zero;
            }

            nuint byteCount = ((nuint)str.Length + 1) * sizeof(uint); // +1 for null terminator
            uint* ptr = (uint*)NativeMemory.Alloc(byteCount);
            try
            {
                // Copy the string to the allocated memory as UTF-32
                for (int i = 0; i < str.Length; i++)
                {
                    ptr[i] = str[i];
                }

                // Add null terminator
                ptr[str.Length] = 0;

                return new IntPtr(ptr);
            }
            catch
            {
                NativeMemory.Free(ptr);
                throw;
            }
        }
#endif
    }

    public unsafe static IntPtr StringToBSTR(string? str)
    {
#if !NET8_0_OR_GREATER
        return System.Runtime.InteropServices.Marshal.StringToBSTR(str);
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return System.Runtime.InteropServices.Marshal.StringToBSTR(str);
        }
        else
        {
            // On non-Windows platforms, we can use a simple UTF-32 encoding for BSTR.
            // Note: This is a simplified implementation and may not cover all edge cases of BSTR.
            if (str == null)
            {
                return IntPtr.Zero;
            }

            nuint byteCount = ((nuint)str.Length + 1) * sizeof(uint); // +1 for null terminator, *4 for UTF-32
            uint* bstrPtr = (uint*)NativeMemory.Alloc(byteCount + sizeof(uint));
            try
            {
                *bstrPtr = (uint)byteCount;

                // Copy the string to the allocated memory as UTF-32
                for (int i = 0; i < str.Length; i++)
                {
                    bstrPtr[1 + i] = str[i];
                }

                // Add null terminator
                bstrPtr[1 + str.Length] = 0;

                return new IntPtr(bstrPtr);
            }
            catch
            {
                NativeMemory.Free(bstrPtr);
                throw;
            }
        }
#endif
    }

    public unsafe static string? PtrToStringBSTR(IntPtr bstr)
    {
#if !NET8_0_OR_GREATER
        return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(bstr);
#else
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(bstr);
        }
        else
        {
            uint* bstrPtr = (uint*)bstr;
            if (bstrPtr == null)
            {
                return null;
            }

            // On non-Windows platforms, we can read the length prefix and then decode the UTF-32 string.
            uint byteCount = *bstrPtr;
            if (byteCount == 0)
            {
                return string.Empty;
            }

            uint charCount = byteCount / sizeof(uint);
            char[] chars = new char[charCount - 1];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)bstrPtr[1 + i];
            }

            return new string(chars);
        }
#endif
    }
}