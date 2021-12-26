using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups;
using Backups.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BackupsExtra
{
    public class ComplementedBackupJob : BackupJob
    {
        private static ILogger _logger;

        private List<RestorePoint> _restorePoints;

        private List<FileInfo> _jobObjects => GetJobObjects().ToList();

        private IStorageSaver _storageSaver;

        public ComplementedBackupJob(
            IStorageSaver storageSaver,
            FileSystem fileSystem,
            DataService dataService)
            : base(storageSaver, fileSystem)
        {
            _restorePoints = new List<RestorePoint>();
            _storageSaver = storageSaver;
            Name = storageSaver.GetType().ToString();
            dataService.AddBackupJob(this);
        }

        public string Name { get; }

        [JsonProperty("JobObjectsPaths")]
        public string[] JobObjectsPaths
        {
            get { return GetJobObjects().Select(file => Path.Combine(file.Directory.ToString(), file.Name)).ToArray(); }
        }

        public FileInfo AddJobObject(bool isTimecodeOn, string filePath)
        {
            var file = new FileInfo(filePath);
            _jobObjects.Add(file);
            _logger.LogInformation(isTimecodeOn
                ? $"Storage '{file.Name}' was successfully created"
                : $"{DateTime.Now}: Storage '{file.Name}' was successfully created");

            return file;
        }

        public void DeleteJobObject(bool isTimecodeOn, FileInfo file)
        {
            _logger.LogInformation(isTimecodeOn
                ? $"Storage '{file.Name}' was successfully deleted"
                : $"{DateTime.Now}: Storage '{file.Name}' was successfully deleted");
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
            _storageSaver.SaveStorage(backupSaver, GetFileSystem(), _jobObjects, restorePoint);

            var restorePointDirectory = new DirectoryInfo(restorePoint.Path);
            _logger.LogInformation(isTimecodeOn
                ? $"Restore point '{restorePointDirectory.Name}' was successfully created"
                : $"{DateTime.Now}: Restore point '{restorePointDirectory.Name}' was successfully created");

            return restorePoint;
        }
    }
}