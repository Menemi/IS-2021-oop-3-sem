using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Isu.Tools;
using IsuExtra.Exceptions;

namespace IsuExtra
{
    public class IsuService
    {
        private List<JointGroupOfTrainingDirections> _listJgtd = new List<JointGroupOfTrainingDirections>();

        private List<ComplementedGroup> _listGroup = new List<ComplementedGroup>();

        public ComplementedGroup AddGroup(string name, List<Class> timetable, MegaFaculty megaFaculty)
        {
            var newGroup = new ComplementedGroup(name, timetable, megaFaculty);
            _listGroup.Add(newGroup);
            return newGroup;
        }

        public ComplementedStudent AddStudent(ComplementedGroup group, string name)
        {
            var newStudent = new ComplementedStudent(name, group);
            group.AddStudent(newStudent);
            return newStudent;
        }

        public JointGroupOfTrainingDirections AddJgtd(MegaFaculty megaFaculty, List<Stream> streams)
        {
            if (streams.Any(stream => stream.MegaFaculty != megaFaculty))
            {
                throw new WrongMegaFacultyException();
            }

            var newJgtd = new JointGroupOfTrainingDirections(megaFaculty, streams);
            _listJgtd.Add(newJgtd);
            return newJgtd;
        }

        public List<Stream> GetStreamsByCourse(MegaFaculty megaFaculty)
        {
            foreach (var jgtd in _listJgtd
                .Where(jgtd => jgtd.GetMegaFaculty() == megaFaculty))
            {
                return jgtd.GetStreams().ToList();
            }

            throw new MegaFacultyDoesNotExcist();
        }

        public ReadOnlyCollection<ComplementedStudent> GetStudentsByStream(Stream stream)
        {
            return stream.GetStudents();
        }

        public List<ComplementedStudent> GetNotSignedUpByGroup(ComplementedGroup group)
        {
            return group.GetNotSignedUp();
        }
    }
}