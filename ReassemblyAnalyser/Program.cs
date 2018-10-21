using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLua;
using ReassemblyAnalyser.Data;
using ReassemblyAnalyser.IO;
using ReassemblyAnalyser.Ships;

namespace ReassemblyAnalyser
{
    static class Program
    {
        public static string ExecutablePath = "D:/Program Files (x86)/Steam/steamapps/common/Reassembly/win32/ReassemblyRelease.exe";
        public static string ShipFolderPath = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Reassembly\\data\\ships";
        public static string LogFilePath = "C:\\Users\\Lomztein\\Saved Games\\Reassembly\\data\\log.txt";
        public static string BlockDataURL = "http://www.anisopteragames.com/sync/blockstats.lua";

        //[STAThread]
        static void Main()
        {
            CVarsBackup.Backup();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            var blueprints = Blueprint.FromFile("D:\\Program Files (x86)\\Steam\\userdata\\39229456\\329130\\remote\\data\\save0\\blueprints.lua");
            var data = blueprints.Select(x => x.GetRawData()).ToList ();
            CVarsBackup.Reset();
        }

        public static void RunTest(string shipOnePath, string shipTwoPath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = ExecutablePath,
                Arguments =
                    "--EnableDevBindings=1 " +
                    $"--SandboxScript=\"Arena '{shipOnePath}' '{shipTwoPath}'\"" +
                    "--LoadSuperFast=1 " +
                    "--SteamEnable=0 " +
                    "--NetworkEnable=0 " +
                    "--TimestampLog=0 " +
                    "--HeadlessMode=0 "
            };

            string[] log = File.ReadAllLines(LogFilePath);
            string winner = log.First(x => x.StartsWith("Arena complete, winner is "));
            Console.WriteLine(winner);

            Process reassembly = Process.Start(startInfo);
            reassembly.WaitForExit();
        }
    }
}
