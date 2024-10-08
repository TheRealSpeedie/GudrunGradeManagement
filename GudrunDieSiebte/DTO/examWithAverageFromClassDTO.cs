using GudrunDieSiebte.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace GudrunDieSiebte.DTO
{
    public class examWithAverageFromClassDTO:ExamDTO
    {
        public double ClassSpecificAverage { get; set; }
    }
}
