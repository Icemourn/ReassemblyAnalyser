using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Data.DataStruct
{
    public class JsonDataStruct : DataStructBase
    {
        private JToken InternalToken { get; set; }

        public override int Count => InternalToken.Children().Count();
        public override bool IsNull => InternalToken == null;

        public JsonDataStruct (JToken token)
        {
            InternalToken = token;
        }

        public override IDataStruct Get(object identifier)
        {
            switch (identifier)
            {
                case int index:
                    var atIndex = (InternalToken as JArray)[index];
                    return new JsonDataStruct(atIndex);

                case string key:
                    var atKey = (InternalToken as JObject).GetValue(key);
                    return new JsonDataStruct(atKey);

                default:
                    throw new ArgumentException("Type " + identifier.GetType ().Name + " is not a valid identifier.");
            }
        }

        public override object GetValue(object identifier)
        {
            var value = Get(identifier);
            return value;
        }

        public override string ToString()
        {
            return InternalToken.ToString();
        }

        public override string Serialize()
        {
            return InternalToken.ToString();
        }
    }
}
