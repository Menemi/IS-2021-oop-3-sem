using System.Collections.Generic;
using Backups;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class RemoveByNumber : IRemoveRestorePoint
    {
        private readonly int _countCheck = 2;

        public List<RestorePoint> RemoveRestorePoint(
            IRestorePointRemover restorePointRemover,
            ComplementedBackupJob backupJob,
            bool isTimecodeOn)
        {
            if (backupJob.GetNewRestorePoints().Count <= _countCheck)
            {
                return restorePointRemover.RemoveRestorePoint(backupJob, new List<RestorePoint>(), isTimecodeOn);
            }

            var counter = 0;
            var restorePointsToDelete = new List<RestorePoint>();
            foreach (var restorePoint in backupJob.GetNewRestorePoints())
            {
                if (backupJob.GetNewRestorePoints().Count - counter == _countCheck)
                {
                    break;
                }

                restorePointsToDelete.Add(restorePoint);
                ++counter;
            }

            return restorePointRemover.RemoveRestorePoint(backupJob, restorePointsToDelete, isTimecodeOn);
        }
    }
}