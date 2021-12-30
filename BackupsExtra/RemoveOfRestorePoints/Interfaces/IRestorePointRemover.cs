using System.Collections.Generic;
using Backups;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public interface IRestorePointRemover
    {
        // virtual / local
        List<RestorePoint> RemoveRestorePoint(ComplementedBackupJob backupJob, List<RestorePoint> restorePointsToDelete, bool isTimecodeOn);
    }
}