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
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            ExtractAndSave();
            RunTestsOnDirectory(AppContext.BaseDirectory + "ExtractedShips");
            Console.ReadLine();
        }

        private static void ExtractAndSave ()
        {
            var blueprints = Blueprint.FromFile("D:\\Program Files (x86)\\Steam\\userdata\\39229456\\329130\\remote\\data\\save0\\blueprints.lua");
            Fleet[] fleets = new Fleet[blueprints.Count];

            for (int b = 0; b < blueprints.Count; b++)
            {
                Blueprint blueprint = blueprints[b];
                int amount = 8000 / blueprint.Cost;
                Blueprint[] individual = new Blueprint[amount];
                for (int i = 0; i < amount; i++)
                {
                    individual[i] = new Blueprint(blueprint.Name, blueprint.Cost, blueprint.GetRawData());
                }
                fleets[b] = new Fleet(blueprint.Name, blueprint.Cost * amount, individual);
                string raw = fleets[b].GetRawData();
            }

            SaveAgentsToDirectory(AppContext.BaseDirectory + "ExtractedShips", fleets);
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            CVarsBackup.Reset();
        }

        static void SaveAgentsToDirectory (string path, IAgent[] agents)
        {
            foreach (var agent in agents)
            {
                agent.SaveTo(path + "\\" + agent.Name + ".lua");
            }
        }

        static void RunTestsOnDirectory (string path)
        {
            string[] files = Directory.GetFiles(path);
            for (int x = 0; x < files.Length; x++)
            {
                for (int y = 0; y < files.Length; y++)
                {
                    RunTest(files[x], files[y]);
                }
            }
        }

        public static void RunTest(string shipOnePath, string shipTwoPath)
        {
            CVarsBackup.Backup();
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
            Process reassembly = Process.Start(startInfo);
            CVarsBackup.Reset();
            reassembly.WaitForExit();
            CVarsBackup.Reset();

            //string[] log = File.ReadAllLines(LogFilePath);
            //string winner = log.First(x => x.StartsWith("Arena complete, winner is "));
            //Console.WriteLine(winner);
        }
    }
}
