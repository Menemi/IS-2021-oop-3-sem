using System.Collections.Generic;
using Backups.Interfaces;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            // Local test
            const string generalPath = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";
            IStorageSaver singleSaver = new Single();
            IStorageSaver splitSaver = new Split();
            IBackupSaver localSaver = new LocalSaver();
            var fileSystem = new FileSystem(generalPath);

            var splitBackupJob = new BackupJob(splitSaver, fileSystem);
            var singleBackupJob = new BackupJob(singleSaver, fileSystem);

            const string filePath1 = @"C:\Users\danil\Desktop\name1.txt";
            const string filePath2 = @"C:\Users\danil\Desktop\name2.txt";
            const string filePath3 = @"C:\Users\danil\Desktop\name3.txt";

            var filePaths = new List<string>
            {
                filePath1,
                filePath2,
                filePath3,
            };

            foreach (var filePath in filePaths)
            {
                splitBackupJob.FileExsistingCheck(filePath);
            }

            var file11 = splitBackupJob.AddJobObject(filePath1);
            var file12 = splitBackupJob.AddJobObject(filePath2);
            var file13 = splitBackupJob.AddJobObject(filePath3);
            var file21 = singleBackupJob.AddJobObject(filePath1);
            var file22 = singleBackupJob.AddJobObject(filePath2);
            var file23 = singleBackupJob.AddJobObject(filePath3);

            singleBackupJob.CreateRestorePoint(localSaver, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(localSaver, generalPath, "SplitRestorePoint");
            splitBackupJob.FileExsistingCheck(filePath3);
            splitBackupJob.DeleteJobObject(file13);
            singleBackupJob.DeleteJobObject(file23);
            singleBackupJob.CreateRestorePoint(localSaver, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(localSaver, generalPath, "SplitRestorePoint");
        }
    }
}