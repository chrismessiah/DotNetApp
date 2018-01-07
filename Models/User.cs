using System;
using System.ComponentModel.DataAnnotations;

/*
* Data annotations for tell EntityFramework
* how to treat the fields of each model.
*/
namespace DotNetAPI2.Models
{
    public class User
    {
        [Key] // Primary Key attribute
        public int Id { get; set; }

        [Required] // Required field attribute
        [MaxLength(50)] // heavily used for validation and will restrict strings
        public string Fullname { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public GenderClass Gender { get; set; }
    }

    public enum GenderClass
    {
        Male,
        Female,
        Other
    }
}
