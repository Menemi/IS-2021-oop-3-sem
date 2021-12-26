using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Backups;
using Backups.Interfaces;
using BackupsExtra.Logging;

namespace BackupsExtra
{
    [DataContract]
    public class ComplementedBackupJob : BackupJob
    {
        [DataMember]
        private List<RestorePoint> _restorePoints;

        private List<FileInfo> _jobObjects;

        private IStorageSaver _storageSaver;

        public ComplementedBackupJob(
            IStorageSaver storageSaver,
            FileSystem fileSystem,
            DataService dataService)
            : base(storageSaver, fileSystem)
        {
            _restorePoints = new List<RestorePoint>();
            _jobObjects = new List<FileInfo>();
            _storageSaver = storageSaver;
            Name = storageSaver.GetType().ToString();
            dataService.AddBackupJob(this);
        }

        public string Name { get; }

        [DataMember]
        public string[] JobObjectsPaths
        {
            get { return _jobObjects.Select(file => Path.Combine(file.Directory.ToString(), file.Name)).ToArray(); }
        }

        public FileInfo AddJobObject(ILogging logging, bool isTimecodeOn, string filePath)
        {
            var file = new FileInfo(filePath);
            _jobObjects.Add(file);

            logging.CreateLog(isTimecodeOn, $"Storage '{file.Name}' was successfully created in '{Name}' backup job");
            return file;
        }

        public void DeleteJobObject(ILogging logging, bool isTimecodeOn, FileInfo file)
        {
            logging.CreateLog(isTimecodeOn, $"Storage '{file.Name}' was successfully deleted from '{Name}' backup job");
            _jobObjects.Remove(file);
        }

        public RestorePoint CreateRestorePoint(
            ILogging logging,
            IBackupSaver backupSaver,
            bool isTimecodeOn,
            string restorePointPath,
            string restorePointName)
        {
            var fullRestorePointPath = Path.Combine(restorePointPath, restorePointName);
            var restorePoint = new RestorePoint(fullRestorePointPath);
            _restorePoints.Add(restorePoint);
            _storageSaver.SaveStorage(backupSaver, GetFileSystem(), _jobObjects, restorePoint);

            var restorePointDirectory = new DirectoryInfo(restorePoint.Path);
            logging.CreateLog(
                isTimecodeOn,
                $"Restore point '{restorePointDirectory.Name}{restorePoint.Id}' was successfully created");

            return restorePoint;
        }
    }
}