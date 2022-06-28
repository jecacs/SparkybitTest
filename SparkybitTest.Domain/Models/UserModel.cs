namespace SparkybitTest.Domain.Models;

public class UserModel
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public DateTime CreatedAt { get; set; }
}