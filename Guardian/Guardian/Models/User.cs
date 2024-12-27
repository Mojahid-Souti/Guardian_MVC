using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Guardian.Models
{
    public partial class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [StringLength(150, ErrorMessage = "Email Address cannot exceed 150 characters.")]
        public string EmailAddress { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", 
            ErrorMessage = "Password must contain at least one letter, one number, and one special character.")]
        public string Password { get; set; } = null!;
    }
}