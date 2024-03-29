﻿using System;
using System.Linq;
using Backups;
using Backups.Interfaces;
using BackupsExtra.Logging;
using BackupsExtra.Merge;
using BackupsExtra.Recovery;
using BackupsExtra.RemoveOfRestorePoints;
using NUnit.Framework;
using Single = Backups.Single;

namespace BackupsExtra.Tests
{
    public class Tests
    {
        private const string generalPath = @"D:\ITMOre than a university\1Menemi1\BackupsExtra\BackupDirectory";
        
        [Test]
        public void AutoRemoveOfRestorePointsTest()
        {
            IStorageSaver singleSaver = new Single();
            IStorageSaver splitSaver = new Split();
            var fileSystem = new FileSystem(generalPath);
            IRemoveRestorePoint numberRemove = new RemoveByNumber();
            IRestorePointRemover virtualRemove = new VirtualRemove();
            IRecoveryPlacement customPlacement = new CustomPlacementRecovery();
            IRecoveryProcessMethod virtualRecovery = new VirtualRecovery();
            IMergeProcessMethod virtualMerge = new VirtualMerge();
            ILogging consoleLogging = new ConsoleLogging();
            IBackupSaver virtualSaver = new VirtualSaver();
            const int countCheck = 2;
            var dateCheck = DateTime.Now.Date.AddDays(-3);
            const bool isAllLimitsOn = false;
            var dataService = new DataService(consoleLogging);
            
            var splitBackupJob = new ComplementedBackupJob(
                splitSaver,
                fileSystem,
                numberRemove, // dateRemove / hybridRemove
                virtualRemove,
                dataService,
                customPlacement, // originalPlace
                virtualRecovery,
                virtualMerge,
                consoleLogging,
                countCheck,
                dateCheck,
                isAllLimitsOn);
            var singleBackupJob = new ComplementedBackupJob(
                singleSaver,
                fileSystem,
                numberRemove, // dateRemove / hybridRemove
                virtualRemove,
                dataService,
                customPlacement, // originalPlace
                virtualRecovery,
                virtualMerge,
                consoleLogging,
                countCheck,
                dateCheck,
                isAllLimitsOn);

            const string filePath1 = @"C:\Users\danil\Desktop\name1.txt";
            const string filePath2 = @"C:\Users\danil\Desktop\name2.txt";
            const string filePath3 = @"C:\Users\danil\Desktop\name3.txt";

            var file11 = splitBackupJob.AddJobObject(true, filePath1);
            var file12 = splitBackupJob.AddJobObject(true, filePath2);
            var file13 = splitBackupJob.AddJobObject(true, filePath3);
            var file21 = singleBackupJob.AddJobObject(true, filePath1);
            var file22 = singleBackupJob.AddJobObject(true, filePath2);
            var file23 = singleBackupJob.AddJobObject(true, filePath3);

            var restorePoint1 = singleBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint2 = splitBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SplitRestorePoint");
            splitBackupJob.DeleteJobObject(true, file13);
            singleBackupJob.DeleteJobObject(true, file23);
            var restorePoint3 = singleBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint4 = singleBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SingleRestorePoint");
            
            Assert.AreEqual(splitBackupJob.GetNewRestorePoints().Count, 1);
            Assert.AreEqual(singleBackupJob.GetNewRestorePoints().Count, 2);
        }

        [Test]
        public void MergeTest()
        {
            IStorageSaver singleSaver = new Single();
            IStorageSaver splitSaver = new Split();
            var fileSystem = new FileSystem(generalPath);
            IRemoveRestorePoint numberRemove = new RemoveByNumber();
            IRestorePointRemover virtualRemove = new VirtualRemove();
            IRecoveryPlacement customPlacement = new CustomPlacementRecovery();
            IRecoveryProcessMethod virtualRecovery = new VirtualRecovery();
            IMergeProcessMethod virtualMerge = new VirtualMerge();
            ILogging consoleLogging = new ConsoleLogging();
            IBackupSaver virtualSaver = new VirtualSaver();
            const int countCheck = 2;
            var dateCheck = DateTime.Now.Date.AddDays(-3);
            const bool isAllLimitsOn = false;
            var dataService = new DataService(consoleLogging);
            
            var splitBackupJob = new ComplementedBackupJob(
                splitSaver,
                fileSystem,
                numberRemove, // dateRemove / hybridRemove
                virtualRemove,
                dataService,
                customPlacement, // originalPlace
                virtualRecovery,
                virtualMerge,
                consoleLogging,
                countCheck,
                dateCheck,
                isAllLimitsOn);
            var singleBackupJob = new ComplementedBackupJob(
                singleSaver,
                fileSystem,
                numberRemove, // dateRemove / hybridRemove
                virtualRemove,
                dataService,
                customPlacement, // originalPlace
                virtualRecovery,
                virtualMerge,
                consoleLogging,
                countCheck,
                dateCheck,
                isAllLimitsOn);

            const string filePath1 = @"C:\Users\danil\Desktop\name1.txt";
            const string filePath2 = @"C:\Users\danil\Desktop\name2.txt";
            const string filePath3 = @"C:\Users\danil\Desktop\name3.txt";

            var file11 = splitBackupJob.AddJobObject(true, filePath1);
            var file12 = splitBackupJob.AddJobObject(true, filePath2);
            var file13 = splitBackupJob.AddJobObject(true, filePath3);
            var file21 = singleBackupJob.AddJobObject(true, filePath1);
            var file22 = singleBackupJob.AddJobObject(true, filePath2);
            var file23 = singleBackupJob.AddJobObject(true, filePath3);

            var restorePoint1 = singleBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint2 = splitBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SplitRestorePoint");
            var restorePoint3 = singleBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint4 = splitBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SplitRestorePoint");
            splitBackupJob.DeleteJobObject(true, file13);
            singleBackupJob.DeleteJobObject(true, file23);
            var restorePoint5 = singleBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SingleRestorePoint");
            var restorePoint6 = splitBackupJob.CreateRestorePoint(virtualSaver, true, generalPath, "SplitRestorePoint");
            
            // я знаю, что сначала идёт логика, и только потом все ассёрты, но тут
            // так проще, а лишнюю запоминающую переменную не хочется, пощадите
            singleBackupJob.Merge(singleBackupJob, restorePoint3, restorePoint5, true);
            var countOfStorages = restorePoint5.GetRepositories()
                .SelectMany(repository => repository.GetStorageList()).Count();
            Assert.AreEqual(countOfStorages, 2);
            
            singleBackupJob.Merge(splitBackupJob,restorePoint5, restorePoint4, true);
            countOfStorages = restorePoint4.GetRepositories()
                .SelectMany(repository => repository.GetStorageList()).Count();
            Assert.AreEqual(countOfStorages, 3);
            
            splitBackupJob.Merge(singleBackupJob, restorePoint4, restorePoint3, true);
            countOfStorages = restorePoint3.GetRepositories()
                .SelectMany(repository => repository.GetStorageList()).Count();
            Assert.AreEqual(countOfStorages, 3);
            
            splitBackupJob.Merge(splitBackupJob,restorePoint4, restorePoint6, true);
            countOfStorages = restorePoint6.GetRepositories()
                .SelectMany(repository => repository.GetStorageList()).Count();
            Assert.AreEqual(countOfStorages, 3);
        }
    }
}