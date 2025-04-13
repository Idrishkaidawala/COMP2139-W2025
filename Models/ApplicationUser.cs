using Microsoft.AspNetCore.Identity;

namespace SmartInventoryManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public override string? PhoneNumber { get; set; }
        public List<int> PreferredCategories { get; set; } = new List<int>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
} 