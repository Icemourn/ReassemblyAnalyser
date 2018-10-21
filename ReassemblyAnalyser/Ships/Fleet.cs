using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Ships
{
    public class Fleet : IAgent
    {
        public string FilePath { get; set; }

        Blueprint[] InternalBlueprints { get; set; }

        public string GetRawData()
        {
            throw new NotImplementedException();
        }
    }
}
