using System;

namespace LexiconToolset
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var q = FileManagement.FileManagement.GetAllFiles("D:\\Libraries\\Pictures\\", true);
                var o = FileManagement.FileManagement.GetAllExtensions(q.Item1);
            }
            catch { }
        }
    }
}
