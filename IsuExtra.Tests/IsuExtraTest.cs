using System.Collections.Generic;
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
                new Class("oop", new WeekDayTime(WeekDay.Monday, ClassNumber.Second),
                    "Nosovizkiy", 403),
                new Class("math", new WeekDayTime(WeekDay.Monday, ClassNumber.Third),
                    "Vozianova", 331),
                new Class("probability theory", new WeekDayTime(WeekDay.Tuesday, ClassNumber.Third),
                    "SuSlina", 0),
                new Class("oc", new WeekDayTime(WeekDay.Tuesday, ClassNumber.Fourth),
                    "Mayatin", 466),
                new Class("oop", new WeekDayTime(WeekDay.Tuesday, ClassNumber.Fifth),
                    "Blashenkov", 151)
            };
            _cybersecurityFirstStream = new List<Class>()
            {
                new Class("cybersecurity", new WeekDayTime(WeekDay.Wednesday, ClassNumber.First),
                    "Savkov", 212),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Wednesday, ClassNumber.Second),
                    "Savkov", 212),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Wednesday, ClassNumber.Third),
                    "Budko", 306),
                new Class("software methods and tools", new WeekDayTime(WeekDay.Wednesday, ClassNumber.Sixth),
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

            Assert.AreEqual(studentsAfterAdd[0], danya);
            Assert.AreEqual(new List<ComplementedStudent>(), streams[0].GetStudents());
        }

        [Test]
        public void GetStreamsByCourse()
        {
            var cybersecuritySecondStream = new List<Class>()
            {
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassNumber.First),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassNumber.Second),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassNumber.Third),
                    "Budko", 302),
                new Class("software methods and tools", new WeekDayTime(WeekDay.Wednesday, ClassNumber.Sixth),
                    "Klimenkov", 0)
            };

            var innovativeMarketingFirstStream = new List<Class>()
            {
                new Class("basic int property", new WeekDayTime(WeekDay.Wednesday, ClassNumber.Third),
                    "Ivanov", 111),
                new Class("basic int property", new WeekDayTime(WeekDay.Saturday, ClassNumber.First),
                    "Sidorov", 222),
                new Class("basic int property", new WeekDayTime(WeekDay.Saturday, ClassNumber.Second),
                    "Petrov", 333),
                new Class("innovative marketing", new WeekDayTime(WeekDay.Saturday, ClassNumber.Third),
                    "Lapenko", 444),
                new Class("innovative marketing", new WeekDayTime(WeekDay.Saturday, ClassNumber.Fourth),
                    "Blud", 555),
                new Class("innovative marketing", new WeekDayTime(WeekDay.Saturday, ClassNumber.Fifth),
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
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassNumber.First),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassNumber.Second),
                    "Gavrichkov", 412),
                new Class("cybersecurity", new WeekDayTime(WeekDay.Saturday, ClassNumber.Third),
                    "Budko", 302),
                new Class("software methods and tools", new WeekDayTime(WeekDay.Wednesday, ClassNumber.Sixth),
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

            Assert.AreEqual(alex, _isuService.GetNotSignedUpByGroup(m3204)[0]);
        }
    }
}