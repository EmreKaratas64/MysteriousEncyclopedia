using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.Request
{
    public class RequestDto
    {
        public int RequestID { get; set; }
        [Required(ErrorMessage = "Name and Surname cannot be empty")]
        public string RequestNameSurname { get; set; }
        [Required(ErrorMessage = "Username cannot be empty")]
        public string RequestUserName { get; set; }
        [Required(ErrorMessage = "Event Title cannot be empty")]
        public string RequestEventTitle { get; set; }
        [Required(ErrorMessage = "The Description cannot be empty")]
        public string RequestDescription { get; set; }
        public DateTime RequestDate { get; set; }
        public string? RequestStatus { get; set; }

    }
}
