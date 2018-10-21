using NLua;
using ReassemblyAnalyser.Data;
using ReassemblyAnalyser.Data.DataStruct;
using ReassemblyAnalyser.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Ships
{
    public class Blueprint : IAgent
    {
        public string FilePath { get; set; }
        private string RawData { get; set; }

        public string Name { get; private set; }
        public int Cost { get; private set; }

        public Blueprint (string name, int cost, string blueprintData)
        {
            Name = name;
            Cost = cost;
            RawData = blueprintData;
        }

        public static Blueprint FromTable(IDataStruct blueprintTable)
        {
            var data = blueprintTable.Get ("data");
            string name = data.GetValue("name") as string;
            IDataStruct blocks = blueprintTable.Get ("blocks");
            int cost = 0;
            Console.WriteLine("Parsing blueprint for ship: " + name);

            foreach (IDataStruct block in blocks)
            {
                int id = int.Parse(block.GetValue (0).ToString ());
                cost += BlockData.GetBlockCost(id);
            }

            Console.WriteLine($"Succesfully parsed ship {name} with a cost of {cost}");
            return new Blueprint(name, cost, blueprintTable.ToString());
        }

        public static List<Blueprint> FromFile(string path)
        {
            using (IDataStruct data = DataParse.FromFile(path))
            {
                List<Blueprint> results = new List<Blueprint>();
                IDataStruct blueprints = data.Get("blueprints");

                for (int i = 0; i < blueprints.Count; i++)
                {
                    IDataStruct blueprintTable = blueprints.Get(i);
                    if (blueprintTable.Count > 1)
                    {
                        results.Add(FromTable(blueprintTable));
                    }
                }
                return results;
            }
        }

        public override string ToString()
        {
            return Name + " - " + Cost;
        }

        public string GetRawData() => RawData;
    }
}
