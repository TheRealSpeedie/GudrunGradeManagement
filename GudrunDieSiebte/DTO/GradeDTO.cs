using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public float grade { get; set; }
        public bool isConfirmed { get; set; }
       
    }
}
