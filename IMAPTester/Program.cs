namespace IMAPTester
{
    using S22.Imap;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine.Text;
    using CommandLine;

    class Program
    {
        static int Main(string[] args)
        {
            Options options = new Options();
            CommandLine.Parser parser = new Parser();

            if (!parser.ParseArguments(args, options))
            {
                Console.WriteLine(options.GetUsage());
                return -2;
            }

            // consume Options type properties
            if (options.Verbose)
            {
                Console.WriteLine("Hostname: {0}", options.HostName);
                Console.WriteLine("Username: {0}", options.Username);
                Console.WriteLine("Password: {0}", options.Password);
                Console.WriteLine("Port    : {0}", options.Port);
                Console.WriteLine("SSL?    : {0}", options.UseSSL);
            }

            try
            {
                using (ImapClient Client = new ImapClient(hostname: options.HostName,
                    port: options.Port,
                    ssl: options.UseSSL,
                    username: options.Username,
                    password: options.Password,
                    method: AuthMethod.Auto))
                {
                    Console.WriteLine("Connection successful.");
                    return 0;
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }
    }

    class Options
    {
        [Option('h', "host", Required = true, HelpText = "The IMAP host.")]
        public string HostName { get; set; }

        [Option('u', "username", Required = true, HelpText = "Username.")]
        public string Username { get; set; }

        [Option('p', "password", Required = true, HelpText = "Password.")]
        public string Password { get; set; }

        [Option('x', "port", DefaultValue = 143, HelpText = "Port number.")]
        public int Port { get; set; }

        [Option('s', "ssl", HelpText = "Use SSL?")]
        public bool UseSSL { get; set; }

        [Option('v', "verbose", HelpText = "Verbose logging?")]
        public bool Verbose { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("IMAPTester", "0.0.1"),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Tests a connection to an IMAP email server.");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Returns: non-zero if error.");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("Usage:");
            help.AddOptions(this);
            return help;
        }
    }
}
