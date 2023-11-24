using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.TopicDto
{
    public class ResultTopicDto
    {
        public int TopicID { get; set; }
        [Required(ErrorMessage = "Topic name cannot be empty")]
        public string TopicName { get; set; }
        [Required(ErrorMessage = "Topic Image cannot be empty")]
        public string TopicImage { get; set; }
    }
}
