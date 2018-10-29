using ReassemblyAnalyser.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Game
{
    /// <summary>
    /// Represets an instance of the "Reassembly" game.
    /// </summary>
    public class Reassembly : IGameInstance
    {
        private static string ExecutablePath = "D:/Program Files (x86)/Steam/steamapps/common/Reassembly/win32/ReassemblyRelease.exe";
        private static string LogFilePath = "C:\\Users\\Lomztein\\Saved Games\\Reassembly\\data\\log.txt";
        
        public bool Headless { get; private set; }
        public string SandboxScript { get; private set; }
        public bool Done { get; private set; }

        private Process gameProcess;

        public Reassembly (bool headless, string sandboxScript)
        {
            Headless = headless;
            SandboxScript = sandboxScript;
        }

        public void Run ()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = ExecutablePath,
                Arguments =
                    "--EnableDevBindings=1 " +
                    $"--SandboxScript=\"{SandboxScript}\"" +
                    "--LoadSuperFast=1 " +
                    "--SteamEnable=0 " +
                    "--NetworkEnable=0 " +
                    "--TimestampLog=0 " +
                    $"--HeadlessMode={(Headless == true ? 1 : 0)} "
            };

            CVarsBackup.Backup();
            gameProcess = Process.Start(startInfo);
            CVarsBackup.Reset();
        }

        public void WaitForExit()
        {
            gameProcess.WaitForExit();
            Done = true;
        }

        public string[] GetLog ()
        {
            if (!Done)
            {
                throw new GameNotDoneException("Game has not yet been marked as Done, use Reassembly.WaitForExit () before calling Reassembly.GetLog ()");
            }
            return File.ReadAllLines(LogFilePath);
        }
    }
}
