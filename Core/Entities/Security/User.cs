

namespace Core.Entities.Security;

public class User 
{
    public Guid Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string PasswordSalt { get; set; }

    public DateTime RegisterDate { get; set; }
    
    public DateTime? LastLoginDate { get; set; }
}