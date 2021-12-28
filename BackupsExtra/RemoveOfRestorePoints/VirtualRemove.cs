using System.Collections.Generic;
using Backups;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class VirtualRemove : IRestorePointRemover
    {
        public List<RestorePoint> RemoveRestorePoint(ComplementedBackupJob backupJob, List<RestorePoint> restorePointsToDelete, bool isTimecodeOn)
        {
            backupJob.RemoveRestorePoints(restorePointsToDelete, isTimecodeOn);
            return restorePointsToDelete;
        }
    }
}