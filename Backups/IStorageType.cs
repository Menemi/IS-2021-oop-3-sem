using System.Collections.Generic;

namespace Backups
{
    public interface IStorageType
    {
        public void StorageSaver(string storageType, string restorePointName, string backupPlace, int id);

        public List<List<MyFile>> StorageSaver(string storageType, List<MyFile> files);
    }
}