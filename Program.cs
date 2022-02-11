using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App
{
    record AutoLaunchProtocolEntry
    {
        [JsonPropertyName("allowed_origins")]
        public string[] AllowedOrigins { get; init; }
        [JsonPropertyName("protocol")]
        public string Protocol { get; init; }
    }

    static class Program
    {
        public readonly static string PROTO = "wv2proto";
        public readonly static string APP_NAME = "wv2proto test app";

        [STAThread]
        static void Main(string[] args)
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                RegisterCustomProto();
                Application.Run(new MainForm());
            }
            else
            {
                MessageBox.Show(Environment.CommandLine, caption: APP_NAME);
            }
        }

        static void RegisterCustomProto()
        {
            // register custom protocol

            using var key = Registry.CurrentUser.CreateSubKey(
                $@"Software\Classes\{PROTO}",
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue(String.Empty, $"URL:{APP_NAME}");
            key.SetValue("URL Protocol", APP_NAME);

            using var protoKey = key.CreateSubKey(@"shell\open\command",
                RegistryKeyPermissionCheck.ReadWriteSubTree);
            protoKey.SetValue(String.Empty, $"\"{Application.ExecutablePath}\" \"%1\"");

            using var policyKey = Registry.CurrentUser.CreateSubKey(
                @"SOFTWARE\Policies\Microsoft\Edge\WebView2", 
                RegistryKeyPermissionCheck.ReadWriteSubTree);

            // enable protocol autolaunch

            // read exisiting entries
            var keyValue = policyKey.GetValue("AutoLaunchProtocolsFromOrigins") as string;
            var entries = new List<AutoLaunchProtocolEntry>();
            if (!String.IsNullOrEmpty(keyValue))
            {
                var oldEntries = JsonSerializer.Deserialize<AutoLaunchProtocolEntry[]>(keyValue);
                entries.AddRange(oldEntries.Where(entry => entry.Protocol != PROTO));
            }

            entries.Add(new AutoLaunchProtocolEntry
            {
                AllowedOrigins = new string[] { "*" },
                Protocol = PROTO
            });

            keyValue = JsonSerializer.Serialize(entries);
            policyKey.SetValue("AutoLaunchProtocolsFromOrigins", keyValue); 
        }
    }
}
