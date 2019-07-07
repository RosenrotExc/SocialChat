using System;

namespace SocialChat.Domain.Core.DTO.Users
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
