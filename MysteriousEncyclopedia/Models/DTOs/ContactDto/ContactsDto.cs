using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models.DTOs.ContactDto
{
    public class ContactsDto
    {
        public int ContactID { get; set; }

        [Required(ErrorMessage = "Subject cannot be empty")]
        public string ContactTitle { get; set; }

        [Required(ErrorMessage = "Contact name cannot be empty")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Contact message cannot be empty")]
        public string ContactText { get; set; }

        public DateTime ContactDate { get; set; }
    }
}
