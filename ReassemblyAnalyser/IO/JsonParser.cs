using Newtonsoft.Json.Linq;
using ReassemblyAnalyser.Data.DataStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.IO
{
    public class JsonParser : IDataParser
    {
        public string FileExtension => ".json";

        public IDataStruct FromString(string input)
        {
            return new JsonDataStruct (JToken.Parse(input));
        }
    }
}
