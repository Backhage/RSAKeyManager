using System;
using System.Collections;
using System.Text;

namespace RSAKeyManager
{
    internal class Containers
    {
        internal enum Storage
        {
            Personal = 0,
            Machine = 0x20
        }

        internal static string[] GetContainerNames(Storage storage)
        {
            const uint provRsaFull = 0x1;
            const uint cryptVerifycontext = 0xF0000000;
            const uint ppEnumcontainers = 0x2;
            const uint cryptFirst = 0x1;
            var cspkeytype = (uint)storage;
            var cspFlags = cryptVerifycontext | cspkeytype;
            var hProv = IntPtr.Zero;

            var gotCsp = Win32Crypto.CryptAcquireContext(ref hProv, null, null, provRsaFull, cspFlags);
            if (!gotCsp)
            {
                return null;
            }

            uint pcbData = 0;
            var dwFlags = cryptFirst;
            Win32Crypto.CryptGetProvParam(hProv, ppEnumcontainers, (StringBuilder)null, ref pcbData, dwFlags);
            var bufferSize = (int)(2 * pcbData);
            var sb = new StringBuilder(bufferSize);

            dwFlags = cryptFirst;
            var containerNames = new ArrayList();
            while (Win32Crypto.CryptGetProvParam(hProv, ppEnumcontainers, sb, ref pcbData, dwFlags))
            {
                dwFlags = 0; // Required to continue enumeration
                containerNames.Add(sb.ToString());
            }

            if (hProv != IntPtr.Zero)
            {
                Win32Crypto.CryptReleaseContext(hProv, 0);
            }

            if (containerNames.Count == 0)
            {
                return null;
            }

            return (string[])containerNames.ToArray(typeof(string));
        }
    }
}
