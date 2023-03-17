using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileManagement
{
    public class FileStructureDetails
    {
        public List<FileInfo> Files { get; set; } = new List<FileInfo>();
        public int TotalSize { get; set; } = 0;
        public Dictionary<string, int> FileExtensions { get; set; } = new Dictionary<string, int>();
    }

    public class FileDetails
    {
        public string FilenameFull { get; set; }
        public string Filename { get; set; }
        public int Filesize { get; set; }

        public override string ToString()
        {
            return FilenameFull;
        }
    }

    public class FileManagementException : Exception
    {
        public FileManagementException(string message) : base(message)
        {
        }
    }
}
