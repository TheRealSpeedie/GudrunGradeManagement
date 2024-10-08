using GudrunDieSiebte.Data;
using GudrunDieSiebte.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using YamlDotNet.Serialization;

namespace GudrunDieSiebte.Utility
{
    public class DataFactory
    {
        List<int> personIDs = new List<int>();
        List<int> managmentIDs = new List<int>();
        List<int> modulIDs = new List<int>();
        List<int> teachingsCompanyIDs = new List<int>();
        List<int> classIDs = new List<int>();
        List<int> examIDs = new List<int>();
        List<int> studentIDs = new List<int>();
        List<int> classroomsIDs = new List<int>();
        List<int> teacherIDs = new List<int>();
        List<int> courseIDs = new List<int>();
        List<int> lessonIDs = new List<int>();
        List<int> absenceIDs = new List<int>();
        public List<IdentityUser> names = new List<IdentityUser>();
        Random random = new Random();
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly SignInManager<IdentityUser> _signInManager;

        List<List<int>> history = new List<List<int>>();
        List<object> grades = new List<object>();

        public DataFactory(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _rolemanager = roleManager;
            _usermanager = userManager;
            _signInManager = signInManager;
        }
        public async Task GenerateData()
        {
            await DeleteData();
            await CreateRoles();
            await CreateHildegard();
            CreateSchoolManagement(1);
            GetPrimaryKeys("management");
            CreateTeachingCompany(30);
            GetPrimaryKeys("company");
            await CreatePerson(50);
            GetPrimaryKeys("person");
            CreateClassRoom(10);
            GetPrimaryKeys("classroom");
            CreateModul(10);
            GetPrimaryKeys("modul");
            CreateCourse(50);
            GetPrimaryKeys("course");
            CreateAppointment(15);
            CreateClassCourse(20);
            GetPrimaryKeys("student");
            CreateExam(50);
            CreateLesson(1500);
            GetPrimaryKeys("lesson");
            CreateAbsence(200);
            GetPrimaryKeys("absence");
            CreateSetting(11);
            CreateNotification(20);
            GetPrimaryKeys("student");
            GetPrimaryKeys("class");
            CreateToDos(3);
        }
        private async Task DeleteData()
        {
            var StudentTODO = _context.StudentToDo.ToList();
            _context.StudentToDo.RemoveRange(StudentTODO);
            var Todo = _context.ToDo.ToList();
            _context.ToDo.RemoveRange(Todo);
            var AbsenceLesson = _context.AbsenceLesson.ToList();
            _context.AbsenceLesson.RemoveRange(AbsenceLesson);
            var AbsencePerson = _context.AbsencePerson.ToList();
            _context.AbsencePerson.RemoveRange(AbsencePerson);
            var student = _context.Student.ToList();
            _context.Student.RemoveRange(student);
            var exam = _context.Exam.ToList();
            _context.Exam.RemoveRange(exam);
            var TeachingCompany = _context.TeachingCompany.ToList();
            _context.TeachingCompany.RemoveRange(TeachingCompany);
            var Notification = _context.Notification.ToList();
            _context.Notification.RemoveRange(Notification);
            var Lesson = _context.Lesson.ToList();
            _context.Lesson.RemoveRange(Lesson);
            var classCourse = _context.ClassCourse.ToList();
            _context.ClassCourse.RemoveRange(classCourse);
            var Class = _context.Class.ToList();
            _context.Class.RemoveRange(Class);
            var course = _context.Course.ToList();
            _context.Course.RemoveRange(course);
            var Teacher = _context.Teacher.ToList();
            _context.Teacher.RemoveRange(Teacher);
            var SchoolManagement = _context.SchoolManagement.ToList();
            _context.SchoolManagement.RemoveRange(SchoolManagement);
            var Person = _context.Person.ToList();
            _context.Person.RemoveRange(Person);
            var Modul = _context.Modul.ToList();
            _context.Modul.RemoveRange(Modul);
            var ClassRoom = _context.ClassRoom.ToList();
            _context.ClassRoom.RemoveRange(ClassRoom);
            var UserRoles = _context.UserRoles.ToList();
            _context.UserRoles.RemoveRange(UserRoles);
            teacherIDs.Clear();
            _context.SaveChanges();
            var User = _usermanager.Users.ToList();
            foreach (var user in User)
            {
                await _usermanager.DeleteAsync(user);
            }
            var Userroles = _rolemanager.Roles.ToList();
            foreach (var role in Userroles)
            {
                await _rolemanager.DeleteAsync(role);
            }
        }



