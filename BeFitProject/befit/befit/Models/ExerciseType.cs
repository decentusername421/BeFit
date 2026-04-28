using System.ComponentModel.DataAnnotations;

namespace befit.Models
{
    public class ExerciseType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Nazwa ćwiczenia")]
        public string Name { get; set; }
    }
}