using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations;

namespace GudrunDieSiebte.DTO
{
    public class PersonDTO
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

    }
}
