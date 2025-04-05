using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Display(Name = "Task Title")]
    [Required]
    [StringLength(100,ErrorMessage = "Task Title cannot be more than 100 characters.")]
    public required string Title { get; set; }
    
    
    [Display(Name = "Task Title")]
    [Required]
    [StringLength(500,ErrorMessage = "Task Description cannot be more than 500 characters.")]
    public required string Description { get; set; }
    
    // Foreign Key
    public int ProjectId { get; set; } 
    
    // Navigation Property
    // This property allows easy access to the related Project entity from the Task entity
    public Project? Project { get; set; }
    
}