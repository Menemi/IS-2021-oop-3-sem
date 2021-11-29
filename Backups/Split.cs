using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups
{
    public class Split : IStorageSaver
    {
        public void SaveStorage(IBackupSaver backupSaver, FileSystem fileSystem, List<FileInfo> filesToSave, RestorePoint restorePoint)
        {
            var repositories = new List<Repository>();
            foreach (var file in filesToSave)
            {
                var repository = new Repository();
                repository.AddStorage(file);
                repositories.Add(repository);
            }

            backupSaver.SaveBackup(repositories, fileSystem, restorePoint);
        }
    }
}