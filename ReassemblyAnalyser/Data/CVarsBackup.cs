using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReassemblyAnalyser.Data
{

    public static class CVarsBackup
    {
        private static readonly string OriginalPath = "C:\\Users\\Lomztein\\Saved Games\\Reassembly\\data\\cvars.txt";
        private static readonly string BackupPath = AppContext.BaseDirectory + "cvarsCopy.txt";

        public static void Backup ()
        {
            File.Delete(BackupPath);
            File.Copy(OriginalPath, BackupPath);
        }

        public static void Reset ()
        {
            File.Delete(OriginalPath);
            File.Copy(BackupPath, OriginalPath);
        }
    }
}
