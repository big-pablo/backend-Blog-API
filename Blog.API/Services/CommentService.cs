﻿using Blog.API.Exceptions;
using Blog.API.Models;
using Blog.API.Models.DTOs;
using Blog.API.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Blog.API.Services
{
    public interface ICommentService
    {
        public Task<List<CommentDTO>> GetNestedComments(string id);
        public Task AddComment(string postId, string parentCommentId, string content, string userId);
        public Task EditComment(string commentId, string content, string userId);
        public Task DeleteComment(string commentId, string userId);
    }

    public class CommentService:ICommentService
    {
        private Context _context;
        public CommentService(Context context)
        {
            _context = context;
        }
        private List<CommentEntity> GetAllChildren(string parentId)
        {
            var currentChildren = _context.CommentEntities.Where(x => x.ParentComment.Id == parentId).Include(x => x.User).ToList();
            var allChildren = new List<CommentEntity>();
            allChildren.AddRange(currentChildren);
            foreach (var child in currentChildren)
            {
                var nextChildren = GetAllChildren(child.Id);
                allChildren.AddRange(nextChildren);
            }
            return allChildren;
        }
        public async Task<List<CommentDTO>> GetNestedComments(string id)
        {
            if (_context.CommentEntities.FirstOrDefault(x => x.Id == id) == null)
            {
                throw new NotFoundException("There is no such comment");
            }
            List<CommentEntity> nestedCommentsEntities = GetAllChildren(id);
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
                    SubComments = _context.CommentEntities.Where(x => x.ParentComment.Id == nestedCommentEntity.Id).ToList().Count(),
                    CreateDate = nestedCommentEntity.CreateDate
                });
            }
            return commentDTOs;
        }
        public async Task AddComment(string postId, string parentCommentId, string content, string userId)
        {
            if (_context.CommentEntities.Where(x => x.Id == postId) == null)
            {
                throw new NotFoundException("There is no such comment");
            }
            string commentRegex = @"^$";
            if (Regex.IsMatch(content, commentRegex))
            {
                throw new ValidationException("Comment cannot be empty");
            }
            Guid id = Guid.NewGuid();
            CommentEntity newCommentToAdd = new CommentEntity()
            {
                Id = id.ToString(),
                Post = _context.PostEntities.FirstOrDefault(x => x.Id == postId),
                Content=content,
                User = _context.UserEntities.FirstOrDefault(x => x.Id == userId),
                ParentComment = _context.CommentEntities.FirstOrDefault(x => x.Id == parentCommentId),
                ModifiedDate = null,
                DeleteDate = null,
                CreateDate = DateTime.Now
            };
            _context.CommentEntities.Add(newCommentToAdd);
            _context.SaveChangesAsync();
        }
        public async Task EditComment(string commentId, string content, string userId) 
        {
            string commentRegex = @"^$";
            if (Regex.IsMatch(content, commentRegex))
            {
                throw new ValidationException("Comment cannot be empty");
            }
            if (_context.CommentEntities.Where(x => x.Id == commentId && x.User.Id == userId) == null)
            {
                throw new ForbiddenException();
            }
            CommentEntity commentToEdit = _context.CommentEntities.FirstOrDefault(x => x.Id == commentId);
            if (commentToEdit.Content == "[Комментарий удалён]")
            {
                throw new ObjectExistsException("You cannot edit deleted comments");
            }
            if (commentToEdit == null)
            {
                throw new NotFoundException("There is no such comment");
            }
            commentToEdit.Content = content;
            commentToEdit.ModifiedDate = DateTime.Now;
            _context.CommentEntities.Update(commentToEdit);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteComment(string commentId, string userId)
        {
            if (_context.CommentEntities.Where(x => x.Id == commentId && x.User.Id == userId) == null)
            {
                throw new ForbiddenException();
            }
            CommentEntity commentToDelete = _context.CommentEntities.FirstOrDefault(x => x.Id == commentId);
            if (commentToDelete == null)
            {
                throw new NotFoundException("There is no such comment");
            }
            if (commentToDelete.Content == "[Комментарий удалён]")
            {
                throw new ObjectExistsException("This comment had already been deleted");
            }
            commentToDelete.DeleteDate = DateTime.Now;
            commentToDelete.Content = "[Комментарий удалён]";
            _context.CommentEntities.Update(commentToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