        private void GetPrimaryKeys(string thing)
        {
            switch (thing)
            {
                case "person":
                    personIDs = (from person in _context.Person.ToList<Person>() select person.Id).ToList();
                    Debug.WriteLine("GetPersonIds = " + personIDs.Count());
                    break;
                case "management":
                    var managments = _context.SchoolManagement.ToList<SchoolManagement>();
                    foreach (SchoolManagement managment in managments)
                    {
                        managmentIDs.Add(managment.Id);
                    }
                    break;
                case "modul":
                    var modules = _context.Modul.ToList<Modul>();
                    foreach (Modul modul in modules)
                    {
                        modulIDs.Add(modul.Id);
                    }
                    break;
                case "class":
                    var classes = _context.Class.ToList<Class>();
                    foreach (Class class1 in classes)
                    {
                        classIDs.Add(class1.Id);
                    }
                    break;
                case "company":
                    var companys = _context.TeachingCompany.ToList<TeachingCompany>();
                    foreach (TeachingCompany company in companys)
                    {
                        teachingsCompanyIDs.Add(company.Id);
                    }
                    break;
                case "exam":
                    var exams = _context.Exam.ToList<Exam>();
                    foreach (Exam exam in exams)
                    {
                        examIDs.Add(exam.Id);
                    }
                    break;
                case "student":
                    var students = _context.Student.ToList<Student>();
                    foreach (Student student in students)
                    {
                        studentIDs.Add(student.Id);
                    }
                    break;
                case "teacher":
                    var teachers = _context.Teacher.ToList<Teacher>();
                    foreach (Teacher teacher in teachers)
                    {
                        teacherIDs.Add(teacher.Id);
                    }
                    break;
                case "classroom":
                    var classroom = _context.ClassRoom.ToList<ClassRoom>();
                    foreach (ClassRoom c in classroom)
                    {
                        classroomsIDs.Add(c.Id);
                    }
                    break;
                case "course":
                    var course = _context.Course.ToList<Course>();
                    foreach (Course Course in course)
                    {
                        courseIDs.Add(Course.Id);
                    }
                    break;
                case "lesson":
                    var lesson = _context.Lesson.ToList<Lesson>();
                    foreach (Lesson l in lesson)
                    {
                        lessonIDs.Add(l.Id);
                    }
                    break;
                case "absence":
                    var absence = _context.Absence.ToList<Absence>();
                    foreach (Absence a in absence)
                    {
                        absenceIDs.Add(a.Id);
                    }
                    break;
            }
        }
        private bool GetRandomBool()
        {
            int number = random.Next(0, 100);
            if ((number % 2) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private async Task AssignRoleByPersonID(int personIndex, Roles.RoleType roleType)
        {
            var userId = _context.Person.Where(p => p.Id == personIndex).Select(p => p.fk_user).First();
            var user = await _usermanager.FindByIdAsync(userId);
            if (user == null)
            {
                Debug.WriteLine("userId = " + userId);
                Debug.WriteLine("user = " + user);
                Debug.WriteLine("personIndex = " + personIndex);
                Debug.WriteLine("personIDS = " + personIDs.Count());
            }
            else
            {
                await AssignRole(user, roleType);
            }
        }
        private async Task AssignRole(IdentityUser user, Roles.RoleType roleType)
        {
            var role = Roles.getRoleString(roleType);
            var resultuser = await _usermanager.AddToRoleAsync(user, role);
        }
        private string GetModulName()
        {
            var number = Faker.RandomNumber.Next(100, 1000);
            var letter = "M";
            var modulname = letter + number.ToString();
            return modulname;
        }
        private DateTime RandomDate()
        {
            DateTime start = new DateTime(2022, 1, 1);
            int range = (DateTime.Today - start).Days * 2;
            return start.AddDays(random.Next(range));
        }
        private DateTime RandomBirthdayDate()
        {
            DateTime start = new DateTime(1950, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
        public List<object> GetHistory()
        {
            return grades;
        }
        private List<int> ClonePersonIds()
        {
            List<int> result = new List<int>(personIDs.Count);
            foreach (int id in personIDs)
            {
                result.Add(id);
            }
            return result;
        }
        private DateTime RandomTime()
        {
            DateTime Date = RandomDate();
            int minutes = random.Next(0, 60);
            int hours = random.Next(8, 17);
            DateTime Datemin = Date.AddMinutes(minutes);
            DateTime Datefinal = Datemin.AddHours(hours);
            return Datefinal;
        }
        private string GetOneRandomWord()
        {
            var words = Faker.Lorem.Words(2);
            return words.First();
        }
        private string GeneratePassword(string name)
        {
            return name + "!123";
        }



        private async Task CreateRoles()
        {
            foreach (Roles.RoleType roleType in Enum.GetValues(typeof(Roles.RoleType)))
            {
                string roleName = Roles.getRoleString(roleType);
                bool roleExist = await _rolemanager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await _rolemanager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
        private async Task CreateHildegard()
        {
            string path = @"passwordFile\gudrun_pw.yaml";
            string yamlString = File.ReadAllText(path);
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize<Dictionary<string, string>>(yamlString);
            var password = yamlObject["Passwort"];
            var email = "aileen.koesters03@gmail.com";
            IdentityUser user = new IdentityUser(email);
            user.Email = email;
            user.PasswordHash = _usermanager.PasswordHasher.HashPassword(user, password);
            user.EmailConfirmed = true;
            await _usermanager.CreateAsync(user);
            await AssignRole(user, Roles.RoleType.Admin);
            _context.SaveChanges();
            await _signInManager.SignInAsync(user, isPersistent: false);
        }
        private async Task CreatePerson(int x)
        {
            for (int y = 0; y <= x; y++)
            {
                var LastName = Faker.Name.Last();
                var FirstName = Faker.Name.First();
                var password = GeneratePassword(FirstName);
                var username = FirstName + "_" + LastName;
                string allowableLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_";
                foreach (char c in username)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                    {
                        username = username.Replace(c.ToString(), "");
                    }
                }
                var email = username + "@gudrun.ch";
                IdentityUser user = new IdentityUser(email);
                user.Email = email;
                user.PasswordHash = _usermanager.PasswordHasher.HashPassword(user, password);
                user.EmailConfirmed = true;
                IdentityResult result = await _usermanager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    Console.WriteLine(result);
                    names.Add(user);
                }
                _context.SaveChanges();
                var teacherbool = true;
                if (y > 3)
                {
                    teacherbool = false;
                }
                Person person = new Person
                {
                    LastName = LastName,
                    FirstName = FirstName,
                    Birthday = RandomBirthdayDate(),
                    Address = Faker.Address.StreetName(),
                    City = Faker.Address.City(),
                    isTeacher = teacherbool,
                    fk_user = user.Id
                };
                _context.Add(person);
                _context.SaveChanges();
                if (teacherbool)
                {
                    await CreateTeacher(person);
                }
                else
                {
                    if (y == 4)
                    {
                        GetPrimaryKeys("teacher");
                        CreateClass(5);
                        GetPrimaryKeys("class");
                    }
                    await CreateStudent(person);
                }
            }
        }
        private void CreateTeachingCompany(int x)
        {
            for (int y = 0; y < x; y++)
            {
                var name = Faker.Company.Name();
                var adresse = Faker.Address.StreetName();
                var city = Faker.Address.City();
                string allowableLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                foreach (char c in name)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                    {
                        name = name.Replace(c.ToString(), "");
                    }
                }
                foreach (char c in adresse)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                    {
                        adresse = adresse.Replace(c.ToString(), "");
                    }
                }
                foreach (char c in city)
                {
                    if (!allowableLetters.Contains(c.ToString()))
                    {
                        city = city.Replace(c.ToString(), "");
                    }
                }

                TeachingCompany teachingCompany = new TeachingCompany
                {
                    Name = name,
                    Address = adresse,
                    City = city
                };
                _context.Add(teachingCompany);
            }
            _context.SaveChanges();
        }
        private void CreateSchoolManagement(int x)
        {
            for (int y = 0; y < x; y++)
            {
                SchoolManagement schoolManagement = new SchoolManagement
                {
                    SchoolName = Faker.Company.Name()
                };
                _context.Add(schoolManagement);
            }
            _context.SaveChanges();
        }
        private void CreateClassRoom(int x)
        {
            for (int y = 0; y < x; y++)
            {
                ClassRoom classRoom = new ClassRoom
                {
                    RoomNumber = Faker.RandomNumber.Next(0, 100),
                    Floor = Faker.RandomNumber.Next(0, 10)
                };
                _context.Add(classRoom);
            }
            _context.SaveChanges();
        }
        private void CreateModul(int x)
        {
            for (int y = 0; y < x; y++)
            {
                Modul modul = new Modul
                {
                    Name = GetModulName(),
                    description = Faker.Lorem.Paragraph(2),
                    Shedule = Faker.Lorem.Paragraph()
                };
                _context.Add(modul);
            }
            _context.SaveChanges();
        }
        private void CreateCourse(int x)
        {
            for (int y = 0; y < x; y++)
            {
                DateTime start = RandomTime();
                var ModulIndex = random.Next(modulIDs.Count);
                Course course = new Course
                {
                    Typ = Faker.Name.First(),
                    StartTime = start,
                    EndTime = start.AddMinutes(45),
                    modulId = modulIDs[ModulIndex]
                };
                _context.Add(course);
            }
            _context.SaveChanges();
        }
        private void CreateClassCourse(int x)
        {
            for (int y = 0; y < x; y++)
            {
                var ClassIndex = random.Next(classIDs.Count);
                var CourseIndex = random.Next(courseIDs.Count);

                ClassCourse classCourse = new ClassCourse
                {
                    fk_Class = classIDs[ClassIndex],
                    fk_Course = courseIDs[CourseIndex],
                };
                _context.Add(classCourse);
            }
            _context.SaveChanges();
        }
        private async Task CreateTeacher(Person person)
        {

            var PersonIndex = person.Id;
            await AssignRoleByPersonID(PersonIndex, Roles.RoleType.Teacher);
            var SchoolManagementIndex = random.Next(managmentIDs.Count);
            Teacher teacher = new Teacher
            {
                HireDate = RandomDate(),
                fk_Person = PersonIndex,
                fk_SchoolManagement = managmentIDs[SchoolManagementIndex]
            };
            history.Add(ClonePersonIds());
            _context.Add(teacher);
            _context.SaveChanges();
        }
        private void CreateClass(int x)
        {
            List<char> letters = new List<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            for (int y = 0; y < x; y++)
            {
                var TeacherIndex = random.Next(teacherIDs.Count);
                var number = Faker.RandomNumber.Next(1, 30);
                var letter = random.Next(letters.Count);
                var classname = number.ToString() + letters[letter];
                Class class1 = new Class
                {
                    Name = classname,
                    fk_teacher = teacherIDs[TeacherIndex]
                };
                teacherIDs.Remove(TeacherIndex);
                _context.Add(class1);
            }
            _context.SaveChanges();
        }
        private void CreateGrade(int x, Exam exam)
        {
            for (int y = 0; y < x; y++)
            {
                var StudentIndex = random.Next(studentIDs.Count);
                var highscore = exam.Highscore;
                var diff = 0;
                if (highscore > 30)
                {
                    diff = Faker.RandomNumber.Next(1, 30);
                }
                else
                {
                    diff = Faker.RandomNumber.Next(1, 9);
                }
                var score = highscore - diff;
                float grade = ((float)score / highscore * 5) + 1;
                grades.Add(grade);
                Grade grad = new Grade
                {
                    Score = score,
                    isConfirmed = GetRandomBool(),
                    grade = grade,
                    fk_Student = studentIDs[StudentIndex],
                    fk_Exam = exam.Id,

                };
                _context.Add(grad);
            }
            _context.SaveChanges();
        }
        private void CreateToDos(int x)
        {
            foreach (var c in studentIDs)
            {
                for (int y = 0; y < x; y++)
                {
                    var TeacherIndex = random.Next(teacherIDs.Count);
                    var ModulIndex = random.Next(modulIDs.Count);
                    var ClassIndex = random.Next(classIDs.Count);
                    ToDo todo = new ToDo
                    {
                        Task = GetOneRandomWord(),
                        UpTo = RandomDate(),
                        fk_Modul = modulIDs[ModulIndex],
                        fk_Teacher = teacherIDs[TeacherIndex],
                        fk_Class = classIDs[ClassIndex]
                    };
                    _context.Add(todo);
                    _context.SaveChanges();
                    CreateStudentToDo(todo, c);
                }
            }

        }
        private void CreateStudentToDo(ToDo todo, int studentID)
        {
            StudentToDo studentToDo = new StudentToDo
            {
                Done = GetRandomBool(),
                fk_Student = studentID,
                fk_ToDo = todo.Id
            };
            _context.Add(studentToDo);
            _context.SaveChanges();
        }
        private void CreateExam(int x)
        {
            for (int y = 0; y < x; y++)
            {
                var ModulIndex = random.Next(modulIDs.Count);
                var highscore = Faker.RandomNumber.Next(10, 100);
                Exam exam = new Exam
                {
                    Highscore = highscore,
                    name = GetOneRandomWord(),
                    Thema = Faker.Lorem.Sentence(),
                    fk_Modul = modulIDs[ModulIndex],
                    Examday = RandomDate()
                };
                _context.Add(exam);
                _context.SaveChanges();
                CreateGrade(10, exam);
            }
        }
        private async Task CreateStudent(Person person)
        {
            var PersonIndex = person.Id;
            await AssignRoleByPersonID(PersonIndex, Roles.RoleType.Student);
            var ClassIndex = random.Next(classIDs.Count);
            var CompanyIndex = random.Next(teachingsCompanyIDs.Count);
            Student student = new Student
            {
                EnrollmentDate = RandomDate(),
                fk_Person = PersonIndex,
                fk_Class = classIDs[ClassIndex],
                fk_TeachingCompany = teachingsCompanyIDs[CompanyIndex]
            };
            history.Add(ClonePersonIds());
            _context.Add(student);
            _context.SaveChanges();
        }
        private void CreateLesson(int x)
        {
            DateTime start = RandomTime();
            for (int y = 0; y < x; y++)
            {
                var ClassRoomIndex = random.Next(classroomsIDs.Count);
                var ClassIndex = random.Next(classIDs.Count);
                var ModulIndex = random.Next(modulIDs.Count);
                var TeacherIndex = random.Next(teacherIDs.Count);
                if ((y % 5) == 0)
                {
                    start = RandomTime();
                }
                else
                {
                    start = start.AddDays(1);
                }
                Lesson lesson = new Lesson
                {
                    StartTime = start,
                    EndTime = start.AddMinutes(60),
                    fk_ClassRoom = classroomsIDs[ClassRoomIndex],
                    fk_Class = classIDs[ClassIndex],
                    fk_Modul = modulIDs[ModulIndex],
                    fk_Teacher = teacherIDs[TeacherIndex]
                };
                _context.Add(lesson);
            }
            _context.SaveChanges();
        }
        private void CreateAppointment(int x)
        {
            for (int y = 0; y < x; y++)
            {
                DateTime start = RandomTime();
                var PersonIndex = random.Next(personIDs.Count);
                Appointment appointment = new Appointment
                {
                    StartTime = start,
                    EndTime = start.AddMinutes(60),
                    Reason = Faker.Lorem.Paragraph(),
                    fk_Person = personIDs[PersonIndex]
                };
                _context.Add(appointment);
            }
            _context.SaveChanges();
        }
        private void CreateSetting(int x)
        {
            for (int y = 0; y < x; y++)
            {
                var PersonIndex = random.Next(personIDs.Count);
                Setting setting = new Setting
                {
                    fk_Person = personIDs[PersonIndex]
                };
                _context.Add(setting);
            }
            _context.SaveChanges();
        }
        private void CreateAbsence(int x)
        {
            for (int y = 0; y < x; y++)
            {
                var PersonIndex = random.Next(personIDs.Count);
                DateTime start = RandomTime();
                Absence absence = new Absence
                {
                    Reason = Faker.Lorem.Paragraph(),
                    fk_Person = personIDs[PersonIndex],
                    StartTime = start,
                    EndTime = start.AddDays(1),
                    confirmed = GetRandomBool()
                };
                _context.Add(absence);
                _context.SaveChanges();
                CreateAbsenceLesson(absence);
            }
        }
        private void CreateNotification(int x)
        {
            for (int y = 0; y < x; y++)
            {
                var ClassIndex = random.Next(classIDs.Count);
                var TeacherIndex = random.Next(teacherIDs.Count);
                var ModulIndex = random.Next(modulIDs.Count);

                Notification notification = new Notification
                {
                    Type = (Notification.Types)Faker.RandomNumber.Next(1, 4 + 1),
                    Message = Faker.Lorem.Sentence(2),
                    fk_Class = classIDs[ClassIndex],
                    fk_Teacher = teacherIDs[TeacherIndex],
                    fk_Modul = modulIDs[ModulIndex],
                    CreateTime = RandomTime()
                };
                _context.Add(notification);
            }
            _context.SaveChanges();
        }
        public void GenerateLessons(int x)
        {
            GetPrimaryKeys("classroom");
            GetPrimaryKeys("class");
            GetPrimaryKeys("modul");
            GetPrimaryKeys("teacher");
            for (int y = 0; y < x; y++)
            {
                var ClassRoomIndex = random.Next(classroomsIDs.Count);
                var ClassIndex = random.Next(classIDs.Count);
                var ModulIndex = random.Next(modulIDs.Count);
                var TeacherIndex = random.Next(teacherIDs.Count);

                DateTime start = RandomTime();
                Lesson lesson = new Lesson
                {
                    StartTime = start,
                    EndTime = start.AddMinutes(60),
                    fk_ClassRoom = classroomsIDs[ClassRoomIndex],
                    fk_Class = classIDs[ClassIndex],
                    fk_Modul = modulIDs[ModulIndex],
                    fk_Teacher = teacherIDs[TeacherIndex]
                };
                _context.Add(lesson);
            }
            _context.SaveChanges();
        }
        public void GenerateAppointments(int x)
        {
            GetPrimaryKeys("person");
            for (int y = 0; y < x; y++)
            {
                var PersonIndex = random.Next(personIDs.Count);
                Appointment appointment = new Appointment
                {
                    Reason = Faker.Lorem.Paragraph(),
                    fk_Person = personIDs[PersonIndex]
                };
                _context.Add(appointment);
            }
            _context.SaveChanges();
        }
        public void CreateAbsenceLesson(Absence absence)
        {
            List<Lesson> lessons = new List<Lesson>();
            if (!absence.Person.isTeacher)
            {
                var student = _context.Student.Where(t => t.fk_Person == absence.Person.Id).First();
                var cl = student.Class;
                var lessonlist = _context.Lesson.Where(l => l.StartTime <= absence.EndTime).Where(l => l.EndTime >= absence.StartTime).Where(l => l.fk_Class == cl.Id).ToList();
                lessons.AddRange(lessonlist);
            }
            foreach (var l in lessons)
            {
                AbsenceLesson absenceLesson = new AbsenceLesson
                {
                    fk_Absence = absence.Id,
                    fk_Lesson = l.Id
                };
                _context.Add(absenceLesson);
                _context.SaveChanges();
                createAbsencePerson(absence, l);
            }

        }
        public void createAbsencePerson(Absence absence, Lesson lesson)
        {
            var teacher = _context.Teacher.Where(t => t.Id == lesson.fk_Teacher).First();
            var person = _context.Person.Where(p => p.Id == teacher.fk_Person).First();
            AbsencePerson absencePerson = new AbsencePerson
            {
                isVisible = true,
                fk_Absence = absence.Id,
                fk_Person = person.Id
            };
            _context.Add(absencePerson);
            _context.SaveChanges();
        }
    }
}
