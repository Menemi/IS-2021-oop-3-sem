using System.IO;

namespace Backups
{
    public class BackupJob
    {
        private static string _path;

        private static int _idCounter = 1;

        private string _storageType;

        private int _id;

        public BackupJob(string storageType, string path = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory")
        {
            _id = _idCounter++;
            _storageType = storageType;
            _path = path;
        }

        public void AddRestorePoint(string restorePointName, string backupPlace)
        {
            var directory = new DirectoryInfo(_path);
            var directory2 = new DirectoryInfo(@$"{_path}\Job Objects");

            if (!directory.Exists)
            {
                directory.Create();
            }

            if (!directory2.Exists)
            {
                directory2.Create();
            }

            var restorePoint = new RestorePoint(restorePointName, _storageType, backupPlace);
        }
    }
}