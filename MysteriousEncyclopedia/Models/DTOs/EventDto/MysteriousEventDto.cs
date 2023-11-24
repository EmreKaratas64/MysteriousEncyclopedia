using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.EventDto
{
    public class MysteriousEventDto
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Event title cannot be empty")]
        [MinLength(3, ErrorMessage = "Event title must be at least 3 characters")]
        [MaxLength(200, ErrorMessage = "Event title cannot exceed 200 characters")]
        public string EventTitle { get; set; }

        [Required(ErrorMessage = "Event image cannot be empty")]
        [MaxLength(250, ErrorMessage = "Event image url cannot exceed 250 characters")]
        public string EventImage { get; set; }

        [Required(ErrorMessage = "Event topics cannot be empty")]
        [MaxLength(100, ErrorMessage = "Event topics cannot exceed 100 characters")]
        public string EventTopics { get; set; }

        [Required(ErrorMessage = "Event date cannot be empty")]
        public string EventDate { get; set; }

        public DateTime EventModifiedDate { get; set; }

        [Required(ErrorMessage = "Event location cannot be empty")]
        [MaxLength(500, ErrorMessage = "Event location cannot exceed 500 characters")]
        public string EventLocation { get; set; }

        [Required(ErrorMessage = "Event content cannot be empty!")]
        public string EventContent { get; set; }

        public bool EventStatus { get; set; }

        public bool EventVisible { get; set; }
    }
}
