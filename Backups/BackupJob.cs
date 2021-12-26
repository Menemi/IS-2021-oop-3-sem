using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Backups.Exceptions;
using Backups.Interfaces;

namespace Backups
{
    public class BackupJob
    {
        private List<RestorePoint> _restorePoints;

        private List<FileInfo> _jobObjects;

        private IStorageSaver _storageSaver;

        [DataMember]
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

<<<<<<< 02685f340b6d1296e5eec3155f1198c2d936c9e5
=======
        public ReadOnlyCollection<FileInfo> GetJobObjects()
        {
            return _jobObjects.AsReadOnly();
        }

        public FileSystem GetFileSystem()
        {
            return _fileSystem;
        }

>>>>>>> feat: add getter of file system + add data params
        public FileInfo AddJobObject(string filePath)
        {
            var file = new FileInfo(filePath);
            _jobObjects.Add(file);

            return file;
        }

        public void DeleteJobObject(FileInfo file)
        {
            _jobObjects.Remove(file);
        }

        public void FileExsistingCheck(string filePath)
        {
            var file = new FileInfo(filePath);
            if (!file.Exists)
            {
                throw new BackupsException("File doesn't exist");
            }

            var sr = new StreamReader(filePath);
            sr.Close();
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