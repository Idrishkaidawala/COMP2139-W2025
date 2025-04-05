using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Display(Name = "Project Name")]
        [Required]
        [StringLength(100, ErrorMessage = "Project Name cannot be more than 100 characters.")]
        public required string Name { get; set; }

        [Display(Name = "Project Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Project Description cannot be longer than 500 characters.")]
        public string? Description { get; set; }

        [Display(Name = "Project Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate 
        { 
            get => _startDate.ToUniversalTime(); 
            set => _startDate = value.ToUniversalTime();
        }
        private DateTime _startDate;

        [Display(Name = "Project End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate 
        { 
            get => _endDate.ToUniversalTime(); 
            set => _endDate = value.ToUniversalTime();
        }
        private DateTime _endDate;

        public string? Status { get; set; }

        public List<ProjectTask>? Tasks { get; set; } = new();
    }
}