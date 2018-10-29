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
using ReassemblyAnalyser.Game;
using ReassemblyAnalyser.IO;
using ReassemblyAnalyser.Ships;

namespace ReassemblyAnalyser
{
    static class Program
    {
        public static string ShipFolderPath = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Reassembly\\data\\ships";
        public static string BlockDataURL = "http://www.anisopteragames.com/sync/blockstats.lua";

        //[STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            IAgent[] fleets = ExtractAndSave();
            RunTestsOnAgents (fleets);
            Console.ReadLine();
        }

        private static IAgent[] ExtractAndSave ()
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
            return fleets;
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

        static void RunTestsOnAgents (IAgent[] agents)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = x; y < 4; y++)
                {
                    RunTest(agents[x], agents[y]);
                }
            }
        }

        public static void RunTest(IAgent agentOne, IAgent agentTwo)
        {
            Console.WriteLine($"Testing agent {agentOne.Name} versus {agentTwo.Name}");
            DuelArena duel = new DuelArena(agentOne, agentTwo, true);
            duel.Run();
            duel.WaitForExit();
            string[] log = duel.GetLog();
            string winner = log.First(x => x.StartsWith("Arena complete, winner is "));
            Console.WriteLine(winner);
        }
    }
}
