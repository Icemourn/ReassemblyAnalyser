using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Ships
{
    public static class AgentExtensions
    {
        public static void SaveTo (this IAgent agent, string path)
        {
            File.Delete(path);
            File.WriteAllText(path, agent.GetRawData());
            agent.FilePath = path;
        }
    }
}
