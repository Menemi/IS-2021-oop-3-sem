using System.Collections.Generic;
using System.IO;

namespace Backups.Interfaces
{
    public interface IStorageSaver
    {
        // split or single
        void SaveStorage(IBackupSaver backupSaver, FileSystem fileSystem, List<FileInfo> filesToSave, RestorePoint restorePoint);
    }
}