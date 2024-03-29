﻿using System;
using System.Collections.Generic;
using System.Linq;
using Backups;
using BackupsExtra.Exception;

namespace BackupsExtra.RemoveOfRestorePoints
{
    public class RemoveByDate : IRemoveRestorePoint
    {
        public List<RestorePoint> RemoveRestorePoint(
            IRestorePointRemover restorePointRemover,
            ComplementedBackupJob backupJob,
            bool isTimecodeOn)
        {
            var restorePointsToDelete = backupJob.GetNewRestorePoints()
                .Where(restorePoint => restorePoint.CreationTime.Date <= backupJob.RemoveDateCheck).ToList();

            if (restorePointsToDelete.Count == backupJob.GetNewRestorePoints().Count)
            {
                throw new BackupsExtraException(
                    $"The count of restore points is {backupJob.GetNewRestorePoints().Count}, you can't remove all of them!");
            }

            return restorePointRemover.RemoveRestorePoint(backupJob, restorePointsToDelete, isTimecodeOn);
        }
    }
}