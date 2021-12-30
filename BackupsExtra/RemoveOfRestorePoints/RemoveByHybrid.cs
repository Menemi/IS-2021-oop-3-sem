using System;
using System.Collections.Generic;
using System.Linq;
using Backups;
using BackupsExtra.Exception;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class RemoveByHybrid : IRemoveRestorePoint
    {
        public List<RestorePoint> RemoveRestorePoint(
            IRestorePointRemover restorePointRemover,
            ComplementedBackupJob backupJob,
            bool isTimecodeOn)
        {
            var resultRemoveList = new List<RestorePoint>();
            if (backupJob.IsAllLimitsOn)
            {
                var restorePointsToDelete = backupJob.GetNewRestorePoints()
                    .Where(restorePoint => restorePoint.CreationTime.Date <= backupJob.RemoveDateCheck).ToList();

                if (restorePointsToDelete.Count == backupJob.GetNewRestorePoints().Count)
                {
                    throw new BackupsExtraException(
                        $"The count of restore points is {backupJob.GetNewRestorePoints().Count}, you can't remove all of them!");
                }

                var counter = 0;
                var restorePointsToDelete2 = new List<RestorePoint>();
                foreach (var restorePoint in backupJob.GetNewRestorePoints())
                {
                    if (backupJob.GetNewRestorePoints().Count - counter == backupJob.RemoveCountCheck)
                    {
                        break;
                    }

                    restorePointsToDelete2.Add(restorePoint);
                    ++counter;
                }

                foreach (var restorePoint1 in restorePointsToDelete)
                {
                    foreach (var restorePoint2 in restorePointsToDelete2)
                    {
                        if (restorePoint1 == restorePoint2)
                        {
                            resultRemoveList.Add(restorePoint1);
                        }
                    }
                }
            }
            else
            {
                var restorePointsToDelete = backupJob.GetNewRestorePoints()
                    .Where(restorePoint => restorePoint.CreationTime.Date <= backupJob.RemoveDateCheck).ToList();

                if (restorePointsToDelete.Count == backupJob.GetNewRestorePoints().Count)
                {
                    throw new BackupsExtraException(
                        $"The count of restore points is {backupJob.GetNewRestorePoints().Count}, you can't remove all of them!");
                }

                var counter = 0;
                var restorePointsToDelete2 = new List<RestorePoint>();
                foreach (var restorePoint in backupJob.GetNewRestorePoints())
                {
                    if (counter == backupJob.RemoveCountCheck)
                    {
                        break;
                    }

                    restorePointsToDelete2.Add(restorePoint);
                    ++counter;
                }

                resultRemoveList = restorePointsToDelete;
                foreach (var restorePoint1 in restorePointsToDelete)
                {
                    foreach (var restorePoint2 in restorePointsToDelete2)
                    {
                        if (restorePoint1 != restorePoint2)
                        {
                            resultRemoveList.Add(restorePoint2);
                        }
                    }
                }
            }

            if (resultRemoveList.Count == backupJob.GetNewRestorePoints().Count)
            {
                throw new BackupsExtraException(
                    $"The count of restore points is {backupJob.GetNewRestorePoints().Count}, you can't remove all of them!");
            }

            return restorePointRemover.RemoveRestorePoint(backupJob, resultRemoveList, isTimecodeOn);
        }
    }
}