using Backups;

namespace BackupsExtra.Merge
{
    public interface IMergeType
    {
        RestorePoint Merge(ComplementedBackupJob backupJob, RestorePoint oldRestorePoint, RestorePoint newRestorePoint, bool isTimecodeOn);
    }
}