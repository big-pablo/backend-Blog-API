using Blog.API.Exceptions;
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
        public Task<PostPagedListDTO> GetPage(string userId, string author, PostSortingEnum sorting, int page, int size, int? minReadingTime, int? maxReadingTime, List<string>? tags);
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

        private static List<PostDTO> Sorting(PostSortingEnum sorting, List<PostDTO> list)
        {
            if (sorting == PostSortingEnum.CreateAsc) list.Sort((first, second) => DateTime.Compare((DateTime)first.Created, (DateTime)second.Created));
            if (sorting == PostSortingEnum.CreateDesc) list.Sort((first, second) => DateTime.Compare((DateTime)second.Created, (DateTime)first.Created));
            if (sorting == PostSortingEnum.LikeAsc) list.Sort((first, second) => first.Likes.CompareTo(second.Likes));
            if (sorting == PostSortingEnum.LikeDesc) list.Sort((first, second) => second.Likes.CompareTo(first.Likes));

            return list;
        }
        private static bool PostContainsFilterTags(List<TagDTO> postTags, List<string> tagstoFilter)
        {
            var count = 0;
            foreach (TagDTO tag in postTags)
            {
                if (tagstoFilter.Contains(tag.Name))
                {
                    count++;
                }
            }
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<PostPagedListDTO> GetPage(string userId, string author, PostSortingEnum sorting, int page, int size, int? minReadingTime, int? maxReadingTime, List<string>? tags)
        {
            List<PostEntity> postEntites = _context.PostEntities.Include(x => x.Tags).Include(x => x.Author).ThenInclude(j => j.UserPosts).ToList();
            if (minReadingTime == null)
            {
                minReadingTime = 0;
            }
            if (maxReadingTime == null)
            {
                maxReadingTime = 2147483647;
            }
            if (author == null)
            {
                postEntites = postEntites.Where(x => x.ReadingTime <= maxReadingTime && x.ReadingTime >= minReadingTime).ToList();
            }
            else
            {
               postEntites = postEntites.Where(x => x.Author.FullName == author && x.ReadingTime < maxReadingTime && x.ReadingTime > minReadingTime).ToList();
            }
            var postDTOs = new List<PostDTO>();
            foreach (PostEntity post in postEntites)
            {   
                List<CommentEntity> comments = _context.CommentEntities.Where(x => x.Post.Id == post.Id).ToList();
                List<LikeEntity> likes = _context.LikeEntities.Where(x => x.Post.Id == post.Id).ToList();
                List<TagDTO> tagList = new List<TagDTO>();
                bool hasLike = false;
                if (_context.LikeEntities.FirstOrDefault(x => x.User.Id == userId && x.Post.Id == post.Id) != null)
                {
                    hasLike = true;
                }
                foreach (TagEntity tagEntity in post.Tags)
                {
                    tagList.Add(new TagDTO()
                    {
                        Id = tagEntity.Id,
                        Name = tagEntity.Name
                    });
                }
                if (PostContainsFilterTags(tagList, tags) && tags.Count > 0)
                {
                    postDTOs.Add(new PostDTO()
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Description = post.Description,
                        ReadingTime = Convert.ToInt32(post.ReadingTime),
                        Image = post.Image,
                        AuthorId = post.Author.Id,
                        Author = post.Author.FullName,
                        Likes = likes.Count(),
                        HasLike = hasLike,
                        CommentsCount = comments.Count(),
                        Tags = tagList,
                        Created = post.Created
                    }); ;
                }
                else if (tags.Count == 0)
                {
                    postDTOs.Add(new PostDTO()
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Description = post.Description,
                        ReadingTime = Convert.ToInt32(post.ReadingTime),
                        Image = post.Image,
                        AuthorId = post.Author.Id,
                        Author = post.Author.FullName,
                        Likes = likes.Count(),
                        HasLike = hasLike,
                        CommentsCount = comments.Count(),
                        Tags = tagList,
                        Created = post.Created
                    }); ;
                }
            }
            postDTOs = Sorting(sorting, postDTOs);
            int i = 1;
            List<PostDTO> pagedPosts = new List<PostDTO>();
            foreach (PostDTO postDTO in postDTOs)
            {
                if (i > size * (page - 1) && i <= size * page)
                {
                    pagedPosts.Add(postDTO);
                }
                i++;
            }
            if (postDTOs.Count() == 0)
            {
                throw new NotFoundException("There are no posts with such parameters");
            }
            var pagination = new PageInfoDTO()
            {
                Current = page.ToString(),
                Size = size.ToString(),
                Count = postDTOs.Count().ToString()
            };
            return new PostPagedListDTO()
            {
                Posts = pagedPosts,
                Pagination = pagination
            };
        }
        public async Task<PostFullDTO> GetCertainPost(string id)
        {
            PostEntity postEntity = _context.PostEntities.Include(z => z.Tags).Include(x => x.Author).FirstOrDefault(x => x.Id == id);
            if (postEntity == null)
            {
                throw new NotFoundException("There is no such post");
            }
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
                Comments = commentDTOs,
                Created = postEntity.Created
            };  
            return postFullDTOs;
        }
        public async Task AddLike(string postId, string userId)
        {
            if (_context.PostEntities.FirstOrDefault(x => x.Id == postId) == null)
            {
                throw new NotFoundException("There is no such post");
            }
            if (_context.LikeEntities.FirstOrDefault(x => x.Post.Id == postId && x.User.Id == userId) != null)
            {
                throw new ObjectExistsException("Post is already liked");
            }
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
            if (_context.PostEntities.FirstOrDefault(x => x.Id == postId) == null)
            {
                throw new NotFoundException("There is no such post");
            }
            LikeEntity likeToRemove = _context.LikeEntities.FirstOrDefault(x => x.Post.Id == postId && x.User.Id == userId);
            if (likeToRemove == null)
            {
                throw new NotFoundException("No like to remove");
            }
            _context.LikeEntities.Remove(likeToRemove);
            _context.SaveChanges();
            return;
        }
    }
}
