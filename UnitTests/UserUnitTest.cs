using Posterr.Domain.User.Entities;

namespace UnitTests;

public class UserUnitTest
{
    [Test]
    public void ValidClass_CreatingUserClass()
    {
        //trying to quode a quode post - not allowed
        const string name = "flaviovilaca1";
        var newUser = new User(name);

        var isOk = newUser.Validate();
        var errorMsg = newUser.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_UserNameWithWrongLengh()
    {
        //trying to quode a quode post - not allowed
        const string name = "123456789123456";
        var newUser = new User(name);

        var isOk = newUser.Validate();
        var errorMsg = newUser.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
    
    [Test]
    public void InvalidClass_InvalidUserName()
    {
        //trying to quode a quode post - not allowed
        const string name = "_@viovilaca";
        var newUser = new User(name);

        var isOk = newUser.Validate();
        var errorMsg = newUser.GetErrorList()?.Aggregate("", (current, error) => current + ("\n" + error));
        
        Assert.That(!isOk, errorMsg);
    }
}