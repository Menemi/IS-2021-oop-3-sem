using System;

namespace Backups
{
    public class MyFile
    {
        private string _name;

        private DateTime _creationTime;

        public MyFile(string name, DateTime creationTime)
        {
            _name = name;
            _creationTime = creationTime;
        }
    }
}