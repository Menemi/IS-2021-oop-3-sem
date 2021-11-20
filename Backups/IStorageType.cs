using System.Collections.Generic;

namespace Backups
{
    public interface IStorageType
    {
        public void StorageSaver(StorageType storageType, string restorePointName, string backupPlace, int id);

        public List<List<MyFile>> StorageSaver(StorageType storageType, List<MyFile> files);
    }
}