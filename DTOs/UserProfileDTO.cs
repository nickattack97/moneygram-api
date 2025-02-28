using System;
namespace moneygram_api.DTOs
{
    public class UserProfileDTO
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{FirstName} {Surname}";
    }
}

