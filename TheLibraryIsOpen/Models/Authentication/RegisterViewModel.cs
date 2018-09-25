using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TheLibraryIsOpen.Models.DBModels;

namespace TheLibraryIsOpen.Models.Authentication
{
    public class RegisterViewModel
    {
        [Required, DataType(DataType.EmailAddress), DisplayName("Email")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), DisplayName("Password")]
        public string Password { get; set; }

        [DataType(DataType.Password), Compare(nameof(Password)), DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("First Name")]
        public string FName { get; set; }

        [DisplayName("Last Name")]
        public string LName { get; set; }

        [DisplayName("Home Address")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber),DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public Client ToClient()
        {
            return new Client(FName, LName, Email, Address, PhoneNumber, Password, false);
        }

    }
}
