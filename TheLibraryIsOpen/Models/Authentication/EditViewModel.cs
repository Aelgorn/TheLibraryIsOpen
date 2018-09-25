using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheLibraryIsOpen.Models.Authentication
{
    public class EditViewModel
    {
        [DataType(DataType.EmailAddress), DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("First Name")]
        public string FName { get; set; }

        [DisplayName("Last Name")]
        public string LName { get; set; }

        [DisplayName("Home Address")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber), DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        [DisplayName("Is An Administrator")]
        public bool IsAdmin { get; set; }
    }
}
