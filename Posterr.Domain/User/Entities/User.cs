using FluentValidation.Results;
using Posterr.Domain.Validations;

namespace Posterr.Domain.User.Entities;

public class User
{
    private readonly Guid _id;
    private readonly string _name;
    private readonly DateTime _joinedDate;
    private IEnumerable<string>? _errorList;
    
    #region properties
    public Guid GetId() => _id;
    public string Name => _name;
    public DateTime JoinedDate => _joinedDate; 
    #endregion
    
    public User(string name)
    {
        _id = Guid.NewGuid();
        _joinedDate = DateTime.Now;
        
        _name = name;
    }
    
    private ValidationResult IsValid()
    {
        return new CreateUserValidation().Validate(this);
    }

    public IEnumerable<string>? GetErrorList()
    {
        return _errorList;
    }
    public bool Validate()
    {
        var isValid = this.IsValid();
        if (isValid.IsValid) return true;

        _errorList = new List<string>();
        _errorList = isValid.Errors.Select(x => x.ErrorMessage);

        return false;
    }
}