FolderSync

FolderSync is a C# console application that synchronizes the content of a source folder with a replica folder. The application ensures that the replica is a complete, identical copy of the source folder by comparing file content using MD5 hashes. It also supports periodic synchronization, logs all operations, and is robust against errors.

This project was created as part of interview task I was given when applying for a junior developer position.

Features

One-Way Synchronization:
Ensures the replica folder is a mirror of the source folder.
Deletes files and folders in the replica that are not present in the source.

Content-Based Comparison:
Compares files using MD5 hashing to ensure identical content.

Periodic Execution:
Synchronization runs automatically at user-specified intervals.

Comprehensive Logging:
Logs file operations (copy, update, delete) to both the console and a log file.
Captures errors for easier debugging.

Recursive Folder Synchronization:
Synchronizes nested subfolders.

Error Handling:
Handles missing folders, invalid paths, and access issues gracefully.

Usage

Command-Line Arguments
The application accepts the following command-line arguments:
<SourcePath>: Path to the source folder.
<ReplicaPath>: Path to the replica folder.
<LogFilePath>: Path to the log file.
<IntervalInSeconds>: Interval for periodic synchronization (in seconds).

Example command:
FolderSync.exe "C:\SourceFolder" "C:\ReplicaFolder" "C:\Logs\sync.log" 30

This command:
Synchronizes the contents of C:\SourceFolder with C:\ReplicaFolder.
Logs operations to C:\Logs\sync.log.
Runs synchronization every 30 seconds.

Setup Instructions

Prerequisites
.NET SDK: Ensure the .NET SDK is installed on your system. Download it from Microsoft .NET.

Building the Project
Clone or download the repository.
Open the project in Visual Studio or any C# IDE.
Build the project.
After building the project, locate the .exe file:
bin/Debug/net8.0/FolderSync.exe
Run the program using the command-line arguments.

Behavior and Workflow

File Synchronization:
Files are copied or updated in the replica folder if their MD5 hash differs from the corresponding file in the source folder.
Files in the replica that are missing in the source are deleted.

Folder Synchronization:
Subfolders are handled recursively.
Subfolders in the replica not present in the source are deleted.

Logging:
Logs all operations to both the console and the specified log file.

Example log entry:
2024-12-03 14:32:01: Copied/Updated file: C:\SourceFolder\example.txt to C:\ReplicaFolder\example.txt

Error Handling:
Detects and logs errors such as missing paths or file access issues.

Contributors

Developed by Jakub Maryška
