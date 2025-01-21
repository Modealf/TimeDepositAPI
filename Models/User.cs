using System.ComponentModel.DataAnnotations.Schema;

namespace TimeDepositAPI.Models
{
    // Define the user roles
    public enum UserRole
    {
        Admin,
        User
    }

    public class User
    {
        public int Id { get; set; }  // Primary key
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        
        public required decimal Balance { get; set; }

        // New Role property with a default value
        public UserRole Role { get; set; } = UserRole.User;

        // Navigation property for user’s deposits
        public required ICollection<Deposit> Deposits { get; set; }
    }
}
