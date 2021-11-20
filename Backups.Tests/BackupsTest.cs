using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private static readonly string _path = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";
        private BackupJob _splitBackupJob;
        private BackupJob _singleBackupJob;

        [SetUp]
        public void Setup()
        {
            _splitBackupJob = new BackupJob("split");
            _singleBackupJob = new BackupJob("single");
        }

        // Local test with real files

        // [Test]
        // public void Test()
        // {
        //     _splitBackupJob.AddRestorePoint("RestorePoint", _path);
        //     _singleBackupJob.AddRestorePoint("RestorePoint", _path);
        //     var file = new FileInfo(@$"{_path}\Job Objects\name1.txt");
        //     file.Delete();
        //     _splitBackupJob.AddRestorePoint("RestorePoint", _path);
        //     _singleBackupJob.AddRestorePoint("RestorePoint", _path);
        // }

        // Tests for git action with virtual files

        [Test]
        public void SingleStorageTest()
        {
            var firstFile = new MyFile("name1", DateTime.Now);
            var secondFile = new MyFile("name2", DateTime.Now);
            var thirdFile = new MyFile("name3", DateTime.Now);

            var filesForBackup = new List<MyFile>
            {
                firstFile,
                secondFile,
                thirdFile
            };

            var startResultFiles = _singleBackupJob.AddVirtualRestorePoint(filesForBackup);
            var startResultFilesCount = startResultFiles[0].Count;
            filesForBackup.Remove(firstFile);
            var resultFiles = _singleBackupJob.AddVirtualRestorePoint(filesForBackup);
            var resultFilesCount = resultFiles[0].Count;

            Assert.True(_singleBackupJob.GetRestorePoints().Count == 2);
            Assert.True(startResultFilesCount == 3);
            Assert.True(resultFilesCount == 2);
        }

        [Test]
        public void SplitStorageTest()
        {
            var firstFile = new MyFile("name1", DateTime.Now);
            var secondFile = new MyFile("name2", DateTime.Now);
            var thirdFile = new MyFile("name3", DateTime.Now);

            var filesForBackup = new List<MyFile>
            {
                firstFile,
                secondFile,
                thirdFile
            };

            var startResultFilesCount = _splitBackupJob.AddVirtualRestorePoint(filesForBackup).Count;
            filesForBackup.Remove(firstFile);
            var resultFilesCount = _splitBackupJob.AddVirtualRestorePoint(filesForBackup).Count;

            Assert.True(_splitBackupJob.GetRestorePoints().Count == 2);
            Assert.True(startResultFilesCount == 3);
            Assert.True(resultFilesCount == 2);
        }
    }
}