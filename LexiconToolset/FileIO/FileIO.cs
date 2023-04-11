using System;
using System.Collections.Generic;
using System.Text;

namespace LexiTools
{
    public static partial class FileIO
    {
        private static Logger _Loggy = new Logger(Logger.LoggerSeverity.Info);

        public static event EventHandler DirectoryProcessUpdate;

        public enum DuplicateFileLogic
        {
            Ignore,             // File will not be copied.
            Overwrite,          // File will overwrite previous.
            Rename,             // File will be copied as "<Name> (2)"
            DuplicatesFolder    // File will be copied to "TargetDir\Duplicates\"
        }
    }
}
