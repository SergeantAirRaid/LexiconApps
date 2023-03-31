using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace LexiconToolset
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var files = LexiTools.FileIO.GetAllExtensions("D:\\Libraries\\Pictures\\");

                foreach(var ext in files)
                {
                    Console.WriteLine(ext.Key + ": " + ext.Value);
                }


                //Dictionary<long, int> sizes = new Dictionary<long, int>();
                //int duplicates = 0;
                //foreach (var file in files.Item1)
                //{
                //    if (sizes.ContainsKey(file.Length))
                //        sizes[file.Length]++;
                //    else
                //        sizes.Add(file.Length, 1);
                //    //list.Add(line);
                //    //Console.WriteLine(line);
                //}

                //foreach(var size in sizes)
                //{
                //    duplicates += (size.Value - 1);
                //}

                //Dictionary<long, List<string>> sizes = new Dictionary<long, List<string>>();
                //List<string> duplicates = new List<string>();

                //foreach(var file in files.Item1)
                //{
                //    if (!sizes.ContainsKey(file.Length))
                //    {
                //        sizes.Add(file.Length, new List<string>());
                //    }
                //    sizes[file.Length].Add(file.FullName);
                //}

                //foreach(var size in sizes)
                //{
                //    if(size.Value.Count > 1)
                //    {
                //        foreach(var entry in size.Value)
                //        {
                //            duplicates.Add(entry);
                //        }
                //    }
                //}

                //foreach(var dupe in duplicates)
                //{
                //    LexiTools.FileIO.CopyTo(dupe, "D:\\Libraries\\Pictures\\Devtest");
                //}

                //IEnumerable<FileInfo> sortedFiles = files.Item1.OrderBy(c => c.Length);

                //List<String> list = new List<string>();
                //foreach (var file in sortedFiles)
                //{
                //    string line = file.Length + "  " + file.FullName;
                //    list.Add(line);
                //    Console.WriteLine(line);
                //}
            }
            catch { }
        }


    }

    public class CompareFiles : Comparer<FileInfo>
    {
        public override int Compare([AllowNull] FileInfo x, [AllowNull] FileInfo y)
        {
            return (x.Length > y.Length) ? 1 : 0;
        }
    }
}
