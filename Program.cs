﻿using System;

namespace RSAKeyManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (args.Length == 0)
            {
                Console.WriteLine(options.GetUsage());
                Environment.Exit(1);
            }

            CommandLine.Parser.Default.ParseArguments(args, options);
            if (options.List)
            {
                ListExistingKeyContainers();
            }
            else if (options.NewContainerName != null)
            {
                GenerateNewKeyContainer(options.NewContainerName);
            }
            else if (options.DeleteContainerName != null)
            {
                DeleteKeyContainer(options.DeleteContainerName);
            }
            else if (options.ExportOptions != null)
            {
                if (options.ExportOptions.Length != 2)
                {
                    Console.WriteLine("Invalid options. Use --help for usage.");
                    Environment.Exit(1);
                }
                ExportKeysToXml(options.ExportOptions[0], options.ExportOptions[1]);
            }
            else if (options.ExportPublicOptions != null)
            {
                if (options.ExportPublicOptions.Length != 2)
                {
                    Console.WriteLine("Invalid options. Use --help for usage.");
                    Environment.Exit(1);
                }
                ExportPublicKeyToXml(options.ExportPublicOptions[0], options.ExportPublicOptions[1]);
            }
            else if (options.ImportOptions != null)
            {
                if (options.ImportOptions.Length != 2)
                {
                    Console.WriteLine("Invalid options. Use --help for usage.");
                    Environment.Exit(1);
                }
                ImportKeysFromXml(options.ImportOptions[0], options.ImportOptions[1]);
            }
        }

        private static void ListExistingKeyContainers()
        {
            var containerNames = KeyManager.GetContainerNames();
            if (containerNames == null)
            {
                Console.WriteLine("Unable to list containers.");
                return;
            }
            Console.WriteLine("");
            Console.WriteLine("Existing machine level RSA Key Containers");
            Console.WriteLine("-----------------------------------------");
            foreach (var containerName in containerNames)
            {
               Console.WriteLine(containerName); 
            }
        }

        private static void GenerateNewKeyContainer(string containerName)
        {
            Console.WriteLine($"Creating Key Container \"{containerName}\"");
            if (KeyManager.Generate(containerName))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
                Environment.ExitCode = 1;
            }
        }

        private static void DeleteKeyContainer(string containerName)
        {
            Console.WriteLine($"Deleting Key Container \"{containerName}\"");
            if (KeyManager.Delete(containerName))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
                Environment.ExitCode = 2;
            }
        }

        private static void ExportKeysToXml(string containerName, string filename)
        {
            Console.WriteLine($"Exporting Key Container \"{containerName}\" to \"{filename}\"");
            if (KeyManager.Export(containerName, filename))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
                Environment.ExitCode = 3;
            }
        }

        private static void ExportPublicKeyToXml(string containerName, string filname)
        {
            Console.WriteLine($"Exporting public key from \"{containerName}\" to \"{filname}\"");
            if (KeyManager.ExportPublic(containerName, filname))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
                Environment.ExitCode = 4;
            }
        }

        private static void ImportKeysFromXml(string containerName, string filename)
        {
            Console.WriteLine($"Importing Key Container \"{containerName}\" from \"{filename}\"");
            if (KeyManager.Import(containerName, filename))
            {
                Console.WriteLine("Success");
            }
            else
            {
                Console.WriteLine("Failed");
                Environment.ExitCode = 5;
            }
        }
    }
}
