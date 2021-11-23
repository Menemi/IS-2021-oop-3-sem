namespace Backups.Interfaces
{
    public interface ILocalSaver
    {
        public void Save(string restorePointName, string backupPlace, int id);
    }
}