using Backups;

namespace BackupsExtra.Merge
{
    public interface IMergeProcessMethod
    {
        RestorePoint Merge(ComplementedBackupJob backupJobOld, ComplementedBackupJob backupJobNew, RestorePoint oldRestorePoint, RestorePoint newRestorePoint, bool isTimecodeOn);
    }
}