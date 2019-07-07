using SocialChat.Domain.Core.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace SocialChat.Domain.Core.Models.Users
{
    public class User
    {
        [Required]
        [StringLength(30, ErrorMessage = Constants.Validation.Users.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = Constants.Validation.Users.LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = Constants.Validation.Users.EmailMaxLength)]
        [EmailAddress(ErrorMessage = Constants.Validation.Users.EmailError)]
        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
