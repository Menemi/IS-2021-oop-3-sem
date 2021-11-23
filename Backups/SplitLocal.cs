using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups
{
    public class SplitLocal : ILocalSaver
    {
        public void Save(string restorePointName, string backupPlace, int id)
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
    }
}