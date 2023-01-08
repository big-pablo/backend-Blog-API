using Blog.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace Blog.API.Models
{
    public class Context:DbContext
    {
        public DbSet<CommentEntity> CommentEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<PostEntity> PostEntities { get; set; }
        public DbSet<TagEntity> TagEntities { get; set; }
        public DbSet<TokenEntity> TokenEntities { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

    }
}
