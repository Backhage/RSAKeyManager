using System;
using System.Runtime.InteropServices;
using System.Text;

namespace RSAKeyManager
{
    internal class Win32Crypto
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider,
             uint dwProvType, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam, [In, Out] byte[] pbData,
            ref uint dwDataLen, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool CryptGetProvParam(IntPtr hProv, uint dwParam,
            [MarshalAs(UnmanagedType.LPStr)] StringBuilder pbData, ref uint dwDataLen, uint dwFlags);

        [DllImport("advapi32.dll")]
        public static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);
    }
}