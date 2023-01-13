using Blog.API.Models;
using Blog.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Blog.API.Services
{
    public interface IUserService
    {
        public Task<ActionResult<UserDTO>> GetUserProfile(string userId);
        public Task<ActionResult> UpdateUserProfile(string userId, UserEditDTO model);
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
                //Здесь вернём что-то, чтобы поймать это в контроллере и выкинуть NotFound
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
        public async Task<ActionResult> UpdateUserProfile(string userId, UserEditDTO userEdit)
        {
            var user = _context.UserEntities.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                //Здесь вернём что-то, чтобы поймать это в контроллере и выкинуть NotFound
            }
            string emailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            if (!Regex.IsMatch(userEdit.Email, emailRegex))
            {
                //Возвращаем что email невалидный
            }
            string phoneRegex = @"\+7[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]";
            if (!Regex.IsMatch(userEdit.PhoneNumber, phoneRegex))
            {
                //Возвращаем что номер невалидный
            }
            //Проверка на уникальность мейла и никнейма
            //А вот тут вопросики
            //Если пикать из UserEntities, а данные не меняются, мы эту же самую энтити и будем пикать
            user.FullName = userEdit.FullName;
            user.BirthDate = userEdit.BirthDate;
            user.Gender = userEdit.Gender;
            user.PhoneNumber = userEdit.PhoneNumber;
            user.Email = userEdit.Email;
            _context.UserEntities.Update(user);
            await _context.SaveChangesAsync();
            return new OkResult(); //Под вопросом необходимость возвращать OkResult() здесь
        }


    }
}
