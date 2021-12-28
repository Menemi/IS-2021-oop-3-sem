using System.Collections.Generic;
using Backups;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public interface IRemoveRestorePoint
    {
        // by count / date / hybrid
        List<RestorePoint> RemoveRestorePoint(IRestorePointRemover restorePointRemover, ComplementedBackupJob backupJob, bool isTimecodeOn);
    }
}