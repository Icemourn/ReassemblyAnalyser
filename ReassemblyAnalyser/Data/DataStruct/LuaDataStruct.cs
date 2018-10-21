using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LsonLib;

namespace ReassemblyAnalyser.Data.DataStruct
{
    // This is almost identical to JsonDataStruct. Could it be possible to combine some of the code?
    public class LuaDataStruct : DataStructBase
    {
        private LsonValue InternalLson { get; set; }

        public override int Count => InternalLson.Count;
        public override bool IsNull => InternalLson == null;

        public LuaDataStruct(LsonValue lson)
        {
            InternalLson = lson;
        }

        public override IDataStruct Get(object identifier)
        {
            OneIndex(ref identifier);
            switch (identifier)
            {
                case int index:
                    var atIndex = InternalLson[index];
                    return new LuaDataStruct(atIndex);

                case string key:
                    var atKey = InternalLson[key];
                    return new LuaDataStruct(atKey);

                default:
                    throw new ArgumentException("Type " + identifier.GetType().Name + " is not a valid identifier.");
            }
        }

        public override object GetValue(object identifier)
        {
            OneIndex(ref identifier);
            var value = Get(identifier);
            return value;
        }

        private void OneIndex(ref object input)
        {
            if (input is int)
            { // I hate one-indexing
                input = (int)input + 1;
            }
        }

        public override string ToString()
        {
            return InternalLson.ToString ();
        }
    }
}
