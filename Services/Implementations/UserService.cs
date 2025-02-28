using Microsoft.EntityFrameworkCore;
using moneygram_api.Services.Interfaces;
using moneygram_api.Data;
using moneygram_api.DTOs;
using System.Threading.Tasks;

namespace moneygram_api.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly SSODbContext _context;

        public UserService(SSODbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(long userId)
        {
            var user = await _context.tblUsers.FindAsync(userId);
            return user is null ? null : new UserProfileDTO { FirstName = user.FirstName, Surname = user.Surname };
        }
    }
}