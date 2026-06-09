using System.Runtime.InteropServices;

namespace SharpSevenZip;

#if NET8_0_OR_GREATER

internal static class Marshal
{
    public unsafe static IntPtr StringToBSTR(string? str)
    {
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

            nuint byteCount = (nuint)str.Length * sizeof(uint); // *4 for UTF-32

            uint* bstrPtr = (uint*)NativeMemory.Alloc(byteCount + sizeof(uint) * 2);
            *bstrPtr = (uint)byteCount;

            // Copy the string to the allocated memory as UTF-32
            for (int i = 0; i < str.Length; i++)
            {
                bstrPtr[1 + i] = str[i];
            }

            // Add null terminator
            bstrPtr[1 + str.Length] = 0;

            return new IntPtr(++bstrPtr);
        }
    }

    public unsafe static string? PtrToStringBSTR(IntPtr bstr)
    {
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
            uint byteCount = *(bstrPtr - 1);
            if (byteCount == 0)
            {
                return string.Empty;
            }

            uint charCount = byteCount / sizeof(uint);
            char[] chars = new char[charCount];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = (char)bstrPtr[i];
            }

            return new string(chars);
        }
    }

    public unsafe static void FreeBSTR(IntPtr bstr)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            System.Runtime.InteropServices.Marshal.FreeBSTR(bstr);
        }
        else
        {
            uint* bstrPtr = (uint*)bstr;
            NativeMemory.Free(--bstrPtr);
        }
    }

    public unsafe static string? PtrToStringUni(IntPtr s)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return System.Runtime.InteropServices.Marshal.PtrToStringUni(s);
        }
        else
        {
            List<char> chars = new();

            uint* s_ptr = (uint*)s;
            while (*s_ptr != 0)
            {
                chars.Add((char)*s_ptr++);
            }

            return new string(chars.ToArray());
        }
    }
}

#endif