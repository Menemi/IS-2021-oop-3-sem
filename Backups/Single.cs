using System.Collections.Generic;
using System.IO;
using Backups.Interfaces;

namespace Backups
{
    public class Single : IStorageSaver
    {
        public void SaveStorage(IBackupSaver backupSaver, FileSystem fileSystem, List<FileInfo> filesToSave, RestorePoint restorePoint)
        {
            var repositories = new List<Repository>();
            var repository = new Repository();
            repository.AddStorages(filesToSave);
            repositories.Add(repository);

            backupSaver.SaveBackup(repositories, fileSystem, restorePoint);
        }
    }
}