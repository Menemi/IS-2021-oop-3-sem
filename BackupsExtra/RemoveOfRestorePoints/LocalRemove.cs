using System.Collections.Generic;
using System.IO;
using Backups;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class LocalRemove : IRestorePointRemover
    {
        public List<RestorePoint> RemoveRestorePoint(
            ComplementedBackupJob backupJob,
            List<RestorePoint> restorePointsToDelete,
            bool isTimecodeOn)
        {
            foreach (var restorePoint in restorePointsToDelete)
            {
                var directory = new DirectoryInfo($"{restorePoint.Path}{restorePoint.Id}");
                directory.Delete(true);
            }

            backupJob.RemoveRestorePoints(restorePointsToDelete, isTimecodeOn);
            return restorePointsToDelete;
        }
    }
}