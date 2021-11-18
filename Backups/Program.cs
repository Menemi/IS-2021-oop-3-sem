using System;
using System.IO;
using System.IO.Compression;
using Backups;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            // var idCounter = 1;
            // var path = @"D:/ITMOre than a university/1Menemi1/Backups/BackupDirectory";
            // var subpath = @"RestorePoint" + idCounter;
            // var directory = new DirectoryInfo(path);
            // var directory2 = new DirectoryInfo(path + "/Job Objects");
            // if (!directory.Exists)
            // {
            //     directory.Create();
            // }
            //
            // if (!directory2.Exists)
            // {
            //     directory2.Create();
            // }
            //
            // // private void Single()
            // var jobObjectsDirectory =
            //     new DirectoryInfo(@"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory\Job Objects");
            // if (jobObjectsDirectory.GetFiles().Length == 0)
            // {
            //     return;
            // }
            //
            // var restorePointDirectory = "RestorePoint" + idCounter;
            // var archiveName = "Files_" + idCounter;
            // const string startPath = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory\Job Objects";
            // var zipPath =
            //     @$"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory\{restorePointDirectory}\{archiveName}.zip";
            // ZipFile.CreateFromDirectory(startPath, zipPath);
            //
            // // private void Split()
            // var restorePointDirectory2 = "RestorePoint" + idCounter;
            // var jobObjectsDirectory2 =
            //     new DirectoryInfo(@"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory\Job Objects");
            //
            // foreach (var file in jobObjectsDirectory2.GetFiles())
            // {
            //     var archiveName2 =
            //         Path.GetFileNameWithoutExtension(
            //             @$"D:/ITMOre than a university/1Menemi1/Backups/BackupDirectory/Job Objects/{file.Name}")
            //         + "_" + idCounter;
            //
            //     var pathFileToAdd =
            //         @$"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory\Job Objects\{file.Name}";
            //     var zipPath2 =
            //         @$"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory\{subpath}\{archiveName2}.zip";
            //
            //     using var zipArchive = ZipFile.Open(zipPath2, ZipArchiveMode.Create);
            //     zipArchive.CreateEntryFromFile(pathFileToAdd, file.Name);
            // }
        }
    }
}