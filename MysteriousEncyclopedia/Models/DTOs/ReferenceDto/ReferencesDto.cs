using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.ReferenceDto
{
    public class ReferencesDto
    {
        public int ReferenceID { get; set; }

        [Required(ErrorMessage = "Reference title cannot be empty")]
        [MaxLength(100, ErrorMessage = "Reference title cannot exceed 100 characters")]
        public string ReferenceTitle { get; set; }

        [Required(ErrorMessage = "Reference url cannot be empty")]
        [MaxLength(500, ErrorMessage = "Reference url cannot exceed 500 characters")]
        public string ReferenceUrl { get; set; }

        [MaxLength(1000, ErrorMessage = "Reference description cannot exceed 500 characters")]
        public string? ReferenceDescription { get; set; }
    }
}
