using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Backups.Interfaces;

namespace Backups
{
    public class LocalSaver : IBackupSaver
    {
        public void SaveBackup(List<Repository> repositories, FileSystem fileSystem, RestorePoint restorePoint)
        {
            var directories = new List<DirectoryInfo>();
            var id = 1;
            var newRestorePoint =
                fileSystem.AddRestorePointDirectory(@$"{restorePoint.Path}{restorePoint.Id}");
            var directory = new DirectoryInfo(newRestorePoint.FullName);
            foreach (var repository in repositories)
            {
                var tempDirectory = new DirectoryInfo(Path.Combine(@"C:\Users\danil\Desktop\temp"));
                tempDirectory.Create();

                foreach (var storage in repository.GetStorageList())
                {
                    var file = new FileInfo(storage.FullName);
                    var sr = new StreamReader(file.FullName);
                    sr.Close();
                    var newPath = Path.Combine(tempDirectory.ToString(), file.Name);
                    file.CopyTo(newPath, true);
                }

                ZipFile.CreateFromDirectory(
                    tempDirectory.FullName,
                    Path.Combine(newRestorePoint.FullName, $"Files_{id}.zip"));
                ++id;
                foreach (var file in tempDirectory.GetFiles())
                {
                    var fileToDelete = new FileInfo(file.FullName);
                    var sr = new StreamReader(fileToDelete.FullName);
                    sr.Close();
                    fileToDelete.Delete();
                }

                tempDirectory.Delete();
                directories.Add(directory);
                restorePoint.AddRepository(repository);
            }
        }
    }
}