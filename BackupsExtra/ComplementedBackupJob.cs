using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Backups;
using Backups.Interfaces;
using BackupsExtra.Logging;
using BackupsExtra.Merge;
using BackupsExtra.Recovery;
using BackupsExtra.RemoveOfRestorePoints;

namespace BackupsExtra
{
    [DataContract]
    public class ComplementedBackupJob : BackupJob
    {
        [DataMember]
        private List<RestorePoint> _restorePoints;

        private List<FileInfo> _jobObjects;

        private IStorageSaver _storageSaver;

        private FileSystem _fileSystem;

        // by count / date / hybrid
        private IRemoveRestorePoint _removeRestorePoint;

        // virtual / local
        private IRestorePointRemover _restorePointRemover;

        // to origin / custom place
        private IRecoveryPlacement _recoveryPlacement;

        // virtual / local
        private IRecoveryProcessMethod _recoveryProcessMethod;

        // virtual / local
        private IMergeProcessMethod _mergeProcessMethod;

        private ILogging _logger;

        private bool _isAllLimitsOn;

        public ComplementedBackupJob(
            IStorageSaver storageSaver,
            FileSystem fileSystem,
            IRemoveRestorePoint removeRestorePoint,
            IRestorePointRemover restorePointRemover,
            DataService dataService,
            IRecoveryPlacement recoveryPlacement,
            IRecoveryProcessMethod recoveryProcessMethod,
            IMergeProcessMethod mergeProcessMethod,
            ILogging logger,
            bool isAllLimitsOn)
            : base(storageSaver, fileSystem)
        {
            _restorePoints = new List<RestorePoint>();
            _jobObjects = new List<FileInfo>();
            _storageSaver = storageSaver;
            _fileSystem = fileSystem;
            _isAllLimitsOn = isAllLimitsOn;
            _mergeProcessMethod = mergeProcessMethod;
            _recoveryPlacement = recoveryPlacement;
            _recoveryProcessMethod = recoveryProcessMethod;
            _logger = logger;
            _restorePointRemover = restorePointRemover;
            _removeRestorePoint = removeRestorePoint;
            Name = storageSaver.GetType().ToString();
            dataService.AddBackupJob(this);
        }

        public string Name { get; }

        [DataMember]
        public string[] JobObjectsPaths
        {
            get { return _jobObjects.Select(file => Path.Combine(file.Directory.ToString(), file.Name)).ToArray(); }
        }

        public ReadOnlyCollection<RestorePoint> GetNewRestorePoints()
        {
            return _restorePoints.AsReadOnly();
        }

        public bool GetIsAllLimitsOn()
        {
            return _isAllLimitsOn;
        }

        public List<RestorePoint> RemoveRestorePoints(List<RestorePoint> restorePoints, bool isTimecodeOn)
        {
            foreach (var restorePoint in restorePoints)
            {
                _restorePoints.Remove(restorePoint);

                var restorePointDirectory = new DirectoryInfo(restorePoint.Path);
                _logger.CreateLog(
                    isTimecodeOn,
                    $"Restore point '{restorePointDirectory.Name}{restorePoint.Id}' was successfully deleted");
            }

            return _restorePoints;
        }

        public void SetRemoveRestorePoint(IRemoveRestorePoint removeRestorePoint)
        {
            _removeRestorePoint = removeRestorePoint;
        }

        public void SetRestorePointRemover(IRestorePointRemover restorePointRemover)
        {
            _restorePointRemover = restorePointRemover;
        }

        public void SetRecoveryPlace(IRecoveryPlacement recoveryPlacement)
        {
            _recoveryPlacement = recoveryPlacement;
        }

        public void SetRecoveryType(IRecoveryProcessMethod recoveryProcessMethod)
        {
            _recoveryProcessMethod = recoveryProcessMethod;
        }

        public FileInfo AddJobObject(bool isTimecodeOn, string filePath)
        {
            var file = new FileInfo(filePath);
            _jobObjects.Add(file);

            _logger.CreateLog(isTimecodeOn, $"Storage '{file.Name}' was successfully created in '{Name}' backup job");
            return file;
        }

        public void DeleteJobObject(bool isTimecodeOn, FileInfo file)
        {
            _logger.CreateLog(isTimecodeOn, $"Storage '{file.Name}' was successfully deleted from '{Name}' backup job");
            _jobObjects.Remove(file);
        }

        public RestorePoint CreateRestorePoint(
            IBackupSaver backupSaver,
            bool isTimecodeOn,
            string restorePointPath,
            string restorePointName)
        {
            var fullRestorePointPath = Path.Combine(restorePointPath, restorePointName);
            var restorePoint = new RestorePoint(fullRestorePointPath);
            _restorePoints.Add(restorePoint);
            _storageSaver.SaveStorage(backupSaver, _fileSystem, _jobObjects, restorePoint);

            var restorePointDirectory = new DirectoryInfo(restorePoint.Path);
            _logger.CreateLog(
                isTimecodeOn,
                $"Restore point '{restorePointDirectory.Name}{restorePoint.Id}' was successfully created");

            _removeRestorePoint.RemoveRestorePoint(_restorePointRemover, this, isTimecodeOn);
            return restorePoint;
        }

        public void Recovery(RestorePoint restorePoint, bool isTimecodeOn, string pathToRecovery = null)
        {
            _recoveryPlacement.Recovery(_recoveryProcessMethod, restorePoint, pathToRecovery);

            var restorePointDirectory = new DirectoryInfo(restorePoint.Path);
            _logger.CreateLog(
                isTimecodeOn,
                $"Recovery of restore point '{restorePointDirectory.Name}{restorePoint.Id}' was successfully done!");
        }

        public void Merge(RestorePoint oldRestorePoint, RestorePoint newRestorePoint, bool isTimecodeOn)
        {
            _mergeProcessMethod.Merge(this, oldRestorePoint, newRestorePoint, isTimecodeOn);

            var oldRestorePointDirectory = new DirectoryInfo(oldRestorePoint.Path);
            var newRestorePointDirectory = new DirectoryInfo(newRestorePoint.Path);
            _logger.CreateLog(
                isTimecodeOn,
                $"Merge of restore point '{oldRestorePointDirectory.Name}{oldRestorePoint.Id}' was successfully done into restore point '{newRestorePointDirectory.Name}{newRestorePoint.Id}'!");
        }
    }
}