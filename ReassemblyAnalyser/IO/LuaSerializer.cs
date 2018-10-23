using NLua;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.IO
{
    public static class LuaSerializer
    {
        private const string NEWLINE = "\n";
        private const string TAB = "";

        public static string SerializeTable (LuaTable table)
        {
            if (table == null)
                return null;
            return SerializeObject(table, 0);
        }

        private static string SerializeObject (object obj, int depth)
        {
            string result = "";
            Type objectType = obj.GetType();
            if (obj is string)
            {
                result += $"\"{obj}\"";
            }
            else if (IsAny(objectType, typeof(int), typeof(long), typeof(float), typeof(double), typeof (bool)))
            {
                result += obj.ToString().Replace (",", ".");
            } else if (obj is LuaTable table)
            {
                depth++;
                //if (table.Keys.Count != 0)
                // Line 51 in source material. Doesn't look critical, will ignore for now.
                //}

                string tabs = "";
                for (int i = 0; i < depth; i++) tabs += TAB;
                string sep = table.Keys.Count > 4 ? NEWLINE + tabs : "";
                result += sep + "{";
                string js = sep;

                int index = 0;
                foreach (KeyValuePair<object, object> pair in table)
                {
                    bool isStringKey = !int.TryParse(pair.Key.ToString (), out int i);
                    string serializedObject = SerializeObject(pair.Value, depth);

                    if (pair.Key.ToString().StartsWith("color")) // Make sure color is translated back into hex.
                    {
                        int dec = int.Parse (serializedObject.ToString());
                        string hex = "0x" + dec.ToString ("x");
                        serializedObject = hex;
                        //Console.WriteLine(dec + " - " + hex);
                    }

                    if (isStringKey)
                    {
                        js += $"{pair.Key.ToString()}={serializedObject}";
                    }
                    else
                    {
                        js += serializedObject;
                    }
                    if (index != table.Keys.Count - 1)
                    {
                        js += ",";
                    }
                    index++;
                }
                result += js;
                depth--;
                result += "}";
            }
            return result;
        }

        private static bool IsAny (Type type, params Type[] others) => others.Any (x => x.IsEquivalentTo (type));
    }
}
