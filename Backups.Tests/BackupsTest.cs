using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private BackupService _backupService;
        private static readonly string _path = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";
        private BackupJob _splitBackupJob;
        private BackupJob _singleBackupJob;
        
        [SetUp]
        public void Setup()
        {
            _backupService = new BackupService();
            _splitBackupJob = _backupService.AddBackupJob("split");
            _singleBackupJob = _backupService.AddBackupJob("single");
        }
        
        // Local test with real files

        // [Test]
        // public void Test()
        // {
        //     _backupService.AddRestorePoint(_splitBackupJob, "RestorePoint", _path);
        //     _backupService.AddRestorePoint(_singleBackupJob, "RestorePoint", _path);
        //     var file = new FileInfo(@$"{_path}\Job Objects\name1.txt");
        //     file.Delete();
        //     _backupService.AddRestorePoint(_splitBackupJob, "RestorePoint", _path);
        //     _backupService.AddRestorePoint(_singleBackupJob, "RestorePoint", _path);
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
            
            var startResultFiles = _backupService.AddVirtualRestorePoint(_singleBackupJob, filesForBackup);
            var startResultFilesCount = startResultFiles[0].Count;
            filesForBackup.Remove(firstFile);
            var resultFiles = _backupService.AddVirtualRestorePoint(_singleBackupJob, filesForBackup);
            var resultFilesCount = resultFiles[0].Count;
            
            Assert.True(_backupService.GetRestorePoints().Count == 2);
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
            
            var startResultFilesCount = _backupService.AddVirtualRestorePoint(_splitBackupJob, filesForBackup).Count;
            filesForBackup.Remove(firstFile);
            var resultFilesCount = _backupService.AddVirtualRestorePoint(_splitBackupJob, filesForBackup).Count;
            
            Assert.True(_backupService.GetRestorePoints().Count == 2);
            Assert.True(startResultFilesCount == 3);
            Assert.True(resultFilesCount == 2);
        }
    }
}