using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace GudrunDieSiebte.Models
{
   public class Person
    {
        public int Id { get; set; }
        public bool isTeacher { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        [ForeignKey("IdentityUser")]
        public string? fk_user { get; set; }
        public virtual IdentityUser? User { get; set; }
        public virtual ICollection<Setting> Settings { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Absence> Absences { get; set; }
        public virtual ICollection<AbsencePerson> AbsencePerson { get; set; }
    }
}
