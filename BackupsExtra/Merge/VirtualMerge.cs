using System.Collections.Generic;
using System.Linq;
using Backups;
using BackupsExtra.Exception;

namespace BackupsExtra.Merge
{
    public class VirtualMerge : IMergeProcessMethod
    {
        public RestorePoint Merge(
            ComplementedBackupJob backupJobOld,
            ComplementedBackupJob backupJobNew,
            RestorePoint oldRestorePoint,
            RestorePoint newRestorePoint,
            bool isTimecodeOn)
        {
            if (!backupJobOld.GetNewRestorePoints().Contains(oldRestorePoint) &&
                !backupJobNew.GetNewRestorePoints().Contains(newRestorePoint))
            {
                throw new BackupsExtraException("One or more restore points are not contained in any backup job");
            }

            if (oldRestorePoint.GetRepositories().Count == 1 ||
                newRestorePoint.GetRepositories().Count == 1)
            {
                if (oldRestorePoint.GetRepositories()[0].GetStorageList().Count != 1 ||
                    newRestorePoint.GetRepositories()[0].GetStorageList().Count != 1)
                {
                    if (backupJobOld.GetRestorePoints().Contains(oldRestorePoint))
                    {
                        RemoveOldRestorePoint(backupJobOld, oldRestorePoint, isTimecodeOn);
                        return newRestorePoint;
                    }

                    RemoveOldRestorePoint(backupJobNew, oldRestorePoint, isTimecodeOn);
                    return newRestorePoint;
                }
            }

            var newRepositories = new List<Repository>();
            var oldStorageList = oldRestorePoint.GetRepositories()
                .SelectMany(oldRepository => oldRepository.GetStorageList()).ToList();

            var newStorageList = newRestorePoint.GetRepositories()
                .SelectMany(newRepository => newRepository.GetStorageList()).ToList();

            foreach (var oldFile in oldStorageList)
            {
                var checkingAvailabilityFlag = false;
                var repository = new Repository();

                foreach (var newFile in newStorageList.Where(newFile => oldFile.Name == newFile.Name))
                {
                    repository.AddStorage(newFile);
                    newRepositories.Add(repository);
                    checkingAvailabilityFlag = true;
                }

                if (!checkingAvailabilityFlag)
                {
                    repository.AddStorage(oldFile);
                    newRepositories.Add(repository);
                }
            }

            newRestorePoint.RemoveRepositories(newRestorePoint.GetRepositories().ToList());

            RemoveOldRestorePoint(backupJobOld, oldRestorePoint, isTimecodeOn);
            foreach (var repository in newRepositories)
            {
                newRestorePoint.AddRepository(repository);
            }

            return newRestorePoint;
        }

        private void RemoveOldRestorePoint(
            ComplementedBackupJob backupJob,
            RestorePoint oldRestorePoint,
            bool isTimecodeOn)
        {
            var restorePointsToRemove = new List<RestorePoint> { oldRestorePoint };
            backupJob.RemoveRestorePoints(restorePointsToRemove, isTimecodeOn);
        }
    }
}