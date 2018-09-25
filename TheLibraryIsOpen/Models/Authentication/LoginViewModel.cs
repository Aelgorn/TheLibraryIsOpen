using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TheLibraryIsOpen.Models.Authentication
{
    public class LoginViewModel
    {
        [Required, DataType(DataType.EmailAddress), DisplayName("Email")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), DisplayName("Password")]
        public string Password { get; set; }
    }
}
