using ReassemblyAnalyser.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Game
{
    // This is so sad, Alexa play Duel of the Fates.                                                        ..Sorry
    public class DuelArena : IGameInstance
    {
        private readonly Reassembly internalInstance;

        public DuelArena (string agentOne, string agentTwo, bool headless = true)
        {
            internalInstance = new Reassembly(headless, $"Arena '{agentOne}' '{agentTwo}'");
        }

        public DuelArena (IAgent agentOne, IAgent agentTwo, bool headless) : this (agentOne.FilePath, agentTwo.FilePath, headless) { }

        public string[] GetLog()
        {
            return internalInstance.GetLog();
        }

        public void Run()
        {
            internalInstance.Run();
        }

        public void WaitForExit()
        {
            internalInstance.WaitForExit();
        }
    }
}
