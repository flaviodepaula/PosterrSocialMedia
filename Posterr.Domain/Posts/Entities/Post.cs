using FluentValidation.Results;
using Posterr.Domain.Posts.Support.Enums;
using Posterr.Domain.Validations;

namespace Posterr.Domain.Posts.Entities
{
    public class Post
    {
        private readonly Guid _id;
        private readonly string _content;
        private readonly DateTime _createdDate;
        private readonly Guid _authorId;
        private readonly EnumTypeOfPost _typeOfPost;
        private readonly Post? _referencedPost;
        
        #region properties
        public Guid GetId() { return _id; }
        public string Content => _content;
        public DateTime CreatedDate => _createdDate; 
        public Guid AuthorId => _authorId;
        public EnumTypeOfPost TypeOfPost => _typeOfPost; 
        public Post? ReferencedPost => _referencedPost;

        #endregion

        public Post(string content, Guid authorId, EnumTypeOfPost typeOfPost, Post? referencedPost = null)
        {
            _id = Guid.NewGuid();
            _createdDate = DateTime.Now;
            
            _content = content;            
            _authorId = authorId;
            _typeOfPost = typeOfPost;
            _referencedPost = referencedPost;
            _referencedPost = referencedPost;
        }

        public ValidationResult IsValid()
        {
            return new CreatePostValidation().Validate(this);
        }
    }
}
