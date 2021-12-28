using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            foreach (var directory in
                restorePointsToDelete.Select(restorePoint =>
                    new DirectoryInfo($"{restorePoint.Path}{restorePoint.Id.ToString()}")))
            {
                directory.Delete(true);
            }

            backupJob.RemoveRestorePoints(restorePointsToDelete, isTimecodeOn);
            return restorePointsToDelete;
        }
    }
}