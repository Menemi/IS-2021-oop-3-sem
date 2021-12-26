<<<<<<< HEAD
﻿using System;
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
=======
﻿using System.Collections.Generic;
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
>>>>>>> lab-5

        private IStorageSaver _storageSaver;

        public ComplementedBackupJob(
            IStorageSaver storageSaver,
            FileSystem fileSystem,
            DataService dataService)
            : base(storageSaver, fileSystem)
        {
            _restorePoints = new List<RestorePoint>();
<<<<<<< HEAD
=======
            _jobObjects = new List<FileInfo>();
>>>>>>> lab-5
            _storageSaver = storageSaver;
            Name = storageSaver.GetType().ToString();
            dataService.AddBackupJob(this);
        }

        public string Name { get; }

<<<<<<< HEAD
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
=======
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
>>>>>>> lab-5
            _jobObjects.Remove(file);
        }

        public RestorePoint CreateRestorePoint(
<<<<<<< HEAD
=======
            ILogging logging,
>>>>>>> lab-5
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
<<<<<<< HEAD
            _logger.LogInformation(isTimecodeOn
                ? $"Restore point '{restorePointDirectory.Name}' was successfully created"
                : $"{DateTime.Now}: Restore point '{restorePointDirectory.Name}' was successfully created");
=======
            logging.CreateLog(
                isTimecodeOn,
                $"Restore point '{restorePointDirectory.Name}{restorePoint.Id}' was successfully created");
>>>>>>> lab-5

            return restorePoint;
        }
    }
}