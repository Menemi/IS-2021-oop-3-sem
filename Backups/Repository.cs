using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Backups.Exceptions;

namespace Backups
{
    // Репозиторий (Repository) - абстракция над способом хранения бекапов. В рамках самого простого кейса,
    // репозиторием будет некоторая директория на локальной файловой системе, где будут лежать стораджи.

    // В моём случае репозиторий - абстрактное понятие, не являющееся директорией. Репозиторий лишь хранит в
    // себе лист стораджей, кажется, что это подходит под часть определения "где будут лежать стораджи"
    public class Repository
    {
        private List<FileInfo> _storageList;

        public Repository()
        {
            _storageList = new List<FileInfo>();
        }

        public ReadOnlyCollection<FileInfo> GetStorageList()
        {
            return _storageList.AsReadOnly();
        }

        public void AddStorages(List<FileInfo> files)
        {
            if (files.Any(file => file == null))
            {
                throw new BackupsException("File doesn't exist");
            }

            _storageList.AddRange(files);
        }

        public void AddStorage(FileInfo file)
        {
            if (file == null)
            {
                throw new BackupsException("File doesn't exist");
            }

            _storageList.Add(file);
        }
    }
}