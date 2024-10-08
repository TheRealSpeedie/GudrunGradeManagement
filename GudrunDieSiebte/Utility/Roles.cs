namespace GudrunDieSiebte.Utility
{
    public class Roles
    {
        public enum RoleType
        {
            Admin,
            Guest,
            Student,
            Teacher
        }

        public static string getRoleString(RoleType role)
        {
            switch(role)
            {
                case RoleType.Admin:
                    return "Admin";
                case RoleType.Guest:
                    return "Guest";
                case RoleType.Student:
                    return "Student";
                case RoleType.Teacher:
                    return "Teacher";
                default:
                    return null;
            }
        }
    }
}
