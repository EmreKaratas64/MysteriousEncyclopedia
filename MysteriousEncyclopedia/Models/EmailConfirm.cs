using System.ComponentModel.DataAnnotations;

namespace MysteriousEncyclopedia.Models
{
    public class EmailConfirm
    {
        [Required(ErrorMessage = "Please enter your Username")]
        public string userName { get; set; }

        [Required(ErrorMessage = "Please enter the confirmation code")]
        public string confirmationCode { get; set; }
    }
}
