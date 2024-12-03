using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Timers;

namespace FolderSync
{
    public class FolderSynchronizer
    {
        private readonly string _sourcePath;
        private readonly string _replicaPath;
        private readonly string _logFilePath;
        private System.Timers.Timer? _syncTimer;

        public FolderSynchronizer(string sourcePath, string replicaPath, string logFilePath)
        {
            _sourcePath = sourcePath;
            _replicaPath = replicaPath;
            _logFilePath = logFilePath;
        }

        public void StartPeriodicSync(int intervalInSeconds)
        {
            _syncTimer = new System.Timers.Timer(intervalInSeconds * 1000);
            _syncTimer.Elapsed += (sender, e) => RunSynchronization();
            _syncTimer.AutoReset = true;
            _syncTimer.Start();

            Console.WriteLine($"Synchronization started. Running every {intervalInSeconds} seconds.");
            Console.WriteLine("Press Enter to stop synchronization...");
            Console.ReadLine();

            StopSynchronization();
        }

        public void StopSynchronization()
        {
            if (_syncTimer != null)
            {
                _syncTimer.Stop();
                _syncTimer.Dispose();
                _syncTimer = null;
            }
            Console.WriteLine("Synchronization stopped.");
        }

        private void RunSynchronization()
        {
            try
            {
                Console.WriteLine($"Synchronization started at {DateTime.Now}");
                SynchronizeFolders(_sourcePath, _replicaPath, _logFilePath);
                Console.WriteLine($"Synchronization completed at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                Log($"Error during synchronization: {ex.Message}", _logFilePath);
            }
        }

        public static void SynchronizeFolders(string sourcePath, string replicaPath, string logFilePath)
        {
            // Create replica folder if it doesn't exist
            if (!Directory.Exists(replicaPath))
            {
                try
                {
                    Directory.CreateDirectory(replicaPath);
                    Log($"Created replica folder: {replicaPath}", logFilePath);
                }
                catch (Exception ex)
                {
                    Log($"Failed to create replica folder '{replicaPath}': {ex.Message}", logFilePath);
                    return;
                }
            }

            // Synchronize files and folders
            foreach (var file in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(replicaPath, fileName);

                try
                {
                    if (!File.Exists(destFile) || !FilesHaveSameContent(file, destFile))
                    {
                        File.Copy(file, destFile, true);
                        Log($"Copied/Updated file: {file} to {destFile}", logFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failed to copy/update file '{file}' to '{destFile}': {ex.Message}", logFilePath);
                }
            }

            // Delete files that don't exist in source folder
            foreach (var file in Directory.GetFiles(replicaPath))
            {
                string fileName = Path.GetFileName(file);
                string sourceFile = Path.Combine(sourcePath, fileName);

                try
                {
                    if (!File.Exists(sourceFile))
                    {
                        File.Delete(file);
                        Log($"Deleted file: {file}", logFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failed to delete file '{file}': {ex.Message}", logFilePath);
                }
            }

            // Recursively synchronize subfolders
            foreach (var dir in Directory.GetDirectories(sourcePath))
            {
                string dirName = Path.GetFileName(dir);
                string destDir = Path.Combine(replicaPath, dirName);

                try
                {
                    SynchronizeFolders(dir, destDir, logFilePath);
                }
                catch (Exception ex)
                {
                    Log($"Failed to synchronize subfolder '{dir}': {ex.Message}", logFilePath);
                }
            }

            // Delete folders that don't exist in source folder
            foreach (var dir in Directory.GetDirectories(replicaPath))
            {
                string dirName = Path.GetFileName(dir);
                string sourceDir = Path.Combine(sourcePath, dirName);

                try
                {
                    if (!Directory.Exists(sourceDir))
                    {
                        Directory.Delete(dir, true);
                        Log($"Deleted folder: {dir}", logFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Log($"Failed to delete folder '{dir}': {ex.Message}", logFilePath);
                }
            }
        }

        private static void Log(string message, string logFilePath)
        {
            try
            {
                Console.WriteLine(message);
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file '{logFilePath}': {ex.Message}");
            }
        }

        private static bool FilesHaveSameContent(string filePath1, string filePath2)
        {
            try
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream1 = File.OpenRead(filePath1))
                    using (var stream2 = File.OpenRead(filePath2))
                    {
                        var hash1 = md5.ComputeHash(stream1);
                        var hash2 = md5.ComputeHash(stream2);
                        return StructuralComparisons.StructuralEqualityComparer.Equals(hash1, hash2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to compare files '{filePath1}' and '{filePath2}': {ex.Message}");
                return false;
            }
        }
    }
}
