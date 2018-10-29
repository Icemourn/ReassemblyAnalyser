using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Ships
{
    public class Fleet : IAgent
    {
        public string Name { get; set; }
        public int Cost { get; set; }

        public string FilePath { get; set; }

        Blueprint[] InternalBlueprints { get; set; }

        public Fleet (string name, int cost, Blueprint[] blueprints) {
            Name = name;
            Cost = cost;
            InternalBlueprints = blueprints;
        }

        public string GetRawData()
        {
            string total = GetHeader();
            for (int i = 0; i < InternalBlueprints.Length; i++)
            {
                total += InternalBlueprints[i].GetRawData();
                if (i != InternalBlueprints.Length - 1)
                    total += ",";
            }
            total += GetFooter();
            return total;
        }

        private string GetHeader()
        {
            return "{" +
                "color0=11022," +
                "color1=0x2b0b0b," +
                "color2=0," +
                $"name=\"{Name}\"," +
                "faction=8," +
                "currentChild=0," +
                "blueprint={}," +
                "children={}," +
                "blueprints={";
        }

        private string GetFooter ()
        {
            return "},playerprint={}}";
        }
    }
}
