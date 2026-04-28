using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace befit.Models
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Czas rozpoczęcia")]
        public DateTime Start { get; set; }

        [Required]
        [Display(Name = "Czas zakończenia")]
        public DateTime End { get; set; }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}