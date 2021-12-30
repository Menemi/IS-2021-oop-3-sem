using System.Collections.Generic;
using Backups;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class RemoveByNumber : IRemoveRestorePoint
    {
        public List<RestorePoint> RemoveRestorePoint(
            IRestorePointRemover restorePointRemover,
            ComplementedBackupJob backupJob,
            bool isTimecodeOn)
        {
            if (backupJob.GetNewRestorePoints().Count <= backupJob.RemoveCountCheck)
            {
                return restorePointRemover.RemoveRestorePoint(backupJob, new List<RestorePoint>(), isTimecodeOn);
            }

            return restorePointRemover.RemoveRestorePoint(backupJob, new List<RestorePoint>() { backupJob.GetNewRestorePoints()[0] }, isTimecodeOn);
        }
    }
}