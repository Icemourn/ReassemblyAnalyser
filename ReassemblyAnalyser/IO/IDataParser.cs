using ReassemblyAnalyser.Data.DataStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.IO
{
    public interface IDataParser
    {
        string FileExtension { get; }

        IDataStruct FromString (string input);
    }
}
