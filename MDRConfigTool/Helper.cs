using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace MDRConfigTool
{
    public class Helper
    {
        [DllImport("ole32.dll")]
        public static extern int GetRunningObjectTable(int reserved, out UCOMIRunningObjectTable prot);

        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(int reserved, out UCOMIBindCtx ppbc);

        public static string getFreeTcDirectory(string basePath)
        {
            int max = 1;
            List<string> directories = new List<string>(Directory.EnumerateDirectories(basePath));
            foreach(var directory in directories)
            {
                string[] dirNames = directory.Split('\\');
                if (dirNames[dirNames.Length - 1].Contains("TwinCAT Project"))
                {
                    string number = dirNames[dirNames.Length - 1].Substring(15, dirNames[dirNames.Length - 1].Length - 15);
                    if (Convert.ToInt32(number) > max)
                        max = Convert.ToInt32(number);
                }
            }
            max++;

            return "TwinCAT Project" + max.ToString();
        }

        /// <summary>
        /// Get a snapshot of the running object table (ROT).
        /// </summary>
        /// <returns>A hashtable mapping the name of the object
        //     in the ROT to the corresponding object</returns>
        public static Hashtable GetRunningObjectTable()
        {
            Hashtable result = new Hashtable();

            int numFetched;
            UCOMIRunningObjectTable runningObjectTable;
            UCOMIEnumMoniker monikerEnumerator;
            UCOMIMoniker[] monikers = new UCOMIMoniker[1];

            GetRunningObjectTable(0, out runningObjectTable);
            runningObjectTable.EnumRunning(out monikerEnumerator);
            monikerEnumerator.Reset();

            while (monikerEnumerator.Next(1, monikers, out numFetched) == 0)
            {
                UCOMIBindCtx ctx;
                CreateBindCtx(0, out ctx);

                string runningObjectName;
                monikers[0].GetDisplayName(ctx, null, out runningObjectName);

                object runningObjectVal;
                runningObjectTable.GetObject(monikers[0], out runningObjectVal);

                result[runningObjectName] = runningObjectVal;
            }

            return result;
        }


        /// <summary>
        /// Get a table of the currently running instances of the Visual Studio .NET IDE.
        /// </summary>
        /// <param name="openSolutionsOnly">Only return instances
        ///                   that have opened a solution</param>
        /// <returns>A hashtable mapping the name of the IDE
        ///       in the running object table to the corresponding
        ///                                  DTE object</returns>
        public static Hashtable GetIDEInstances(bool openSolutionsOnly, string progId)
        {
            Hashtable runningIDEInstances = new Hashtable();
            Hashtable runningObjects = GetRunningObjectTable();

            IDictionaryEnumerator rotEnumerator = runningObjects.GetEnumerator();
            while (rotEnumerator.MoveNext())
            {
                string candidateName = (string)rotEnumerator.Key;
                if (!candidateName.StartsWith("!" + progId))
                    continue;

                EnvDTE.DTE ide = rotEnumerator.Value as EnvDTE.DTE;
                if (ide == null)
                    continue;

                if (openSolutionsOnly)
                {
                    try
                    {
                        string solutionFile = ide.Solution.FullName;
                        if (solutionFile != String.Empty)
                        {
                            runningIDEInstances[candidateName] = ide;
                        }
                    }
                    catch { }
                }
                else
                {
                    runningIDEInstances[candidateName] = ide;
                }
            }
            return runningIDEInstances;
        }


        /// <summary>
        /// Deletes the specified directory
        /// </summary>
        /// <param name="target_dir">The target_dir.</param>
        public static void DeleteDirectory(string target_dir)
        {
            if (Directory.Exists(target_dir))
            {
                DeleteDirectoryFiles(target_dir);
                while (Directory.Exists(target_dir))
                {
                    //lock (_lock)
                    {
                        DeleteDirectoryDirs(target_dir);
                    }
                }
            }
        }

        private static void DeleteDirectoryDirs(string target_dir)
        {
            System.Threading.Thread.Sleep(100);

            if (Directory.Exists(target_dir))
            {

                string[] dirs = Directory.GetDirectories(target_dir);

                if (dirs.Length == 0)
                    Directory.Delete(target_dir, false);
                else
                    foreach (string dir in dirs)
                        DeleteDirectoryDirs(dir);
            }
        }

        private static void DeleteDirectoryFiles(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectoryFiles(dir);
            }
        }


        /// <summary>
        /// Copies a directory from Source to Destination
        /// </summary>
        /// <param name="src">Source Folder</param>
        /// <param name="dst">Destingation Folder</param>
        public static void CopyDirectory(string src, string dst)
        {
            String[] files;

            if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
                dst += Path.DirectorySeparatorChar;

            if (!Directory.Exists(dst))
                Directory.CreateDirectory(dst);

            files = Directory.GetFileSystemEntries(src);

            foreach (string element in files)
            {
                // Sub directories

                if (Directory.Exists(element))
                    CopyDirectory(element, dst + Path.GetFileName(element));
                // Files in directory

                else
                    File.Copy(element, dst + Path.GetFileName(element), true);
            }
        }

    }
}
