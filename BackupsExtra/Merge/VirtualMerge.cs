using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups;

namespace BackupsExtra.Merge
{
    public class VirtualMerge : IMergeProcessMethod
    {
        public RestorePoint Merge(
            ComplementedBackupJob backupJob,
            RestorePoint oldRestorePoint,
            RestorePoint newRestorePoint,
            bool isTimecodeOn)
        {
            if (oldRestorePoint.GetRepositories().Count == 1 ||
                newRestorePoint.GetRepositories().Count == 1)
            {
                if (oldRestorePoint.GetRepositories()[0].GetStorageList().Count != 1 ||
                    newRestorePoint.GetRepositories()[0].GetStorageList().Count != 1)
                {
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

            var oldRestorePoints = new List<RestorePoint> { oldRestorePoint };
            backupJob.RemoveRestorePoints(oldRestorePoints, isTimecodeOn);
            foreach (var repository in newRepositories)
            {
                newRestorePoint.AddRepository(repository);
            }

            return newRestorePoint;
        }
    }
}