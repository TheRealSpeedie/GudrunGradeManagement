using GudrunDieSiebte.Models;
using Microsoft.CodeAnalysis.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class TeacherSchoolManagementDTO : TeacherDTO
    {
        public SchoolManagementDTO SchoolManagement { get; set; }
    }
}
