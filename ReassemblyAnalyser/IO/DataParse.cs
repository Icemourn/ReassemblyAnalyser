using ReassemblyAnalyser.Data.DataStruct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.IO
{
    public static class DataParse
    {
        private static readonly IDataParser[] Parsers = new IDataParser[]
        {
            new JsonParser (),
            new LuaParser (),
        };

        private static IDataParser GetParser (string path)
        {
            string extension = Path.GetExtension(path);
            return Parsers.First(x => x.FileExtension == extension);
        }

        public static IDataStruct FromFile (string path)
        {
            IDataParser parser = GetParser(path);
            return parser.FromFile(path);
        }
    }
}
