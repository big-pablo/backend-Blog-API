using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services
{
    public interface IAuthorService
    {
        public Task<List<AuthorDTO>> GetAuthors();
    }

    public class AuthorService:IAuthorService
    {
        private readonly Context _context;
        public AuthorService(Context context)
        {
            _context = context;
        }
        private int AllPostLikes(List<PostEntity> posts)
        {
            int likesCount = 0;
            foreach (var post in posts)
            {
                likesCount += _context.LikeEntities.Where(x => x.Post.Id == post.Id).Count();
            }
            return likesCount;
        }
        public async Task<List<AuthorDTO>> GetAuthors()
        {
            List<UserEntity> userEntities = _context.UserEntities.Include(x => x.UserPosts).Where(x => x.UserPosts.Count() > 0).OrderBy(x => x.FullName).ToList();
            List<AuthorDTO> authors = new List<AuthorDTO>();
            foreach (UserEntity userEntity in userEntities)
            {
                int postsAmount = userEntity.UserPosts.Count();
                authors.Add(new AuthorDTO()
                {
                    FullName = userEntity.FullName,
                    BirthDate = userEntity.BirthDate,
                    Gender = userEntity.Gender,
                    Posts = postsAmount,
                    Likes = AllPostLikes(userEntity.UserPosts),
                    Created = userEntity.AccountCreateDate
                });
            }
            return authors;
        }
    }
}
