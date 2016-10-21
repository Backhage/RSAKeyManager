using CommandLine;
using CommandLine.Text;
using System.Collections.Generic;

namespace RSAKeyManager
{
    class Options
    {
        [Option('c', "create", HelpText="Create new RSA key pair. Usage: RSAKeyManager -c \"MyKeys\"")]
        public string NewContainerName { get; set; }

        [Option('d', "delete", HelpText ="Delete an existing RSA key pair. Usage: RSAKeyManager -d \"MyKeys\"")]
        public string DeleteContainerName { get; set; }

        [OptionArray('e', "export", HelpText = "Export to XML. Usage: RSAKeyManager -e \"MyKeys\" \"C:\\users\\me\\keys.xml\"")]
        public string[] ExportOptions { get; set; }

        [OptionArray('i', "import", HelpText = "Import from XML. Usage: RSAKeyManager -i \"MyKeys\" \"C:\\users\\me\\keys.xml\"")]
        public string[] ImportOptions { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
