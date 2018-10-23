using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Ships
{
    public interface IAgent
    {
        string Name { get; set; }

        string FilePath { get; set; }

        string GetRawData();
    }
}
