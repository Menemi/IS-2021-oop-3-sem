namespace Backups
{
    public interface IStorageType
    {
        public bool StorageSaver(string storageType, string restorePointName, string backupPlace, int id);
    }
}