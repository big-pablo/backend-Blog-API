using Blog.API.Models;
using Blog.API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.API.Services
{
    public interface IInnerService
    {
        public Task<string> GetToken(IHeaderDictionary headerDictionary);
        public Task<string> GetUserId(ClaimsPrincipal principal);
        public Task<bool> TokenIsInBlackList(IHeaderDictionary headerDictionary);
    }
    public class InnerService:IInnerService
    {
        private readonly Context _context;
        public InnerService(Context context)
        {
            _context = context;
        }

        public async Task<string> GetToken(IHeaderDictionary headerDictionary)
        {
            var autorizationSrting = Convert.ToString(headerDictionary.Authorization);
            var token = autorizationSrting.Replace("Bearer ", "");
            return token;
        }
        public async Task<string> GetUserId(ClaimsPrincipal principal)
        {
             return principal?.Claims?.SingleOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        public async Task<bool> TokenIsInBlackList(IHeaderDictionary headerDictionary)
        {
            var token = await GetToken(headerDictionary);
            var tokenEntity = _context.TokenEntities.FirstOrDefault(x => x.Token == token);
            return !(tokenEntity == null);
        }

    }
}
