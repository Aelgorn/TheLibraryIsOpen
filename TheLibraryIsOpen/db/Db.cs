/*
    The Db class contains methods for performing sql queries on the db.
    Inspired from https://www.codeproject.com/Articles/43438/Connect-C-to-MySQL
    The database table looks like so:
    Table:      users
    Columns:    clientID int(12) 
                firstName varchar(255) 
                lastName varchar(255) 
                emailAddress varchar(255) 
                homeAddress varchar(255) 
                phoneNumber varchar(255) 
                password varchar(255) 
                isAdmin tinyint(1)
 */
using MySql.Data.MySqlClient;
using TheLibraryIsOpen.Models.DBModels;
using System.Collections.Generic;

namespace TheLibraryIsOpen.Database
{
    public class Db
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        public Db()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "35.236.241.114";
            database = "library";
            uid = "root";
            password = "library343";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.

                // TODO log error
                throw;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                // TODO log error
                throw;
            }
        }

        //Select statement
        public List<Client> GetAllClients()
        {
            //Create a list of unknown size to store the result
            List<Client> list = new List<Client>();
            string query = "SELECT * FROM users";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dr = cmd.ExecuteReader();

                //Read the data, create client object and store in list
                while (dr.Read())
                {
                    int clientID = (int)dr["clientID"];
                    string firstName = dr["firstName"] + "";
                    string lastName = dr["lastName"] + "";
                    string emailAddress = dr["emailAddress"] + "";
                    string homeAddress = dr["homeAddress"] + "";
                    string phoneNumber = dr["phoneNumber"] + "";
                    string password = dr["password"] + "";
                    bool isAdmin = (bool)dr["isAdmin"];

                    Client client = new Client(clientID, firstName, lastName, emailAddress, homeAddress, phoneNumber, password, isAdmin);

                    list.Add(client);
                }

                //close Data Reader
                dr.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        // TODO how are we storing the password, because I can't access it?
        public void CreateClient(Client client)
        {

            string query = $"INSERT INTO users VALUES({client.clientId}, {client.FirstName}, {client.LastName}, {client.EmailAddress}, {client.HomeAddress}, {client.PhoneNo}, null, {client.IsAdmin})";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public void DeleteClient(Client client)
        {
            string query = $"DELETE FROM users WHERE (clientID = {client.clientId})";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        public void UpdateClient(Client client)
        {
            string query = $"UPDATE users SET firstName = {client.FirstName}, lastName = {client.LastName}, emailAddress = {client.EmailAddress}, homeAddress = {client.HomeAddress}, phoneNumber = {client.PhoneNo}, isAdmin = {client.IsAdmin} WHERE clientID = {client.clientId}";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }
    }
}