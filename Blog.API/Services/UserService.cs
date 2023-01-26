using Blog.API.Exceptions;
using Blog.API.Models;
using Blog.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Blog.API.Services
{
    public interface IUserService
    {
        public Task<ActionResult<UserDTO>> GetUserProfile(string userId);
        public Task UpdateUserProfile(string userId, UserEditDTO model);
    }
    public class UserService:IUserService
    {
        private readonly Context _context;
        public UserService(Context context)
        {
            _context = context;
        }
        public async Task<ActionResult<UserDTO>> GetUserProfile(string userId)
        {
            var user = _context.UserEntities.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("There is no such user");
            }
            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AccountCreateDate = user.AccountCreateDate
            };
        }
        public async Task UpdateUserProfile(string userId, UserEditDTO userEdit)
        {
            var user = _context.UserEntities.FirstOrDefault(x => x.Id == userId);
            if (user == null) //Проверка на существование юзера
            {
                throw new NotFoundException("There is no such user"); 
            }
            string emailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (!Regex.IsMatch(userEdit.Email, emailRegex)) //Проверка на формат имейла
            {
                throw new ValidationException("Invalid email format");
            }
            string phoneRegex = @"\+7[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]";
            if (!Regex.IsMatch(userEdit.PhoneNumber, phoneRegex)) //Проверка на формат номера телефона
            {
                throw new ValidationException("Invalid phone number format");
            }
            if (_context.UserEntities.FirstOrDefault(x => x.Id != userId && x.Email == userEdit.Email) != null) //Проверка на уникальность имейла
            {
                throw new ValidationException("This email is already used");
            }
            if (_context.UserEntities.FirstOrDefault(x => x.Id != userId && x.FullName == userEdit.FullName) != null) //Проверка на уникальность никнейма
            {
                throw new ValidationException("This nickname is already used");
            }
            user.FullName = userEdit.FullName;
            user.BirthDate = userEdit.BirthDate;
            user.Gender = userEdit.Gender;
            user.PhoneNumber = userEdit.PhoneNumber;
            user.Email = userEdit.Email;
            _context.UserEntities.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
