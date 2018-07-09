using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace SeekAndArchive
{
    class Program
    {
        static List<FileInfo> FoundFiles;
        static List<FileSystemWatcher> watchers;
        static FileSystemWatcher newWatcher;
        static List<DirectoryInfo> archiveDirs;

        static void Main(string[] args)
        {
            string fileName = args[0];
            string directoryName = args[1];
            FoundFiles = new List<FileInfo>();
            watchers = new List<FileSystemWatcher>();
            DirectoryInfo rootDir = new DirectoryInfo(directoryName);

            if (!rootDir.Exists)
            {
                Console.WriteLine("The specified directory does not exist.");
                return;
            }
        
            RecursiveSearch(FoundFiles, fileName, rootDir);
            
            Console.WriteLine("Found {0} files.", FoundFiles.Count);
            foreach (FileInfo fil in FoundFiles)
            {
                Console.WriteLine("{0}", fil.FullName);
            }
            foreach (FileInfo fil in FoundFiles)
            {
                newWatcher = new FileSystemWatcher(fil.DirectoryName, fil.Name);
                newWatcher.Changed += new FileSystemEventHandler(WatcherChanged);
                newWatcher.Created += new FileSystemEventHandler(WatcherChanged);
                newWatcher.Deleted += new FileSystemEventHandler(WatcherChanged);
                newWatcher.Renamed += new RenamedEventHandler(WatcherNameChange);
                newWatcher.EnableRaisingEvents = true;

                watchers.Add(newWatcher);
            }
            
            archiveDirs = new List<DirectoryInfo>();
            for (int i = 0; i < FoundFiles.Count; i++)
            {
                archiveDirs.Add(Directory.CreateDirectory("archive" + i.ToString()));
            }

            Console.ReadKey();
        }

        static void RecursiveSearch(List<FileInfo> foundFiles, string fileName, DirectoryInfo currentDirectory)
        {
            foreach (FileInfo fi in currentDirectory.GetFiles())
            {
                if (fi.Name == fileName)
                {
                    foundFiles.Add(fi);
                }
            }
            foreach (DirectoryInfo dir in currentDirectory.GetDirectories())
            {
                RecursiveSearch(foundFiles, fileName, dir);
            }
        }
        static void WatcherNameChange(object sender, RenamedEventArgs e)
        {
            // try catch to show only once changes
            try
            {
                newWatcher.EnableRaisingEvents = false;
                Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
                FileSystemWatcher senderWatcher = (FileSystemWatcher)sender;
                int index = watchers.IndexOf(senderWatcher, 0);
                try
                {
                    ArchiveFile(archiveDirs[index], FoundFiles[index]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
            }
            finally
            {
                newWatcher.EnableRaisingEvents = true;
            }
        }

        static void WatcherChanged(object sender, FileSystemEventArgs e)
        {       
            // try catch to show only once changes
            try
            {
                newWatcher.EnableRaisingEvents = false;
                Console.WriteLine("{0} has been changed! Change type {1}", e.FullPath, e.ChangeType);
                FileSystemWatcher senderWatcher = (FileSystemWatcher)sender;
                int index = watchers.IndexOf(senderWatcher, 0);
                try
                {
                    ArchiveFile(archiveDirs[index], FoundFiles[index]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    
                }
            }
            finally
            {
                newWatcher.EnableRaisingEvents = true;
            }
        }

        public static void ArchiveFile(DirectoryInfo archiveDir, FileInfo fileToArchive)
        {
            FileStream originalFileStream = fileToArchive.OpenRead();
            
            if ((File.GetAttributes(fileToArchive.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToArchive.Extension != ".gz")
            {
                using (FileStream compressedFileStream = File.Create(fileToArchive.FullName + ".gz"))
                {
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                        CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                    }
                }
                FileInfo info = new FileInfo(archiveDir + "\\" + fileToArchive.Name + ".gz");
                Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                fileToArchive.Name, fileToArchive.Length.ToString(), info.Length.ToString());

                originalFileStream.Close();
            }   
        }
    }
}
