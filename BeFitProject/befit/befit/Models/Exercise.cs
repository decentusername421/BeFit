using System.ComponentModel.DataAnnotations;

namespace befit.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Obciążenie (kg)")]
        public int Weight { get; set; }

        [Required]
        [Range(1, 20)]
        [Display(Name = "Liczba serii")]
        public int NumOfSeries { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "Liczba powtórzeń")]
        public int NumOfReps { get; set; }

        [Required]
        [Display(Name = "Typ ćwiczenia")]
        public int ExerciseTypeId { get; set; }

        public virtual ExerciseType? ExerciseType { get; set; }

        [Required]
        [Display(Name = "Sesja treningowa")]
        public int SessionId { get; set; }

        public virtual Session? Session { get; set; }
    }
}