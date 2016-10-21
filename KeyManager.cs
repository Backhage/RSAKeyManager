using System;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace RSAKeyManager
{
    class KeyManager
    {
        internal static bool Generate(string containerName)
        {
            if (ContainerExists(containerName))
            {
                Console.WriteLine($"Error: A Key Container named \"{containerName}\" already exists.");
                return false;
            }

            var cspParams = new CspParameters()
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.NoPrompt | CspProviderFlags.UseArchivableKey
            };

            try
            {
                var rsa = new RSACryptoServiceProvider(cspParams);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            return true;
        }

        private static bool ContainerExists(string containerName)
        {
            var cspParams = new CspParameters()
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey
            };

            // Ugly way to check if key container already exist.
            try
            {
                var rsa = new RSACryptoServiceProvider(cspParams);
                return true;
            }
            catch
            {
                return false;
            }
        }
        internal static bool Delete(string containerName)
        {
            var cspParams = new CspParameters()
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey
            };

            try
            {
                var rsa = new RSACryptoServiceProvider(cspParams);
                rsa.PersistKeyInCsp = false;
                rsa.Clear();
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            return true;
        }

        internal static bool Export(string containerName, string filename)
        {
            var cspParams = new CspParameters()
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey
            };

            RSACryptoServiceProvider rsa;
            try
            {
                rsa = new RSACryptoServiceProvider(cspParams);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

            var xml = new XmlDocument();
            xml.LoadXml(rsa.ToXmlString(true));

            try
            {
                xml.Save(filename);
            }
            catch (Exception ex) when (ex is XmlException || ex is IOException || ex is UnauthorizedAccessException)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            return true;
        }

        internal static bool Import(string containerName, string filename)
        {
            if (ContainerExists(containerName))
            {
                Console.WriteLine($"A Key Container named \"{containerName}\" already exists.");
                return false;
            }
            var xml = new XmlDocument();
            try
            {
                xml.Load(filename);
            }
            catch (Exception ex) when (ex is XmlException || ex is IOException || ex is AccessViolationException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            var cspParams = new CspParameters()
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            try
            {
                var rsa = new RSACryptoServiceProvider(cspParams);
                rsa.FromXmlString(xml.InnerXml);
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
            return true;
        }
    }
}
