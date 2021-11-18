using System.IO;

namespace Backups
{
    public class BackupJob
    {
        private static int _idCounter = 1;

        private string _storageType;

        private int _id;

        public BackupJob(string storageType)
        {
            _id = _idCounter++;
            _storageType = storageType;
        }

        public RestorePoint AddRestorePoint(string restorePointName, string backupPlace)
        {
            var directory = new DirectoryInfo(backupPlace);
            var directory2 = new DirectoryInfo(@$"{backupPlace}\Job Objects");

            if (!directory.Exists)
            {
                directory.Create();
            }

            if (!directory2.Exists)
            {
                directory2.Create();
            }

            return new RestorePoint(restorePointName, backupPlace);
        }

        public RestorePoint AddVirtualRestorePoint()
        {
            return new RestorePoint();
        }

        public string GetStorageType()
        {
            return _storageType;
        }
    }
}