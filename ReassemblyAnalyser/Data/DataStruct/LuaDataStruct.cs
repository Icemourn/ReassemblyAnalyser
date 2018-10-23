using NLua;
using ReassemblyAnalyser.IO;
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

        public override string Serialize()
        {
            return LuaSerializer.SerializeTable(InternalTable);
        }
    }
}
