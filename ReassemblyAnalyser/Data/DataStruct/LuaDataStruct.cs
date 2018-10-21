using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Data.DataStruct
{
    public class LuaDataStruct : DataStructBase
    {
        private LuaTable InternalTable { get; set; }
        private Lua Lua { get; set; } // I have no idea why this is needed, but if it gets disposed before usage is done, I get an acceess violation exceptio.

        public override int Count => InternalTable.Keys.Count;
        public override bool IsNull => InternalTable == null;

        public LuaDataStruct(LuaTable table, Lua lua)
        {
            InternalTable = table;
        }

        public override IDataStruct Get(object identifier)
        {
            OneIndex(ref identifier);
            var value = InternalTable[identifier];
            return new LuaDataStruct(value as LuaTable, Lua);
        }

        public override object GetValue(object identifier)
        {
            OneIndex(ref identifier);
            var value = InternalTable[identifier];
            if (value is LuaTable table)
                return new LuaDataStruct(table, Lua);
            return value;
        }

        public override void Dispose()
        {
            if (Lua != null)
                Lua.Dispose();
        }

        private void OneIndex(ref object input)
        {
            if (input is int)
            { // I hate one-indexing
                input = (int)input + 1;
            }
        }

        private string TableToString(LuaTable table, int indentionDepth = 0)
        {
            // I'm not gonna begin to act like I know what is going on here. I just stole it from https://gist.github.com/justnom/9816256 and ported it to C#.

            // Convert a lua table into a lua syntactically correct string
            string result = "{";
            string indent = "";
            for (int i = 0; i < indentionDepth; i++)
            {
                indent += "\t";
            }
            foreach (KeyValuePair<object, object> pair in table)
            {
                // Check the key type(ignore any numerical keys - assume its an array)
                if (pair.Key is string strKey)
                {
                    result +=  $"{indentionDepth}{strKey}=";
                }

                // Check the value type
                if (pair.Value is LuaTable innerTable)
                {
                    result += indentionDepth + TableToString(innerTable, indentionDepth + 1) + "\n";
                }
                else if (pair.Value is bool boolValue)
                {
                    result += indentionDepth + boolValue.ToString() + "\n";
                } else
                {
                    result += $"{indentionDepth}\"{pair.Value}\"\n";
                }
            }
            result += ",";
            // Remove leading commas from the result
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1);
            }
            return result + "}";
        }

        public override string ToString()
        {
            return TableToString(InternalTable);
        }
    }
}
