using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Exceptions;

namespace Backups
{
    public class BackupJob : IStorageType, IRestorePointSaver
    {
        private static int _idCounter = 1;

        private string _storageType;

        private List<RestorePoint> _restorePoints;

        private int _id;

        public BackupJob(string storageType)
        {
            _id = _idCounter++;
            _storageType = storageType;
            _restorePoints = new List<RestorePoint>();
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return _restorePoints;
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

            var restorePoint = new RestorePoint(restorePointName, backupPlace);
            _restorePoints.Add(restorePoint);

            StorageSaver(_storageType, restorePointName, backupPlace, restorePoint.GetId());
            return restorePoint;
        }

        public List<List<MyFile>> AddVirtualRestorePoint(List<MyFile> files)
        {
            var restorePoint = new RestorePoint();
            _restorePoints.Add(restorePoint);
            return StorageSaver(_storageType, files);
        }

        public void StorageSaver(string storageType, string restorePointName, string backupPlace, int id)
        {
            switch (storageType.ToLower())
            {
                case "split":
                    Split(restorePointName, backupPlace, id);
                    break;
                case "single":
                    Single(restorePointName, backupPlace, id);
                    break;
                default:
                    throw new WrongStorageTypeException();
            }
        }

        public List<List<MyFile>> StorageSaver(string storageType, List<MyFile> files)
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

        private void Single(string restorePointName, string backupPlace, int id)
        {
            var jobObjectsDirectory = new DirectoryInfo(@$"{backupPlace}\Job Objects");
            if (jobObjectsDirectory.GetFiles().Length == 0)
            {
                return;
            }

            var restorePointDirectory = restorePointName + id;
            var archiveName = "Files_" + id;
            var startPath = @$"{backupPlace}\Job Objects";
            var zipPath =
                @$"{backupPlace}\{restorePointDirectory}\{archiveName}.zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }

        private List<List<MyFile>> VirtualSingle(List<MyFile> files)
        {
            var backupFiles = new List<List<MyFile>> { files };
            return backupFiles;
        }

        private void Split(string restorePointName, string backupPlace, int id)
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
        }

        private List<List<MyFile>> VirtualSplit(List<MyFile> files)
        {
            return files.Select(file => new List<MyFile> { file }).ToList();
        }
    }
}