using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Exceptions;

namespace Backups
{
    public class BackupService : IStorageType
    {
        private List<RestorePoint> _restorePoints = new List<RestorePoint>();

        public BackupJob AddBackupJob(string storageType)
        {
            return new BackupJob(storageType);
        }

        public bool AddRestorePoint(BackupJob backupJob, string restorePointName, string backupPlace)
        {
            var restorePoint = backupJob.AddRestorePoint(restorePointName, backupPlace);
            _restorePoints.Add(restorePoint);
            return StorageSaver(backupJob.GetStorageType(), restorePointName, backupPlace, restorePoint.GetId());
        }

        public List<List<MyFile>> AddVirtualRestorePoint(BackupJob backupJob, List<MyFile> files)
        {
            var restorePoint = backupJob.AddVirtualRestorePoint();
            _restorePoints.Add(restorePoint);
            return VirtualStorageSaver(backupJob.GetStorageType(), files);
        }

        public bool StorageSaver(string storageType, string restorePointName, string backupPlace, int id)
        {
            return storageType.ToLower() switch
            {
                "split" => Split(restorePointName, backupPlace, id),
                "single" => Single(restorePointName, backupPlace, id),
                _ => throw new WrongStorageTypeException()
            };
        }

        public List<List<MyFile>> VirtualStorageSaver(string storageType, List<MyFile> files)
        {
            switch (storageType.ToLower())
            {
                case "split":
                    return VirtualSplit(files);
                case "single":
                    return VirtualSingle(files);
                default:
                    throw new WrongStorageTypeException();
            }
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return _restorePoints;
        }

        private bool Single(string restorePointName, string backupPlace, int id)
        {
            var jobObjectsDirectory = new DirectoryInfo(@$"{backupPlace}\Job Objects");
            if (jobObjectsDirectory.GetFiles().Length == 0)
            {
                return true;
            }

            var restorePointDirectory = restorePointName + id;
            var archiveName = "Files_" + id;
            var startPath = @$"{backupPlace}\Job Objects";
            var zipPath =
                @$"{backupPlace}\{restorePointDirectory}\{archiveName}.zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);
            return true;
        }

        private List<List<MyFile>> VirtualSingle(List<MyFile> files)
        {
            var backupFiles = new List<List<MyFile>> { files };
            return backupFiles;
        }

        private bool Split(string restorePointName, string backupPlace, int id)
        {
            var restorePointDirectory = restorePointName + id;
            var jobObjectsDirectory = new DirectoryInfo(@$"{backupPlace}\Job Objects");

            foreach (var file in jobObjectsDirectory.GetFiles())
            {
                var archiveName =
                    Path.GetFileNameWithoutExtension(@$"{backupPlace}/Job Objects/{file.Name}") + "_" + id;

                var pathFileToAdd = @$"{backupPlace}\Job Objects\{file.Name}";
                var zipPath = @$"{backupPlace}\{restorePointDirectory}\{archiveName}.zip";

                using var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
                zipArchive.CreateEntryFromFile(pathFileToAdd, file.Name);
            }

            return true;
        }

        private List<List<MyFile>> VirtualSplit(List<MyFile> files)
        {
            return files.Select(file => new List<MyFile> { file }).ToList();
        }
    }
}