using Newtonsoft.Json.Linq;
using ReassemblyAnalyser.Data.DataStruct;
using ReassemblyAnalyser.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Data
{
    public static class BlockData
    {
        public static string ShipCostFilePath = "D:\\Program Files (x86)\\Steam\\steamapps\\common\\Reassembly\\data\\blockstats.json";
        private static Dictionary<int, int> BlockCost { get; set; }

        private static void InitData()
        {
            BlockCost = new Dictionary<int, int>();
            var data = DataParse.FromFile(ShipCostFilePath);

            foreach (IDataStruct entry in data)
            {
                var value = entry.GetValue("ident").ToString();
                int blockId = int.Parse(value);
                IDataStruct costToken = entry.Get("deadliness");
                int cost = costToken.IsNull ? 0 : int.Parse(costToken.ToString());
                BlockCost.Add(blockId, cost);
            }
        }

        public static int GetBlockCost(int id)
        {
            if (BlockCost == null)
                InitData();

            return BlockCost[id];
        }
    }
}
