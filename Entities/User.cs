namespace api.Entities;
// colocar Birthdate

public class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Name { get; set; }

    public string LastName { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
    
    public int RoleId { get; set; }
    public Role Role { get; set; }
    
    
}