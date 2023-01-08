using Blog.API.Models;
using Blog.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Services
{
    public interface IUserService
    {
        public Task<UserDTO> GetUserProfile(string userId);
        public Task UpdateUserProfile(string userId, UserEditDTO model);
    }
    public class UserService:IUserService
    {
        private readonly Context _context;
        public UserService(Context context)
        {
            _context = context;
        }
        public async Task<UserDTO> GetUserProfile(string userId)
        {
            var user = _context.UserEntities.FirstOrDefault(x => x.Id == userId);
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
        }
        public async Task UpdateUserProfile(string userId, UserEditDTO userEdit)
        {
            var user = _context.UserEntities.FirstOrDefault(x => x.Id == userId);
            user.FullName = userEdit.FullName;
            user.BirthDate = userEdit.BirthDate;
            user.Gender = userEdit.Gender;
            user.PhoneNumber = userEdit.PhoneNumber;
            _context.UserEntities.Update(user);
            await _context.SaveChangesAsync();
        }


    }
}
