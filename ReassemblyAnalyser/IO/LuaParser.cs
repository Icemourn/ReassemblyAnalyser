using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReassemblyAnalyser.Data.DataStruct;
using LsonLib;

namespace ReassemblyAnalyser.IO
{
    public class LuaParser : IDataParser
    {
        public string FileExtension => ".lua";

        public IDataStruct FromString(string input)
        {
            input = "data=" + input;
            LsonValue value = LsonVars.Parse (input)["data"];
            return new LuaDataStruct(value);
        }
    }
}
