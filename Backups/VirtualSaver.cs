using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups
{
    public class VirtualSaver : IBackupSaver
    {
        public void SaveBackup(List<Repository> repositories, FileSystem fileSystem, RestorePoint restorePoint)
        {
            var directories = new List<DirectoryInfo>();
            foreach (var repository in repositories)
            {
                var directory = new DirectoryInfo(restorePoint.Path);
                foreach (var storage in repository.GetStorageList())
                {
                    var file = new FileInfo(Path.Combine(Path.Combine(directory.FullName, storage.Name)));
                }

                directories.Add(directory);
                restorePoint.AddRepository(repository);
            }

            // restorePoint.AddRepositories(repositories);
        }
    }
}