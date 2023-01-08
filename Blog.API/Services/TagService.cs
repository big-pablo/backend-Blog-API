using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;

namespace Blog.API.Services
{
    public interface ITagService
    {
        public Task<List<TagEntity>> GetTags();
    }

    public class TagService:ITagService
    {
        private readonly Context _context;
        public TagService(Context context)
        {
            _context = context;
        }
        public async Task<List<TagEntity>> GetTags()
        {
            List<TagEntity> tagEntityList = _context.TagEntities.ToList(); //Мб надо будет переделать, чтобы оно возвращало List<TagDTO>
            return tagEntityList;
        }
    }
}
