using GudrunDieSiebte.Models;

namespace GudrunDieSiebte.Utility.ObjectClasses
{
    public class Event
    {
        public DateTime start;
        public DateTime end;
        public string title;
        public string color;

        public static Event ConvertLessonToEvent(Lesson lesson)
        {
            Event myEvent = new Event();
            myEvent.start = lesson.StartTime;
            myEvent.end = lesson.EndTime;
            myEvent.title = lesson.Modul.Name;
            myEvent.color = "#0000FF";
            return myEvent;
        }
        public static Event ConvertAppointmentToEvent(Appointment appointment)
        {
            Event myEvent = new Event();
            myEvent.start = appointment.StartTime;
            myEvent.end = appointment.EndTime;
            myEvent.title = appointment.Reason;
            myEvent.color = "#00FF00";
            return myEvent;
        }
        public static Event ConvertCourseToEvent(Course course)
        {
            Event myEvent = new Event();
            myEvent.start = course.StartTime;
            myEvent.end = course.EndTime;
            myEvent.title = course.Typ;
            myEvent.color = "#FF0000";
            return myEvent;
        }

    }
}
