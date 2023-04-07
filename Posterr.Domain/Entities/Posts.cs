using FluentValidation.Results;
using Posterr.Domain.Support.Enums;
using Posterr.Domain.Validations;

namespace Posterr.Domain.Entities
{
    public class Posts
    {
        private readonly Guid _id;
        private readonly string _content;
        private readonly DateTime _createdDate;
        private readonly Guid _authorId;
        private readonly EnumTypeOfPost _enumTypeOfPost;
        private readonly Posts? _referencedPost;

        #region properties
        public Guid GetId() { return _id; }
        public string Content => _content;
        public DateTime CreatedDate => _createdDate; 
        public Guid AuthorId => _authorId;
        public EnumTypeOfPost TypeOfPost => _enumTypeOfPost; 
        public Posts? ReferencedPost => _referencedPost;

        #endregion


        public Posts(string content, Guid authorId, EnumTypeOfPost enumTypeOfPost, Posts? referencedPost = null)
        {
            _id = Guid.NewGuid();
            _createdDate = DateTime.Now;
            
            _content = content;            
            _authorId = authorId;
            _enumTypeOfPost = enumTypeOfPost;
            _referencedPost = referencedPost;
            _referencedPost = referencedPost;

            var isValid = IsValid();
            if (isValid.IsValid) return;
            
            var errors = isValid.Errors.Select(x => x.ErrorMessage);
            Exception exception = new($"Error on creating a Post class: {string.Join(", \n", errors)}");
            throw exception;
        }

        private ValidationResult IsValid()
        {
            return new CreatePostValidation().Validate(this);
        }
    }
}
