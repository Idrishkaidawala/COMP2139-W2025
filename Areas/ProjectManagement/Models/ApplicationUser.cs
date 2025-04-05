using Microsoft.AspNetCore.Identity;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class ApplicationUser: IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int UsernameChangeLimit { get; set; }
    public byte[]? ProfilePicture { get; set; }
    
}