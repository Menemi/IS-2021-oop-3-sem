using Backups;

namespace BackupsExtra.Merge
{
    public interface IMergeProcessMethod
    {
        RestorePoint Merge(ComplementedBackupJob backupJob, RestorePoint oldRestorePoint, RestorePoint newRestorePoint, bool isTimecodeOn);
    }
}