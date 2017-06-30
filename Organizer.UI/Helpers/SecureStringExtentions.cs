using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Organizer.UI.Helpers
{
    /// <summary>
    /// Helper from https://stackoverflow.com/questions/818704/how-to-convert-securestring-to-system-string
    /// </summary>
    public static class SecureStringExtentions
    {
        public static String SecureStringToString(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public static bool IsNullOrEmpty(this SecureString value)
        {
            return value == null || value.Length == 0;
        }
    }
}