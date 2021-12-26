<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.IO;
using Backups;
using Backups.Interfaces;
using Microsoft.Extensions.Logging;
=======
﻿using System.Collections.Generic;
using Backups;
using Backups.Interfaces;
using BackupsExtra.Logging;
>>>>>>> lab-5
using Single = Backups.Single;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            var dataService = new DataService();

            // dataService.LoadData();
            // Console.WriteLine();
            const string generalPath = @"D:\ITMOre than a university\1Menemi1\BackupsExtra\BackupDirectory";
            IStorageSaver singleSaver = new Single();
            IStorageSaver splitSaver = new Split();
<<<<<<< HEAD
=======
            ILogging fileLogging = new FileLogging();
            ILogging consoleLogging = new ConsoleLogging();
>>>>>>> lab-5
            IBackupSaver localSaver = new LocalSaver();
            var fileSystem = new FileSystem(generalPath);

            var splitBackupJob = new ComplementedBackupJob(splitSaver, fileSystem, dataService);
            var singleBackupJob = new ComplementedBackupJob(singleSaver, fileSystem, dataService);

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

<<<<<<< HEAD
            var file11 = splitBackupJob.AddJobObject(filePath1);
            var file12 = splitBackupJob.AddJobObject(filePath2);
            var file13 = splitBackupJob.AddJobObject(filePath3);
            var file21 = singleBackupJob.AddJobObject(filePath1);
            var file22 = singleBackupJob.AddJobObject(filePath2);
            var file23 = singleBackupJob.AddJobObject(filePath3);

            var restorePoint = singleBackupJob.CreateRestorePoint(localSaver, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(localSaver, generalPath, "SplitRestorePoint");
            splitBackupJob.FileExsistingCheck(filePath3);
            splitBackupJob.DeleteJobObject(file13);
            singleBackupJob.DeleteJobObject(file23);
            singleBackupJob.CreateRestorePoint(localSaver, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(localSaver, generalPath, "SplitRestorePoint");
=======
            var file11 = splitBackupJob.AddJobObject(fileLogging, true, filePath1);
            var file12 = splitBackupJob.AddJobObject(fileLogging, true, filePath2);
            var file13 = splitBackupJob.AddJobObject(fileLogging, true, filePath3);
            var file21 = singleBackupJob.AddJobObject(fileLogging, true, filePath1);
            var file22 = singleBackupJob.AddJobObject(fileLogging, true, filePath2);
            var file23 = singleBackupJob.AddJobObject(fileLogging, true, filePath3);

            singleBackupJob.CreateRestorePoint(fileLogging, localSaver, true, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(fileLogging, localSaver, true, generalPath, "SplitRestorePoint");
            splitBackupJob.FileExsistingCheck(filePath3);
            splitBackupJob.DeleteJobObject(fileLogging, true, file13);
            singleBackupJob.DeleteJobObject(fileLogging, true, file23);
            singleBackupJob.CreateRestorePoint(fileLogging, localSaver, true, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(fileLogging, localSaver, true, generalPath, "SplitRestorePoint");
>>>>>>> lab-5

            dataService.SaveData();
        }
    }
}