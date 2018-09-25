﻿namespace TheLibraryIsOpen.Models.DBModels
{
    public class Client
    {
        public int clientId;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string HomeAddress { get; set; }
        public string PhoneNo { get; set; }
        private string Password;
        public bool IsAdmin { get; set; }

        
        public Client(string firstName, string lastName, string emailAddress, string homeAddress, string phoneNo, string password, bool isAdmin = false)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            HomeAddress = homeAddress;
            PhoneNo = phoneNo;
            Password = password;
            IsAdmin = isAdmin;
        }
        // another construcor who  assigns client id is added as requested.
        public Client(int cId, string firstName, string lastName, string emailAddress, string homeAddress, string phoneNo, string password, bool isAdmin = false) :
            this(firstName, lastName, emailAddress, homeAddress, phoneNo, password, isAdmin)
        {
            clientId = cId;
        }

        public void SetPassword(string pw)
        {
            Password = pw;
        }

        //verify if the password entered matches
        public bool PasswordVerify(string pswd)
        {
            return pswd.Equals(this.Password);
        }

        //method to allow someone to register as an admin.Since we will have admin class extends client, is this necessary? 
        public void RegisterAsAdmin()
        {
            IsAdmin = true;
        }

        public override string ToString()
        {
            return "Client:\nFirst Name:" + FirstName + "Last Name:" + LastName + "\nID: " + clientId + "\nEmail Address:" + EmailAddress + "\nHome Address:" + HomeAddress + "\nPhone No:" + PhoneNo;
        }
    }
}
