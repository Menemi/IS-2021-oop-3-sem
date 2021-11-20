using System.Collections.Generic;

namespace Backups
{
    public interface IRestorePointSaver
    {
        public RestorePoint AddRestorePoint(string restorePointName, string backupPlace);

        public List<List<MyFile>> AddVirtualRestorePoint(List<MyFile> files);
    }
}