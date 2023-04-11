using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LexiTools
{
    public static partial class FileIO
    {
        public static Dictionary<string, int> GetAllExtensions(string directory, bool includeSubfolders = true)
        {
            _Loggy.LogInfo("Beginning GetAllExtensions at directory " + directory);

            var files = GetAllFilesInternal(directory, includeSubfolders);

            return GetAllExtensions(files.Item1);
        }

        public static Dictionary<string, int> GetAllExtensions(List<FileInfo> files)
        {
            _Loggy.LogInfo("Beginning GetAllExtensions on list of " + files.Count + " files.");

            Dictionary<string, int> extensions = new Dictionary<string, int>();
            foreach (var file in files)
            {
                string extension = file.Extension.ToLower();
                if (extensions.ContainsKey(extension))
                    extensions[extension]++;
                else
                    extensions.Add(extension, 1);
            }

            _Loggy.LogInfo("Completed GetAllExtensions. Extensions found: " + extensions.Count);
            _Loggy.Spacer();

            return extensions;
        }

        /// <summary>
        /// Gets a list of FileInfo objects associated with all files in the provided directory and their total size, with 
        /// or without considering subdirectories.
        /// </summary>
        /// <param name="directory">The directory to get file info from.</param>
        /// <param name="includeSubfolders">Whether or not to get info from files contained in subdirectories as well.</param>
        /// <returns>A tuple containing the list of FileInfo objects, and their total size on disk.</returns>
        public static (List<FileInfo>, long) GetAllFiles(string directory, bool includeSubfolders = true)
        {
            _Loggy.LogInfo("Beginning GetAllFiles at directory " + directory);

            var files = GetAllFilesInternal(directory, includeSubfolders);

            _Loggy.LogInfo("Completed GetAllFiles. " + files.Item1.Count + " files found, totaling " + files.Item2 + " bytes.");
            _Loggy.Spacer();

            return files;
        }

        // Recursive internal method called by GetAllFiles. Wrapped by GetAllFiles for logging.
        private static (List<FileInfo>, long) GetAllFilesInternal(string directory, bool includeSubfolders = true)
        {
            var files = new List<FileInfo>();
            long size = 0;

            // Check that we can even get anything out of this directory.
            //directory = directory.Trim('\\');
            var dirInfo = new DirectoryInfo(directory);
            if (!dirInfo.Exists)
            {
                // If the directory isn't valid, just return an empty set and move on. If we're in recursion, one
                // failure shouldn't stop the whole process - just log the failure and proceed.
                var message = "Directory " + directory + " doesn't exist, was inaccessible, or isn't a valid directory.";
                _Loggy.LogError(message);
                return (new List<FileInfo>(), 0);
            }

            // Next, get the files in the provided directory.
            _Loggy.LogInfo("Processing directory " + dirInfo.FullName);
            DirectoryProcessUpdate?.Invoke(directory, null);

            try
            {
                var tempFiles = Directory.GetFiles(directory).ToList(); // Requires Linq

                foreach (var file in tempFiles)
                {
                    var data = new FileInfo(file);
                    files.Add(data);
                    size += data.Length;
                    _Loggy.LogDebug("File Found: " + file);
                }
            }
            catch (Exception ex)
            {
                _Loggy.LogError("Error with Dir " + directory + ", exception: " + ex.Message);
                return (new List<FileInfo>(), 0);
            }

            // Finally, check the directory's subfolders, if the user provided that we should.
            if (includeSubfolders)
            {
                try
                {
                    var dirs = Directory.GetDirectories(directory).ToList();

                    foreach (var dir in dirs)
                    {
                        var data = GetAllFilesInternal(dir, includeSubfolders);
                        files.AddRange(data.Item1);
                        size += data.Item2;
                    }
                }
                catch (Exception ex)
                {
                    _Loggy.LogError("Error with Dir " + directory + ", exception: " + ex.Message);
                    return (new List<FileInfo>(), 0);
                }
            }

            return (files, size);
        }
    
        private static string SanitizeFilepath(string path)
        {
            //string newPath = path.
            return "";
        }
    }
}