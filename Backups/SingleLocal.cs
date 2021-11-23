using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups
{
    public class SingleLocal : ILocalSaver
    {
        public void Save(string restorePointName, string backupPlace, int id)
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
    }
}