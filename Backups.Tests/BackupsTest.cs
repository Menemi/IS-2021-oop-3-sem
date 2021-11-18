using System.IO;
using NUnit.Framework;

namespace Backups.Tests
{
    public class Tests
    {
        private BackupJob splitBackupJob;
        private BackupJob singleBackupJob;
        
        [SetUp]
        public void Setup()
        {
            splitBackupJob = new BackupJob("split");
            singleBackupJob = new BackupJob("single");
        }

        [Test]
        public void Test()
        {
            const string path = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";
            splitBackupJob.AddRestorePoint("RestorePoint", path);
            singleBackupJob.AddRestorePoint("RestorePoint", path);
            var file = new FileInfo(@$"{path}\Job Objects\name1.txt");
            file.Delete();
            splitBackupJob.AddRestorePoint("RestorePoint", path);
            singleBackupJob.AddRestorePoint("RestorePoint", path);
        }
    }
}