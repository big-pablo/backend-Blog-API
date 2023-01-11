using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;
using Blog.API.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.API.Services
{
    public interface IPostService
    {
        public Task<PostPagedListDTO> GetPage(List<string> tags, string author, int minReadingTime, int maxReadingTime, PostSortingEnum sorting);
        public Task<PostFullDTO> GetCertainPost(string id);
        public Task AddLike(string postId, string userId);
        public Task RemoveLike(string postId, string userId);
    }

    public class PostService : IPostService
    {
        private Context _context;
        public PostService(Context context)
        {
            _context = context;
        }
        public async Task<PostPagedListDTO> GetPage(List<string> tags, string author, int minReadingTime, int maxReadingTime, PostSortingEnum sorting)
        {
            List<PostEntity> postEntites = _context.PostEntities.Include(x => x.Tags).Include(x => x.Author).ThenInclude(j => j.UserPosts).ToList();
            var pagination = new PageInfoDTO()
            {
                Current = "1",
                Size = "1",
                Count = postEntites.Count().ToString()
            };
            var postDTOs = new List<PostDTO>();
            foreach (PostEntity post in postEntites)
            {
                List<CommentEntity> comments = _context.CommentEntities.Where(x => x.Post.Id == post.Id).ToList();
                List<LikeEntity> likes = _context.LikeEntities.Where(x => x.Post.Id == post.Id).ToList();
                List<TagDTO> tagList = new List<TagDTO>();
                foreach (TagEntity tagEntity in post.Tags)
                {
                    tagList.Add(new TagDTO()
                    {
                        Id = tagEntity.Id,
                        Name = tagEntity.Name
                    });
                }
                postDTOs.Add(new PostDTO()
                {
                    Id = post.Id,
                    Title = post.Title,
                    Description = post.Description,
                    ReadingTime = Convert.ToInt32(post.ReadingTime),
                    Image = post.Image,
                    AuthorId = post.Author.Id,
                    Likes = likes.Count(),
                    HasLike = false,
                    CommentsCount = comments.Count(),
                    Tags = tagList,
                });
            }
            return new PostPagedListDTO()
            {
                Posts = postDTOs,
                Pagination = pagination
            };
        }
        public async Task<PostFullDTO> GetCertainPost(string id)
        {
            PostEntity postEntity = _context.PostEntities.Include(z => z.Tags).FirstOrDefault(x => x.Id == id);
            List<TagDTO> tagDTOs = new List<TagDTO>();
            foreach (TagEntity tagEntity in postEntity.Tags)
            {
                tagDTOs.Add(new TagDTO()
                {
                    Id=tagEntity.Id,
                    Name = tagEntity.Name
                });
            }
            List<CommentEntity> commentEntities = _context.CommentEntities.Where(x => x.Post == postEntity && x.ParentComment == null).ToList();
            List<CommentDTO> commentDTOs = new List<CommentDTO>();
            foreach (CommentEntity commentEntity in commentEntities)
            {
                commentDTOs.Add(new CommentDTO()
                {
                    Id = commentEntity.Id,
                    Content = commentEntity.Content,
                    ModifiedDate = commentEntity.ModifiedDate,
                    DeleteDate = commentEntity.DeleteDate,
                    AuthorId = commentEntity.User.Id,
                    Author = commentEntity.User.FullName,
                    SubComments = _context.CommentEntities.Where(x => x.Post == postEntity && x.ParentComment.Id == commentEntity.Id).ToList().Count()
                }) ;
            }
            PostFullDTO postFullDTOs = new PostFullDTO()
            {
                Id = postEntity.Id,
                Title = postEntity.Title,
                Description = postEntity.Description,
                ReadingTime = postEntity.ReadingTime,
                Image = postEntity.Image,
                AuthorId = postEntity.Author.Id,
                Author = postEntity.Author.FullName,
                Likes = _context.LikeEntities.Where(x => x.Post == postEntity).ToList().Count(),
                HasLike = false,
                CommentsCount = _context.CommentEntities.Where(x => x.Post == postEntity).ToList().Count(),
                Tags = tagDTOs,
                Comments = commentDTOs
            };  
            return postFullDTOs;
        }
        public async Task AddLike(string postId, string userId)
        {
            Guid id = Guid.NewGuid();
            LikeEntity likeToAdd = new LikeEntity()
            {
                Id = id.ToString(),
                Post = _context.PostEntities.FirstOrDefault(x => x.Id == postId),
                User = _context.UserEntities.FirstOrDefault(x => x.Id == userId),
            };
            _context.LikeEntities.Add(likeToAdd);
            _context.SaveChanges();
            return;
        }
        public async Task RemoveLike(string postId, string userId)
        {
            LikeEntity likeToRemove = _context.LikeEntities.FirstOrDefault(x => x.Post.Id == postId && x.User.Id == userId);
            _context.LikeEntities.Remove(likeToRemove);
            _context.SaveChanges();
            return;
        }
    }
}
