using System.IO;
using System.IO.Compression;
using Backups.Exceptions;

namespace Backups
{
    public class RestorePoint
    {
        private static int _idCounter = 1;

        private static string _path = @"D:\ITMOre than a university\1Menemi1\Backups\BackupDirectory";

        private int _id;

        public RestorePoint(string restorePointName, string storageType, string backupPlace)
        {
            _id = _idCounter++;
            _path = backupPlace;

            var directory = new DirectoryInfo(@$"{_path}\{restorePointName}{_id}");

            if (!directory.Exists)
            {
                directory.Create();
            }

            switch (storageType.ToLower())
            {
                case "split":
                    Split(restorePointName, _path);
                    break;
                case "single":
                    Single(restorePointName, _path);
                    break;
                default:
                    throw new WrongStorageTypeException();
            }
        }

        private void Single(string restorePointName, string backupPlace)
        {
            var jobObjectsDirectory = new DirectoryInfo(@$"{backupPlace}\Job Objects");
            if (jobObjectsDirectory.GetFiles().Length == 0)
            {
                return;
            }

            var restorePointDirectory = restorePointName + _id;
            var archiveName = "Files_" + _id;
            var startPath = @$"{backupPlace}\Job Objects";
            var zipPath =
                @$"{backupPlace}\{restorePointDirectory}\{archiveName}.zip";
            ZipFile.CreateFromDirectory(startPath, zipPath);
        }

        private void Split(string restorePointName, string backupPlace)
        {
            var restorePointDirectory = restorePointName + _id;
            var jobObjectsDirectory = new DirectoryInfo(@$"{backupPlace}\Job Objects");

            foreach (var file in jobObjectsDirectory.GetFiles())
            {
                var archiveName =
                    Path.GetFileNameWithoutExtension(@$"{backupPlace}/Job Objects/{file.Name}") + "_" + _id;

                var pathFileToAdd = @$"{backupPlace}\Job Objects\{file.Name}";
                var zipPath = @$"{backupPlace}\{restorePointDirectory}\{archiveName}.zip";

                using var zipArchive = ZipFile.Open(zipPath, ZipArchiveMode.Create);
                zipArchive.CreateEntryFromFile(pathFileToAdd, file.Name);
            }
        }
    }
}