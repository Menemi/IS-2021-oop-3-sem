using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups;
using BackupsExtra.Exception;

namespace BackupsExtra.Merge
{
    public class LocalMerge : IMergeProcessMethod
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

            var oldFiles = new List<FileInfo>();
            foreach (var oldRepository in oldRestorePoint.GetRepositories())
            {
                oldFiles.AddRange(oldRepository.GetStorageList());
            }

            var newFiles = new List<FileInfo>();
            foreach (var newRepository in newRestorePoint.GetRepositories())
            {
                newFiles.AddRange(newRepository.GetStorageList());
            }

            var newRepositories = new List<Repository>();
            foreach (var oldFile in oldFiles)
            {
                var oldFileInfo = new FileInfo(oldFile.FullName);
                var checkingAvailabilityFlag = false;
                var repository = new Repository();

                foreach (var newFile in newFiles)
                {
                    if (oldFile.Name == newFile.Name)
                    {
                        var newFileInfo = new FileInfo(newFile.FullName);
                        repository.AddStorage(newFileInfo);
                        newRepositories.Add(repository);
                        checkingAvailabilityFlag = true;
                    }
                }

                if (!checkingAvailabilityFlag)
                {
                    repository.AddStorage(oldFileInfo);
                    newRepositories.Add(repository);
                }
            }

            var repositoriesToDelete = newRestorePoint.GetRepositories();
            newRestorePoint.RemoveRepositories(repositoriesToDelete.ToList());
            var directoryToDelete = new DirectoryInfo($"{newRestorePoint.Path}{newRestorePoint.Id}");
            foreach (var file in directoryToDelete.GetFiles())
            {
                file.Delete();
            }

            var id = 0;
            foreach (var repository in newRepositories)
            {
                newRestorePoint.AddRepository(repository);
                var tempDirectory =
                    new DirectoryInfo(Path.Combine($"{newRestorePoint.Path}{newRestorePoint.Id}", "temp"));
                tempDirectory.Create();

                foreach (var fileToAdd in repository.GetStorageList())
                {
                    fileToAdd.CopyTo(Path.Combine(tempDirectory.FullName, fileToAdd.Name));
                }

                ZipFile.CreateFromDirectory(
                    tempDirectory.FullName,
                    Path.Combine($"{newRestorePoint.Path}{newRestorePoint.Id}", $"Files_{++id}.zip"));
                tempDirectory.Delete(true);
            }

            RemoveOldRestorePoint(backupJobOld, oldRestorePoint, isTimecodeOn);
            return newRestorePoint;
        }

        private void RemoveOldRestorePoint(
            ComplementedBackupJob backupJob,
            RestorePoint oldRestorePoint,
            bool isTimecodeOn)
        {
            var restorePointsToRemove = new List<RestorePoint> { oldRestorePoint };
            var restorePointDirectoryToRemove =
                new DirectoryInfo($"{oldRestorePoint.Path}{oldRestorePoint.Id}");

            restorePointDirectoryToRemove.Delete(true);
            backupJob.RemoveRestorePoints(restorePointsToRemove, isTimecodeOn);
        }
    }
}