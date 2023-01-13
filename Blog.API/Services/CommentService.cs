using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;

namespace Blog.API.Services
{
    public interface ICommentService
    {
        public Task<List<CommentDTO>> GetNestedComments(string id);
        public Task AddComment(string postId, string parentCommentId, string content, string userId);
        public Task EditComment(string commentId, string content);
        public Task DeleteComment(string commentId);
    }

    public class CommentService:ICommentService
    {
        private Context _context;
        public CommentService(Context context)
        {
            _context = context;
        }
        public async Task<List<CommentDTO>> GetNestedComments(string id)
        {
            if (_context.CommentEntities.Where(x => x.Id == id) == null)
            {
                //Возвращаем notFound
            }
            List<CommentEntity> nestedCommentsEntities = _context.CommentEntities.Where(x => x.ParentComment.Id == id).ToList();
            List<CommentDTO> commentDTOs = new List<CommentDTO>();
            foreach (CommentEntity nestedCommentEntity in nestedCommentsEntities)
            {
                commentDTOs.Add(new CommentDTO()
                {
                    Id = nestedCommentEntity.Id,
                    Content = nestedCommentEntity.Content,
                    ModifiedDate = nestedCommentEntity.ModifiedDate,
                    DeleteDate = nestedCommentEntity.DeleteDate,
                    AuthorId = nestedCommentEntity.User.Id,
                    Author = nestedCommentEntity.User.FullName,
                    SubComments = _context.CommentEntities.Where(x => x.ParentComment.Id == nestedCommentEntity.Id).ToList().Count()
                });
            }
            return commentDTOs;
        }
        public async Task AddComment(string postId, string parentCommentId, string content, string userId)
        {
            Guid id = Guid.NewGuid();
            CommentEntity newCommentToAdd = new CommentEntity()
            {
                Id = id.ToString(),
                Post = _context.PostEntities.FirstOrDefault(x => x.Id == postId),
                Content=content,
                User = _context.UserEntities.FirstOrDefault(x => x.Id == userId),
                ParentComment = _context.CommentEntities.FirstOrDefault(x => x.Id == parentCommentId),
                ModifiedDate = null,
                DeleteDate = null
            };
            _context.CommentEntities.Add(newCommentToAdd);
            _context.SaveChangesAsync();
        }
        public async Task EditComment(string commentId, string content) //Проверку на то, что нужный юзер редачит коммент будет сделана в контроллере
        {
            CommentEntity commentToEdit = _context.CommentEntities.FirstOrDefault(x => x.Id == commentId);
            if (commentToEdit == null)
            {
                //Возвращаем NotFound
            }
            commentToEdit.Content = content;
            commentToEdit.ModifiedDate = DateTime.Now;
            _context.CommentEntities.Update(commentToEdit);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteComment(string commentId)
        {
            CommentEntity commentToDelete = _context.CommentEntities.FirstOrDefault(x => x.Id == commentId);
            if (commentToDelete == null)
            {
                //Возвращаем NotFound
            }
            commentToDelete.DeleteDate = DateTime.Now;
            commentToDelete.Content = "[Комментарий удалён]";
            _context.CommentEntities.Update(commentToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
