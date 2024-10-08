using AutoMapper;
using GudrunDieSiebte.DTO;
using GudrunDieSiebte.Models;
using GudrunDieSiebte.Utility;
using GudrunDieSiebte.Utility.ObjectClasses;

namespace GudrunDieSiebte.Profiles
{
    public class AMProfiles : AutoMapper.Profile
    {
        public AMProfiles() 
        {
            CreateMap<Notification, NotificationDTO>();
            CreateMap<Notification, NotificationAllDTO>();
            CreateMap<Person, PersonDTO>();
            CreateMap<Teacher, TeacherDTO>();
            CreateMap<Teacher, TeacherSchoolManagementDTO>();
            CreateMap<Teacher, TeacherAllDTO>();
            CreateMap<SchoolManagement, SchoolManagementDTO>();
            CreateMap<Modul, ModulDTO>();
            CreateMap<ClassRoom, ClassRoomDTO>();
            CreateMap<Lesson, LessonDTO>();
            CreateMap<Lesson, LessonAllDTO>();
            CreateMap<Lesson, LessonWithClassModulDTO>();
            CreateMap<Class, ClassDTO>();
            CreateMap<TeachingCompany, TeachingConpanyDTO>();
            CreateMap<TeachingCompany, TeachingCompanyAllDTO>();
            CreateMap<Student, StudentDTO>();
            CreateMap<Student, StudentAllDTO>();
            CreateMap<Student, Student_ClassDTO>();
            CreateMap<Absence, AbsenceAllDTO>();
            CreateMap<Modul, ModuleAllDTO>();
            CreateMap<Exam, ExamAllDTO>();
            CreateMap<Exam, ExamDTO>();
            CreateMap<Class, ClassAllDTO>();
            CreateMap<Lesson, LessonWithModul>();
            CreateMap<Appointment, AppointmentPersonDTO>();
            CreateMap<Appointment, AppointmentDTO>();
            CreateMap<Person, PersonAppointmentDTO>();
            CreateMap<Teacher, TeacherAppointmentsPersonDTOcs>();
            CreateMap<Modul, ModulCourseDTO>();
            CreateMap<Course, CourseDTO>();
            CreateMap<ClassCourse, ClassCourseDTO>();
            CreateMap<Modul, ModulExamDTO>();
            CreateMap<Modul, ModulLessonDTO>();
            CreateMap<Modul, ModulLessonExamDTO>();
            CreateMap<Absence, AbsencePerson>();
            CreateMap<AbsenceVisibility, AbsenceAllVisibleDTO>();
            CreateMap<Absence, AbsenceDTO>();
            CreateMap<AbsenceLesson, AbsenceLessonDTO>();
            CreateMap<AbsenceWithListLesson, AbsenceWithListLessonDTO>();
            CreateMap<Absence, AbsencePersonDTO>();
            CreateMap<ModulClassExam, ModulClassExamDTO>();
            CreateMap<Modulname, ModulnameDTO>();
            CreateMap<ModulnameAverage, ModulnameAverageDTO>();
            CreateMap<ClassTeacherStudentsModuls, ClassTeacherStudentsModulsDTO>();
            CreateMap<Studentname, StudentnameDTO>();
            CreateMap<ClassModulExamList, ClassModulExamListDTO>();
            CreateMap<Exam, ExamStudentDTO>();
            CreateMap<Grade, GradesStudentDTO>();
            CreateMap<Grade, GradeDTO>();
            CreateMap<ExamGradeStudent, ExamGradeStudentDTO>();
            CreateMap<examWithAverageFromClass, examWithAverageFromClassDTO>();
            CreateMap<ModulWithClassExams, ModulWithClassExamsDTO>();
            CreateMap<ClassModul, ClassModulDTO>();
            CreateMap<Exam, ExamGradeDTO>();
            CreateMap<ClassWithStudentsGrades, ClassWithStudentsGradesDTO>();
            CreateMap<GradeWithHighscore, GradeWithHighscoreDTO>();
            CreateMap<Student, StudentWithGradelistDTO>();
            CreateMap<ToDo, ToDoDTO>();
            CreateMap<ToDoWithConfirm, ToDoWithConfirmDTO>();

        }
    }
}
