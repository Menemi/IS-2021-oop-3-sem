using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Interfaces;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private static readonly string _path = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";
        private BackupJob _backupJob;
        private ILocalSaver _splitLocalSaver;
        private ILocalSaver _singleLocalSaver;
        private IVirtualSaver _splitSaver;
        private IVirtualSaver _singleSaver;

        [SetUp]
        public void Setup()
        {
            _backupJob = new BackupJob();
            _splitLocalSaver = new SplitLocal();
            _singleLocalSaver = new SingleLocal();
            _splitSaver = new SplitStorage();
            _singleSaver = new SingleStorage();
        }

        [Test]
        public void VirtualStorageTest()
        {
            var file1 = @$"{_path}/Job Objects/name1.txt";
            var file2 = @$"{_path}/Job Objects/name2.txt";
            var file3 = @$"{_path}/Job Objects/name3.txt";

            var files = new List<string>
            {
                file1,
                file2,
                file3
            };

            foreach (var file in files)
            {
                _backupJob.AddJobObject(file);
            }

            var restorePoint1 = _backupJob.AddRestorePoint(_splitSaver, _splitLocalSaver, StorageType.Virtual, "RestorePoint", _path);
            var restorePoint2 = _backupJob.AddRestorePoint(_singleSaver, _splitLocalSaver, StorageType.Virtual, "RestorePoint", _path);
            _backupJob.DeleteJobObject(file1);
            var restorePoint3 = _backupJob.AddRestorePoint(_splitSaver, _splitLocalSaver, StorageType.Virtual, "RestorePoint", _path);
            var restorePoint4 = _backupJob.AddRestorePoint(_singleSaver, _splitLocalSaver, StorageType.Virtual, "RestorePoint", _path);

            Assert.True(restorePoint1.GetRepositories().Count == 3);
            Assert.True(restorePoint2.GetRepositories().Count == 1);
            Assert.True(restorePoint3.GetRepositories().Count == 2);
            Assert.True(restorePoint4.GetRepositories().Count == 1);
        }
    }
}