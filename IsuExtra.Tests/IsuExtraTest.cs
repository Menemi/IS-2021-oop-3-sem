using System.Collections.Generic;
using System.Collections.ObjectModel;
using Isu.Services;
using IsuExtra;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class Tests
    {
        private IsuService _isuService;
        private List<Class> _mainTimetable;
        private List<Class> _cybersecurityFirstStream;

        [SetUp]
        public void Setup()
        {
            _isuService = new IsuService();
            _mainTimetable = new List<Class>()
            {
                new Class("oop", new WeekDayTime(WeekDay.Monday, ClassTime.First),
                    "Nosovizkiy", 403),
                new Class("math", new WeekDayTime(WeekDay.Monday, ClassTime.Second),
                    "Vozianova", 331),
                new Class("probability theory", new WeekDayTime(WeekDay.Tuesday, ClassTime.Second),
                    "SuSlina", 0),
                new Class("oc", new WeekDayTime(WeekDay.Tuesday, ClassTime.Third),
                    "Mayatin", 466),
                new Class("oop", new WeekDayTime(WeekDay.Tuesday, ClassTime.Fourth),
                    "Blashenkov", 151)
            };
            _cybersecurityFirstStream = new List<Class>()
            {
                new Class("cybersecurity", new WeekDayTime(WeekDay.Wednesday, ClassTime.Zero),
                    "Savkov", 212),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Wednesday, ClassTime.First),
                    "Savkov", 212),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Wednesday, ClassTime.Second),
                    "Budko", 306),
                new Class("software methods and tools", new WeekDayTime(WeekDay.Wednesday, ClassTime.Fifth),
                    "Klimenkov", 0)
            };
        }

        [Test]
        public void AddAndRemoveTheJgtd()
        {
            var streams = new List<Stream>()
            {
                new Stream(MegaFaculty.KTU, _cybersecurityFirstStream)
            };
            
            var cybersecurity = _isuService.AddJgtd(MegaFaculty.KTU, streams);
            var m3204 = _isuService.AddGroup("M3204", _mainTimetable, MegaFaculty.TINT);
            var danya = _isuService.AddStudent(m3204, "danya");
            
            streams[0].AddStudent(danya);
            List<ComplementedStudent> studentsAfterAdd = new List<ComplementedStudent>(streams[0].GetStudents());
            streams[0].RemoveStudent(danya);
            
            // var newList = new List<ComplementedStudent>();
            // newList.Add(danya);
            
            Assert.AreEqual(studentsAfterAdd[0], danya);
            Assert.AreEqual(new List<ComplementedStudent>(), streams[0].GetStudents());
        }

        [Test]
        public void GetStreamsByCourse()
        {
            var cybersecuritySecondStream = new List<Class>()
            {
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassTime.Zero),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassTime.First),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassTime.Second),
                    "Budko", 302),
                new Class("software methods and tools", new WeekDayTime(WeekDay.Wednesday, ClassTime.Fifth),
                    "Klimenkov", 0)
            };

            var innovativeMarketingFirstStream = new List<Class>()
            {
                new Class("basic int property", new WeekDayTime(WeekDay.Wednesday, ClassTime.Second),
                    "Ivanov", 111),
                new Class("basic int property", new WeekDayTime(WeekDay.Saturday, ClassTime.Zero),
                    "Sidorov", 222),
                new Class("basic int property", new WeekDayTime(WeekDay.Saturday, ClassTime.First),
                    "Petrov", 333),
                new Class("innovative marketing", new WeekDayTime(WeekDay.Saturday, ClassTime.Second),
                    "Lapenko", 444),
                new Class("innovative marketing", new WeekDayTime(WeekDay.Saturday, ClassTime.Third),
                    "Blud", 555),
                new Class("innovative marketing", new WeekDayTime(WeekDay.Saturday, ClassTime.Fourth),
                    "Potapov", 666)
            };

            var cybersecurityStreams = new List<Stream>()
            {
                new Stream(MegaFaculty.KTU, _cybersecurityFirstStream),
                new Stream(MegaFaculty.KTU, cybersecuritySecondStream)
            };

            var innovativeMarketingStreams = new List<Stream>()
            {
                new Stream(MegaFaculty.FTMI, innovativeMarketingFirstStream)
            };

            var cybersecurity = _isuService.AddJgtd(MegaFaculty.KTU, cybersecurityStreams);
            var innovativeMarketing = _isuService.AddJgtd(MegaFaculty.FTMI, innovativeMarketingStreams);

            Assert.AreEqual(cybersecurityStreams, _isuService.GetStreamsByCourse(MegaFaculty.KTU));
        }

        [Test]
        public void GetStudentsByStream()
        {
            var cybersecuritySecondStream = new List<Class>()
            {
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassTime.Zero),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassTime.First),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassTime.Second),
                    "Budko", 302),
                new Class("software methods and tools", new WeekDayTime(WeekDay.Wednesday, ClassTime.Fifth),
                    "Klimenkov", 0)
            };

            var firstStream = new Stream(MegaFaculty.KTU, _cybersecurityFirstStream);
            var secondStream = new Stream(MegaFaculty.KTU, cybersecuritySecondStream);

            var streams = new List<Stream>()
            {
                firstStream,
                secondStream
            };

            var cybersecurity = _isuService.AddJgtd(MegaFaculty.KTU, streams);
            var m3204 = _isuService.AddGroup("M3204", _mainTimetable, MegaFaculty.TINT);
            var m3201 = _isuService.AddGroup("M3201", _mainTimetable, MegaFaculty.TINT);
            var danya = _isuService.AddStudent(m3204, "danya");
            var alex = _isuService.AddStudent(m3204, "alex");
            var misha = _isuService.AddStudent(m3201, "misha");
            streams[0].AddStudent(danya);
            streams[1].AddStudent(alex);
            streams[0].AddStudent(misha);
            
            var students = new List<ComplementedStudent>()
            {
                danya,
                misha
            };
            
            Assert.AreEqual(students, _isuService.GetStudentsByStream(firstStream));
        }

        [Test]
        public void GetNotSignedUpByGroup()
        {
            var firstStream = new Stream(MegaFaculty.KTU, _cybersecurityFirstStream);
            
            var streams = new List<Stream>()
            {
                firstStream
            };
            
            var cybersecurity = _isuService.AddJgtd(MegaFaculty.KTU, streams);
            var m3204 = _isuService.AddGroup("M3204", _mainTimetable, MegaFaculty.TINT);
            var danya = _isuService.AddStudent(m3204, "danya");
            var alex = _isuService.AddStudent(m3204, "alex");
            var misha = _isuService.AddStudent(m3204, "misha");
            streams[0].AddStudent(danya);
            streams[0].AddStudent(misha);
            
            // var newList = new List<ComplementedStudent>();
            // newList.Add(alex);
            
            Assert.AreEqual(alex, _isuService.GetNotSignedUpByGroup(m3204)[0]);
        }
    }
}