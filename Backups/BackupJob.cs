using System.Collections.Generic;
using System.IO;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups
{
    public class BackupJob
    {
        private static int _idCounter = 1;

        private int _id;

        public BackupJob()
        {
            _id = _idCounter++;
            RestorePoints = new List<RestorePoint>();
        }

        private List<RestorePoint> RestorePoints { get; }

        public List<RestorePoint> GetRestorePoints()
        {
            return RestorePoints;
        }

        public void AddJobObject(string filePath)
        {
            var sr = new StreamWriter(filePath);
            sr.Close();
        }

        public void DeleteJobObject(string filePath)
        {
            var file = new FileInfo(filePath);
            file.Delete();
        }

        public RestorePoint AddRestorePoint(
            IVirtualSaver virtualSaver, ILocalSaver localLocalSaver, StorageType storageType, List<string> files, string restorePointName, string backupPlace)
        {
            var restorePoint = new RestorePoint(restorePointName, backupPlace, storageType);
            RestorePoints.Add(restorePoint);
            StorageSaver(virtualSaver, localLocalSaver, storageType, files, restorePointName, backupPlace, restorePoint);
            return restorePoint;
        }

        private void StorageSaver(IVirtualSaver virtualSaver, ILocalSaver localLocalSaver, StorageType storageType, List<string> files, string restorePointName, string backupPlace, RestorePoint restorePoint)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    localLocalSaver.Save(restorePointName, backupPlace, restorePoint.Id);
                    break;
                case StorageType.Virtual:
                    virtualSaver.Save(files, restorePoint);
                    break;
                default:
                    throw new WrongStorageTypeException();
            }
        }
    }
}