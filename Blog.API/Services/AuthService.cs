using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.API.Services
{
    public interface IAuthService
    {
        public Task<ActionResult<TokenResponseDTO>> Register(UserRegisterDTO model);
        public Task<ActionResult<TokenResponseDTO>> Login(LoginCredentialsDTO model);
        public Task Logout(string tokenToDiscard);
    }
    public class AuthService: IAuthService
    {
        private Context _context;
        public AuthService(Context context)
        {
            _context = context;
        }
        private ClaimsIdentity GetIdentity(string email, string id)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, email),
                new Claim(JwtRegisteredClaimNames.NameId, id),
            };
            ClaimsIdentity claimidentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimTypes.NameIdentifier);
            return claimidentity;
        }

        private JwtSecurityToken GenerateJWT(string email, string id)
        {
            var now = DateTime.UtcNow;
            var identity = GetIdentity(email, id);
            var jwt = new JwtSecurityToken(
                issuer: JWTConfiguration.Issuer,
                audience: JWTConfiguration.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.AddMinutes(JWTConfiguration.Lifetime),
                signingCredentials: new SigningCredentials(JWTConfiguration.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return jwt;
        }

        public async Task<ActionResult<TokenResponseDTO>> Register(UserRegisterDTO model)
        {
            var check = _context.UserEntities.FirstOrDefault(x => x.Email == (model.Email).ToString());
            if (check != null) throw new Exception();
            var id = Guid.NewGuid();
            await _context.UserEntities.AddAsync(new UserEntity
            {
                Id = id.ToString(),
                FullName = model.FullName,
                Password = model.Password,
                Email = model.Email.ToString(),
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber
            });

            var jwt = GenerateJWT(model.Email, id.ToString());
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new TokenResponseDTO { Token = encodedJwt };
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ActionResult<TokenResponseDTO>> Login(LoginCredentialsDTO model)
        {
            var user = _context.UserEntities.FirstOrDefault(x => x.Email == (model.Email).ToString() && x.Password == model.Password);
            if (user == null)
            {
                throw new Exception();
            }
            var jwt = GenerateJWT(Convert.ToString(model.Email), user.Id.ToString());
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new TokenResponseDTO { Token = encodedJwt };
            return response;
        }

        public async Task Logout(string token)
        {
            var id = Guid.NewGuid();
            await _context.TokenEntities.AddAsync(new TokenEntity
            {
                Id = id.ToString(),
                Token = token
            });
            await _context.SaveChangesAsync();
            return;
        }

    }
}
