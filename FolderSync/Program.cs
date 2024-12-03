using System;
using FolderSync;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Usage: FolderSync.exe <SourcePath> <ReplicaPath> <LogFilePath> <IntervalInSeconds>");
                return;
            }

            string sourcePath = args[0];
            string replicaPath = args[1];
            string logFilePath = args[2];
            if (!int.TryParse(args[3], out int intervalInSeconds) || intervalInSeconds <= 0)
            {
                Console.WriteLine("Invalid synchronization interval, please enter value bigger than zero.");
                return;
            }

            // Validation that the source folder exists
            if (!Directory.Exists(sourcePath))
            {
                Console.WriteLine($"Error: Source folder '{sourcePath}' does not exist.");
                return;
            }

            // Initialize and start synchronization
            var synchronizer = new FolderSynchronizer(sourcePath, replicaPath, logFilePath);
            synchronizer.StartPeriodicSync(intervalInSeconds);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}