using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups
{
    public class BackupJob
    {
        private List<RestorePoint> _restorePoints;

        private List<FileInfo> _jobObjects;

        private IStorageSaver _storageSaver;

        private FileSystem _fileSystem;

        public BackupJob(IStorageSaver storageSaver, FileSystem fileSystem)
        {
            _restorePoints = new List<RestorePoint>();
            _jobObjects = new List<FileInfo>();
            _storageSaver = storageSaver ?? throw new BackupsException("StorageSaver can't be null");
            _fileSystem = fileSystem ?? throw new BackupsException("FileSystem can't be null");
        }

        public ReadOnlyCollection<RestorePoint> GetRestorePoints()
        {
            return _restorePoints.AsReadOnly();
        }

        public FileInfo AddJobObject(string filePath)
        {
            var file = new FileInfo(filePath);

            if (!file.Exists)
            {
                throw new BackupsException("File doesn't exist");
            }

            _jobObjects.Add(file);
            var sr = new StreamReader(filePath);
            sr.Close();
            return file;
        }

        public void DeleteJobObject(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new BackupsException("File doesn't exist");
            }

            _jobObjects.Remove(file);
        }

        // backupPlace - путь до директории, внутри которой будет условная директория "RestorePointN"
        // restorePointName - название директории, то есть как раз-таки условное "RestorePoint"
        public RestorePoint CreateRestorePoint(IBackupSaver backupSaver, string restorePointPath, string restorePointName)
        {
            var fullRestorePointPath = Path.Combine(restorePointPath, restorePointName);
            var restorePoint = new RestorePoint(fullRestorePointPath);
            _restorePoints.Add(restorePoint);
            _storageSaver.SaveStorage(backupSaver, _fileSystem, _jobObjects, restorePoint);
            return restorePoint;
        }
    }
}