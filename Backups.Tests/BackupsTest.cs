﻿using System;
using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private const string _generalPath = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";
        private IStorageSaver _singleSaver = new Single();
        private IStorageSaver _splitSaver = new Split();
        private IBackupSaver _virtualSaver = new VirtualSaver();
        private FileSystem _fileSystem = new FileSystem(_generalPath);

        [Test]
        public void VirtualStorageTest()
        {
            var splitBackupJob = new BackupJob(_splitSaver, _fileSystem);
            var singleBackupJob = new BackupJob(_singleSaver, _fileSystem);

            var file11 = splitBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name1.txt");
            var file12 = splitBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name2.txt");
            var file13 = splitBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name3.txt");
            var file21 = singleBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name1.txt");
            var file22 = singleBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name2.txt");
            var file23 = singleBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name3.txt");

            var restorePoint1 = singleBackupJob.CreateRestorePoint(_virtualSaver, _generalPath, "SingleRestorePoint");
            var restorePoint2 = splitBackupJob.CreateRestorePoint(_virtualSaver, _generalPath, "SplitRestorePoint");
            splitBackupJob.DeleteJobObject(file13);
            singleBackupJob.DeleteJobObject(file23);
            var restorePoint3 = singleBackupJob.CreateRestorePoint(_virtualSaver, _generalPath, "SingleRestorePoint");
            var restorePoint4 = splitBackupJob.CreateRestorePoint(_virtualSaver, _generalPath, "SplitRestorePoint");

            Assert.AreEqual(restorePoint1.GetRepositories().Count, 1);
            Assert.AreEqual(restorePoint2.GetRepositories().Count, 3);
            Assert.AreEqual(restorePoint3.GetRepositories().Count, 1);
            Assert.AreEqual(restorePoint4.GetRepositories().Count, 2);
        }
    }
}