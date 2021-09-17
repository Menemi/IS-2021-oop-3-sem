using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber
    {
        private int _course;

        public CourseNumber(int course)
        {
            if (course < 1 || course > 4)
            {
                throw new InvalidCourseNumberIsuException();
            }

            _course = course;
        }
    }
}