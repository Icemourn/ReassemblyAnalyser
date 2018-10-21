using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLua;
using ReassemblyAnalyser.Data.DataStruct;

namespace ReassemblyAnalyser.IO
{
    public class LuaParser : IDataParser
    {
        public string FileExtension => ".lua";

        public IDataStruct FromString(string input)
        {
            Lua lua = new Lua();
            input = "data=" + input;
            lua.DoString(input);
            LuaTable dataTable = lua.GetTable("data");
            return new LuaDataStruct(dataTable, lua);
        }
    }
}
