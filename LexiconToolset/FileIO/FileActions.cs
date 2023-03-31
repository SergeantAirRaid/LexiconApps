using System;
using System.IO;

namespace LexiTools
{
    public static partial class FileIO
    {

        public static bool SmartCopy(string sourceFileName, string destFileName, 
            DuplicateFileLogic falseDuplicateLogic = DuplicateFileLogic.Rename, DuplicateFileLogic trueDuplicateLogic = DuplicateFileLogic.Ignore )
        {
            // This detection isn't THAT smart - it just checks if the file sizes are the same. Equal file sizes would indicate they actually are the
            // same file.
            try
            {
                if (File.Exists(destFileName))
                {
                    if( (new FileInfo(sourceFileName)).Length != (new FileInfo(destFileName).Length))
                        return Copy(sourceFileName, destFileName, falseDuplicateLogic);
                    else
                        return Copy(sourceFileName, destFileName, trueDuplicateLogic);
                }
                else
                {
                    return Copy(sourceFileName, destFileName);
                }
            }
            catch(Exception ex)
            {
                _Loggy.LogError("ERROR processing file duplicate smart detection. Message: " + ex.Message);
                return false;
            }
        }

        public static bool CopyTo(string sourceFileName, string destDirectory, DuplicateFileLogic duplicateLogic = DuplicateFileLogic.Rename)
        {
            string destFileName = destDirectory.Trim('\\') + "\\" + (new FileInfo(sourceFileName)).Name;

            return Copy(sourceFileName, destFileName, duplicateLogic);
        }

        /// <summary>
        /// Copies a file. The Duplicate Logic argument determines what Copy will do in the event of a duplicate file. "Ignore" will simply
        /// skip this file. "Overwrite" will overwrite the duplicate in the target directory. "Rename" will copy this file, but with a new
        /// name that allows both files to exist, in the format "FileName (2).ext". "Duplicates Folder" will copy any duplicate to a
        /// subdirectory of the target directory named "Duplicates". If there are duplicates in the Duplicates folder, the file will be
        /// renamed using the "Rename" logic.
        /// </summary>
        /// <param name="sourceFileName">The source file to copy.</param>
        /// <param name="destFileName">The destination file name and path.</param>
        /// <param name="duplicateLogic">Enum determining what to do in the case of a file duplicate.</param>
        /// <returns></returns>
        public static bool Copy(string sourceFileName, string destFileName, DuplicateFileLogic duplicateLogic = DuplicateFileLogic.Rename)
        {
            try
            {
                // Check if the Filename already exists at the destination.
                if (File.Exists(destFileName))
                {
                    switch (duplicateLogic)
                    {
                        // Ignore the duplicate - take no action.
                        case DuplicateFileLogic.Ignore:
                            _Loggy.LogWarning("Duplicate file for " + sourceFileName + " found. File will be ignored.");
                            return true;

                        // Overwrite the duplicate.
                        case DuplicateFileLogic.Overwrite:
                            try
                            {
                                _Loggy.LogInfo("Duplicate file for " + sourceFileName + " found. Duplicate will be overwritten.");
                                return DoCopy(sourceFileName, destFileName, true);
                            }
                            catch(Exception ex)
                            {
                                _Loggy.LogError("ERROR: File " + sourceFileName + " could not overwrite duplicate file. Message: " + ex.Message);
                                return false;
                            }

                        // Rename the duplicate to "<Name> (2).ext"
                        case DuplicateFileLogic.Rename:
                            try
                            {
                                _Loggy.LogInfo("Duplicate file for " + sourceFileName + " found. Copied file will be renamed.");
                                int cnt = 2;
                                string target = destFileName;

                                do
                                {
                                    target = destFileName;
                                    int idx = target.LastIndexOf('.');
                                    if (idx == -1)
                                    {
                                        // File has no extension to be concerned with
                                        target += " (" + cnt + ")";
                                    }
                                    else
                                    {
                                        // Preserve file extension after rename
                                        target = target[..idx] + " (" + cnt + ")." + target[(idx + 1)..];
                                    }
                                    cnt++;
                                }
                                while (File.Exists(target));

                                return DoCopy(sourceFileName, target);
                            }
                            catch(Exception ex)
                            {
                                _Loggy.LogError("ERROR processing rename of file. Message: " + ex.Message);
                                return false;
                            }

                        // Create a folder for duplicates at <TargetDir>\Duplicates
                        // if there is a duplicate in there, follow rename logic
                        case DuplicateFileLogic.DuplicatesFolder:
                            try
                            {
                                var destination = destFileName.Split('\\');
                                destFileName = "";
                                for (int i = 0; i < destination.Length; i++)
                                {
                                    if (i == destination.Length - 1)
                                    {
                                        destFileName += "Duplicates" + "\\";
                                        destFileName += destination[i];
                                    }
                                    else
                                    {
                                        destFileName += destination[i] + "\\";
                                    }
                                }

                                return Copy(sourceFileName, destFileName, DuplicateFileLogic.Rename);
                            }
                            catch(Exception ex)
                            {
                                _Loggy.LogError("ERROR processing duplicates path for this file. Message: " + ex.Message);
                                return false;
                            }
                            
                        // Corrupt method call somehow
                        default:
                            _Loggy.LogWarning("Duplicate file detected, and duplicate setting could not be determined.");
                            return false;
                    }
                }
                else
                {
                    // File doesn't exist, simply copy it
                    _Loggy.LogInfo(destFileName + " will be created.");
                    return DoCopy(sourceFileName, destFileName);
                }
            }
            catch (Exception ex)
            {
                _Loggy.LogError("File " + sourceFileName + " could not be created. Message: " + ex.Message);
                return false;
            }
        }

        // Does the actual file copy - doing the work of creating the target dir if it doesnt exist, and preserving
        // some file attributes of the source file
        private static bool DoCopy(string sourceFileName, string destFileName, bool overwrite = false)
        {
            try
            {
                string targetDir = destFileName.Substring(0, destFileName.LastIndexOf('\\'));
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                var actualCreationDate = File.GetCreationTime(sourceFileName);

                File.Copy(sourceFileName, destFileName, overwrite);

                File.SetCreationTime(destFileName, actualCreationDate);

                return true;
            }
            catch(Exception ex)
            {
                _Loggy.LogError("DoCopy: File " + sourceFileName + " could not be created. Message: " + ex.Message);
                return false;
            }
        }
    }
}
