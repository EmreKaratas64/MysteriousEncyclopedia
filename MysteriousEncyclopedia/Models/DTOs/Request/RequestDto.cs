using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.Request
{
    public class RequestDto
    {
        public int RequestID { get; set; }
        [Required(ErrorMessage = "Name and Surname cannot be empty")]
        public string RequestNameSurname { get; set; }
        public string? RequestUserId { get; set; }
        public string? UserName { get; }
        [Required(ErrorMessage = "Event cannot be empty")]
        public int RequestEventId { get; set; }
        public string? EventTitle { get; set; }
        public DateTime RequestDate { get; set; }
        [Required(ErrorMessage = "The Description cannot be empty")]
        public string RequestDescription { get; set; }
        public string? RequestStatus { get; set; }

    }
}
