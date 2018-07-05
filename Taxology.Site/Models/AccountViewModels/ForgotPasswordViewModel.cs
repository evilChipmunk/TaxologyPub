using System.ComponentModel.DataAnnotations;

namespace Taxology.Site.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
