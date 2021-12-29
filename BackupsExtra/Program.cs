using System.Collections.Generic;
using System.IO;
using Backups;
using Backups.Interfaces;
using BackupsExtra.Logging;
using BackupsExtra.Merge;
using BackupsExtra.Recovery;
using BackupsExtra.RemoveOfRestorePoints;
using Single = Backups.Single;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            const string generalPath = @"D:\ITMOre than a university\1Menemi1\BackupsExtra\BackupDirectory";

            IStorageSaver singleSaver = new Single();
            IStorageSaver splitSaver = new Split();
            var fileSystem = new FileSystem(generalPath);
            IRemoveRestorePoint dateRemove = new RemoveByDate();
            IRemoveRestorePoint hybridRemove = new RemoveByHybrid();
            IRemoveRestorePoint numberRemove = new RemoveByNumber();
            IRestorePointRemover localRemove = new LocalRemove();
            IRecoveryPlacement originalPlacement = new OriginalPlacementRecovery();
            IRecoveryPlacement customPlacement = new CustomPlacementRecovery();
            IRecoveryProcessMethod localRecovery = new LocalRecovery();
            IMergeProcessMethod localMerge = new LocalMerge();
            ILogging fileLogging = new FileLogging();
            ILogging consoleLogging = new ConsoleLogging();
            IBackupSaver localSaver = new LocalSaver();
            var dataService = new DataService(fileLogging);

            // dataService.LoadData();
            var splitBackupJob = new ComplementedBackupJob(
                splitSaver,
                fileSystem,
                numberRemove, // dateRemove / hybridRemove
                localRemove,
                dataService,
                customPlacement, // originalPlace
                localRecovery,
                localMerge,
                fileLogging,
                false);
            var singleBackupJob = new ComplementedBackupJob(
                singleSaver,
                fileSystem,
                numberRemove, // dateRemove / hybridRemove
                localRemove,
                dataService,
                customPlacement, // originalPlace
                localRecovery,
                localMerge,
                fileLogging,
                false);

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

            var file11 = splitBackupJob.AddJobObject(true, filePath1);
            var file12 = splitBackupJob.AddJobObject(true, filePath2);
            var file13 = splitBackupJob.AddJobObject(true, filePath3);
            var file21 = singleBackupJob.AddJobObject(true, filePath1);
            var file22 = singleBackupJob.AddJobObject(true, filePath2);
            var file23 = singleBackupJob.AddJobObject(true, filePath3);

            var restorePoint1 = singleBackupJob.CreateRestorePoint(localSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint2 = splitBackupJob.CreateRestorePoint(localSaver, true, generalPath, "SplitRestorePoint");
            splitBackupJob.FileExsistingCheck(filePath3);
            splitBackupJob.DeleteJobObject(true, file13);
            singleBackupJob.DeleteJobObject(true, file23);
            var restorePoint3 = singleBackupJob.CreateRestorePoint(localSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint4 = splitBackupJob.CreateRestorePoint(localSaver, true, generalPath, "SplitRestorePoint");
            var restorePoint5 = singleBackupJob.CreateRestorePoint(localSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint6 = splitBackupJob.CreateRestorePoint(localSaver, true, generalPath, "SplitRestorePoint");

            singleBackupJob.Merge(restorePoint1, restorePoint3, true);
            singleBackupJob.Merge(restorePoint3, restorePoint2, true);
            singleBackupJob.Merge(restorePoint2, restorePoint4, true);

            var directory = new DirectoryInfo("D:/ITMOre than a university/1Menemi1/BackupsExtra/testDirectory");
            directory.Create();
            singleBackupJob.Recovery(restorePoint3, true, "D:/ITMOre than a university/1Menemi1/BackupsExtra/testDirectory");
            dataService.SaveData(true);
        }
    }
}