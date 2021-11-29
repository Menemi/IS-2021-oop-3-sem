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

            var file11 = splitBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name1.txt");
            var file12 = splitBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name2.txt");
            var file13 = splitBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name3.txt");
            var file21 = singleBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name1.txt");
            var file22 = singleBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name2.txt");
            var file23 = singleBackupJob.AddJobObject(@"C:\Users\danil\Desktop\name3.txt");

            singleBackupJob.CreateRestorePoint(localSaver, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(localSaver, generalPath, "SplitRestorePoint");
            splitBackupJob.DeleteJobObject(file13);
            singleBackupJob.DeleteJobObject(file23);
            singleBackupJob.CreateRestorePoint(localSaver, generalPath, "SingleRestorePoint");
            splitBackupJob.CreateRestorePoint(localSaver, generalPath, "SplitRestorePoint");
        }
    }
}