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

    Table: books
    Columns:
            bookID int(11) AI PK
            title varchar(255)
            author varchar(255)
            format varchar(255)
            pages int(11)
            publisher varchar(255)
            date varchar(255)
            language varchar(255)
            isbn10 varchar(255)
            isbn13 varchar(255)

    Table: magazines
    Columns:
            magazineID int(11) AI PK
            title varchar(255)
            publisher varchar(255)
            language varchar(255)
            date varchar(255)
            isbn10 varchar(255)
            isbn13 varchar(255)

    Table: movies
    Columns:
            movieID int(11) AI PK
            title varchar(255)
            director varchar(255)
            language varchar(255)
            subtitles varchar(255)
            dubbed varchar(255)
            releasedate varchar(255)
            runtime varchar(255)

    Table: cds
    Columns:
            cdID int(11) AI PK
            type varchar(255)
            title varchar(255)
            artist varchar(255)
            label varchar(255)
            releasedate varchar(255)
            asin varchar(255)

    Table: person
    Columns:
            personID int(11) AI PK
            firstname varchar(255)
            lastname varchar(255)

    Table: movieactor , movieproducer
    Columns:
            movieid int(11)
            personid int(11)

    Table: modelCopy
    Columns:
            id int(11) AI PK 
            modelType int() 
            modelID int(11) 
            borrowerID int(12) FK
            borrowedDate date
            returnDate date
            foreign key (borrowerID) references users(clientID)	    

    One things for query language:
    Don't put space between {}. Ex : \"{ isbn13 }\" is wrong, and \"{isbn13}\" is right
 */

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheLibraryIsOpen.Models.DBModels;
using TheLibraryIsOpen.Models;
using static TheLibraryIsOpen.Constants.TypeConstants;

namespace TheLibraryIsOpen.Database
{
    public class Db
    {
        private readonly string connectionString;
        private readonly string server;
        private readonly string database;
        private readonly string uid;
        private readonly string password;

        public Db()
        {
            server = "35.236.241.114";
            database = "library";
            uid = "root";
            password = "library343";
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
        }

        /*
        * For all types of tables
        * Method to send query to database for creating, updating and deleting
        */

        public void QuerySend(string query)
        {
            //open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Execute command
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        #region clients

        // Returns a list of all clients in the db converted to client object.
        public List<Client> GetAllClients()
        {
            //Create a list of unknown size to store the result
            List<Client> list = new List<Client>();
            string query = "SELECT * FROM users;";

            //Open connection

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
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
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }



        //Find modelCopy by Client ID, returns list
        public List<Client> FindClientByModelCopy(ModelCopy mc)
        {
            string query = $"SELECT * FROM users WHERE clientID = \"{mc.borrowerID}\";";
            List<Client> client = new List<Client>();

            //Open connection

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int clientID = (int)dr["clientID"];
                            string firstName = dr["firstName"] + "";
                            string lastName = dr["lastName"] + "";
                            string emailAddress = dr["emailAddress"] + "";
                            string homeAddress = dr["homeAddress"] + "";
                            string phoneNumber = dr["phoneNumber"] + "";
                            string password = dr["password"] + "";
                            bool isAdmin = (bool)dr["isAdmin"];

                            client.Add(new Client(clientID, firstName, lastName, emailAddress, homeAddress, phoneNumber, password, isAdmin));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return client;
        }

        // Selects a client by id and returns a client object
        public Client GetClientById(int id)
        {
            string query = $"SELECT * FROM users WHERE clientID = \"{id}\";";
            Client client = null;

            //Open connection

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int clientID = (int)dr["clientID"];
                            string firstName = dr["firstName"] + "";
                            string lastName = dr["lastName"] + "";
                            string emailAddress = dr["emailAddress"] + "";
                            string homeAddress = dr["homeAddress"] + "";
                            string phoneNumber = dr["phoneNumber"] + "";
                            string password = dr["password"] + "";
                            bool isAdmin = (bool)dr["isAdmin"];

                            client = new Client(clientID, firstName, lastName, emailAddress, homeAddress, phoneNumber, password, isAdmin);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return client;
        }

        // Selects a client by email and returns a client object
        public Client GetClientByEmail(string emailAddres)
        {
            string query = $"SELECT * FROM users WHERE emailAddress = \"{emailAddres}\";";
            Client client = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int clientID = (int)dr["clientID"];
                            string firstName = dr["firstName"] + "";
                            string lastName = dr["lastName"] + "";
                            string emailAddress = dr["emailAddress"] + "";
                            string homeAddress = dr["homeAddress"] + "";
                            string phoneNumber = dr["phoneNumber"] + "";
                            string password = dr["password"] + "";
                            bool isAdmin = (bool)dr["isAdmin"];

                            client = new Client(clientID, firstName, lastName, emailAddress, homeAddress, phoneNumber, password, isAdmin);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return client;
        }

        // Inserts a new client into the db
        public void CreateClient(Client client)
        {
            string query = $"INSERT INTO users (firstName, lastName, emailAddress, homeAddress, phoneNumber, password, isAdmin) VALUES(\"{client.FirstName}\", \"{client.LastName}\", \"{client.EmailAddress}\", \"{client.HomeAddress}\", \"{client.PhoneNo}\", \"{client.Password}\", {client.IsAdmin});";
            QuerySend(query);
        }

        // Deletes a client by id from the db
        public void DeleteClient(Client client)
        {
            string query = $"DELETE FROM users WHERE (clientID = \"{client.clientId}\");";
            QuerySend(query);
        }

        // Updates a client's information in the db by id
        public void UpdateClient(Client client)
        {
            string query = $"UPDATE users SET firstName = \"{client.FirstName}\", lastName = \"{client.LastName}\", emailAddress = \"{client.EmailAddress}\", homeAddress = \"{client.HomeAddress}\", phoneNumber = \"{client.PhoneNo}\", password = \"{client.Password}\", isAdmin = {client.IsAdmin} WHERE clientID = \"{client.clientId}\";";
            QuerySend(query);
        }

        #endregion clients

        #region magazines

        /*
         *  Magazine Table methods
         */

        public void CreateMagazine(Magazine magazine)
        {
            string query =
                $"INSERT INTO magazines (title, publisher, language, date, isbn10, isbn13) VALUES(\"{magazine.Title}\",\"{magazine.Publisher}\",\"{magazine.Language}\",\"{magazine.Date.ToShortDateString()}\",\"{magazine.Isbn10}\",\"{magazine.Isbn13}\");";

            QuerySend(query);
        }

        public void CreateMagazines(params Magazine[] magazines)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO magazines (title, publisher, language, date, isbn10, isbn13) VALUES");
            for (int i = 0; i < magazines.Length; ++i)
            {
                sb.Append($"(\"{magazines[i].Title}\",\"{magazines[i].Publisher}\",\"{magazines[i].Language}\",\"{magazines[i].Date.ToShortDateString()}\",\"{magazines[i].Isbn10}\",\"{magazines[i].Isbn13}\"){(i + 1 < magazines.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        public void UpdateMagazines(params Magazine[] magazines)
        {
            StringBuilder sb = new StringBuilder("UPDATE magazines SET ");
            for (int i = 0; i < magazines.Length; ++i)
            {
                sb.Append($"title = \"{magazines[i].Title}\", publisher = \"{magazines[i].Publisher}\", language = \"{magazines[i].Language}\", date = \"{magazines[i].Date.ToShortDateString()}\", isbn10 = \"{magazines[i].Isbn10}\", isbn13 = \"{magazines[i].Isbn13}\" WHERE (magazineID = \"{magazines[i].MagazineId}\"){(i + 1 < magazines.Length ? "," : ";")}");
            }
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // need improve
        public void UpdateMagazine(Magazine magazine)
        {
            string query = $"UPDATE magazines SET title = \"{magazine.Title}\", publisher = \"{magazine.Publisher}\", language = \"{magazine.Language}\", date = \"{magazine.Date.ToShortDateString()}\", isbn10 = \"{magazine.Isbn10}\", isbn13 = \"{magazine.Isbn13}\" WHERE (magazineID = \"{magazine.MagazineId}\");";

            QuerySend(query);
        }

        // update magazine by ID
        public void UpdateMagazine(Magazine magazine, int magazineID)
        {
            string query = $"UPDATE magazines SET title = \"{magazine.Title}\", publisher = \"{magazine.Publisher}\", language = \"{magazine.Language}\", date = \"{magazine.Date.ToShortDateString()}\", isbn10 = \"{magazine.Isbn10}\", isbn13 = \"{magazine.Isbn13}\" WHERE (magazineID = \"{magazineID}\");";

            QuerySend(query);
        }

        public void DeleteMagazines(params Magazine[] magazines)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM magazines ");
            for (int i = 0; i < magazines.Length; ++i)
            {
                sb.Append($"WHERE magazineID = \"{magazines[i].MagazineId}\"{(i + 1 < magazines.Length ? "," : ";")}");
            }
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // delete magazine by magazine instance
        public void DeleteMagazine(Magazine magazine)
        {
            string query = $"DELETE FROM magazines WHERE (magazineID = \"{magazine.MagazineId}\");";

            QuerySend(query);
        }

        // delete magazine by ID
        public void DeleteMagazineByID(int magazineID)
        {
            string query = $"DELETE FROM magazines WHERE (magazineID = \"{magazineID}\");";

            QuerySend(query);
        }

        public List<Magazine> GetAllMagazines()
        {
            string query = $"SELECT * FROM magazines;";

            List<Magazine> magazines = new List<Magazine>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazines.Add(new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13));
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return magazines;
        }

        public Magazine GetMagazineById(int id)
        {
            string query = $"SELECT * FROM magazines WHERE magazineID = \" { id } \";";

            Magazine magazine = QueryRetrieveMaganize(query);
            return magazine;
        }

        public Magazine GetMagazineByIsbn10(string isbn10)
        {
            string query = $"SELECT * FROM magazines WHERE isbn10 = \"{isbn10}\";";

            Magazine magazine = QueryRetrieveMaganize(query);

            return magazine;
        }

        //
        public Magazine GetMagazineByIsbn13(string isbn13)
        {
            string query = $"SELECT * FROM magazines WHERE isbn13 = \"{isbn13}\";";

            Magazine magazine = QueryRetrieveMaganize(query);

            return magazine;
        }

        /*
    * For retrieving ONE object ONLY
    * Method to retrieve maganize information by id or isbn10 or isbn13
    */

        public Magazine QueryRetrieveMaganize(string query)
        {
            Magazine magazine = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        if (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return magazine;
        }

        #region SearchMagazines

        // methods for searching magazines

        public List<Magazine> SearchMagazinesByTitle(string MagazineString)
        {
            List<Magazine> list = new List<Magazine>();
            Magazine magazine = null;

            string query = $"SELECT * FROM magazines WHERE LOWER(title) LIKE LOWER('%{MagazineString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                            list.Add(magazine);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Magazine> SearchMagazinesByPublisher(string MagazineString)
        {
            List<Magazine> list = new List<Magazine>();
            Magazine magazine = null;

            string query = $"SELECT * FROM magazines WHERE LOWER(publisher) LIKE LOWER('%{MagazineString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                            list.Add(magazine);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Magazine> SearchMagazinesByLanguage(string MagazineString)
        {
            List<Magazine> list = new List<Magazine>();
            Magazine magazine = null;

            string query = $"SELECT * FROM magazines WHERE LOWER(language) LIKE LOWER('%{MagazineString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                            list.Add(magazine);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Magazine> SearchMagazinesByDate(string MagazineString)
        {
            List<Magazine> list = new List<Magazine>();
            Magazine magazine = null;

            string query = $"SELECT * FROM magazines WHERE LOWER(date) LIKE LOWER('%{MagazineString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                            list.Add(magazine);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Magazine> SearchMagazinesByIsbn10(string MagazineString)
        {
            List<Magazine> list = new List<Magazine>();
            Magazine magazine = null;

            string query = $"SELECT * FROM magazines WHERE LOWER(isbn10) = \"{MagazineString}\";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                            list.Add(magazine);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Magazine> SearchMagazinesByIsbn13(string MagazineString)
        {
            List<Magazine> list = new List<Magazine>();
            Magazine magazine = null;

            string query = $"SELECT * FROM magazines WHERE LOWER(isbn13) = \"{MagazineString}\";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        while (dr.Read())
                        {
                            int magazineId = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            magazine = new Magazine(magazineId, title, publisher, language, date, isbn10, isbn13);
                            list.Add(magazine);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }


        // find magazine by modelCopy
        public List<Magazine> FindMagazineByModelCopy(ModelCopy mc)
        {
            string query = $"SELECT * FROM magazines WHERE magazineID = \"{mc.modelID}\";";
            List<Magazine> mag = new List<Magazine>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int magazineID = (int)dr["magazineID"];
                            string title = dr["title"] + "";
                            string publisher = dr["publisher"] + "";
                            string language = dr["language"] + "";
                            DateTime date = DateTime.Parse(dr["date"] + "");
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            mag.Add(new Magazine(magazineID, title, publisher, language, date, isbn10, isbn13));
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return mag;
        }

        #endregion SearchMagazines

        #endregion magazines

        #region music

        /*
         * The following methods are made for the music table
         */

        // Inserts new music into the database
        public void CreateMusic(params Music[] music)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO cds (type, title, artist, label, releasedate, asin) VALUES");
            for (int i = 0; i < music.Length; ++i)
            {
                sb.Append($"(\"{music[i].Type}\", \"{music[i].Title}\", \"{music[i].Artist}\", \"{music[i].Label}\", \"{music[i].ReleaseDate.ToShortDateString()}\", \"{music[i].Asin}\"){(i + 1 < music.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Update a music's information in the database by MusicId
        public void UpdateMusic(params Music[] music)
        {
            StringBuilder sb = new StringBuilder("UPDATE cds SET ");
            for (int i = 0; i < music.Length; ++i)
            {
                sb.Append($"type = \"{music[i].Type}\", title = \"{music[i].Title}\",artist = \"{music[i].Artist}\", label = \"{music[i].Label}\", releasedate = \"{music[i].ReleaseDate.ToShortDateString()}\", asin = \"{music[i].Asin}\" WHERE cdID = \"{music[i].MusicId}\"{(i + 1 < music.Length ? "," : ";")}");
            }
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // Delete music by MusicId from the database
        public void DeleteMusic(params Music[] music)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM cds ");
            for (int i = 0; i < music.Length; ++i)
            {
                sb.Append($"WHERE cdID = \"{music[i].MusicId}\"{(i + 1 < music.Length ? "," : ";")}");
            }
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // Retrieve a music information by id
        public Music GetMusicById(int id)
        {
            string query = $"SELECT * FROM cds WHERE cdID = \" { id } \";";
            return QueryRetrieveMusic(query);
        }

        // Retrieve a music information by ISBN
        public Music GetMusicByAsin(string ASIN)
        {
            string query = $"SELECT * FROM cds WHERE (asin = \"{ ASIN }\");";
            return QueryRetrieveMusic(query);
        }

        /*
         * For retrieving ONE object ONLY
         * Method to retrieve music information by id or asin
         */

        public Music QueryRetrieveMusic(string query)
        {
            Music music = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        if (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return music;
        }

        // Returns a list of all musics in the db converted to music object.
        public List<Music> GetAllMusic()
        {
            //Create a list of unknown size to store the result
            List<Music> list = new List<Music>();
            Music music = null;
            string query = "SELECT * FROM cds;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return list;
        }

        #region SearchMusic

        public List<Music> SearchMusicByType(string musicType)
        {
            List<Music> list = new List<Music>();
            Music music = null;

            string query = $"SELECT * FROM cds WHERE LOWER(type) LIKE LOWER('%{musicType}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return list;
        }

        public List<Music> SearchMusicByTitle(string musicTitle)
        {
            List<Music> list = new List<Music>();
            Music music = null;

            string query = $"SELECT * FROM cds WHERE LOWER(title) LIKE LOWER('%{musicTitle}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return list;
        }

        public List<Music> SearchMusicByArtist(string musicArtist)
        {
            List<Music> list = new List<Music>();
            Music music = null;

            string query = $"SELECT * FROM cds WHERE LOWER(artist) LIKE LOWER('%{musicArtist}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return list;
        }

        public List<Music> SearchMusicByLabel(string musicLabel)
        {
            List<Music> list = new List<Music>();
            Music music = null;

            string query = $"SELECT * FROM cds WHERE LOWER(label) LIKE LOWER('%{musicLabel}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return list;
        }

        public List<Music> SearchMusicByReleaseDate(string musicReleaseDate)
        {
            List<Music> list = new List<Music>();
            Music music = null;

            string query = $"SELECT * FROM cds WHERE LOWER(releasedate) LIKE LOWER('%{musicReleaseDate}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return list;
        }

        public List<Music> SearchMusicByASIN(string musicASIN)
        {
            List<Music> list = new List<Music>();
            Music music = null;

            string query = $"SELECT * FROM cds WHERE LOWER(ASIN) LIKE LOWER('%{musicASIN}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int musicId = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music = new Music(musicId, type, title, artist, label, releaseDate, asin);
                            list.Add(music);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }

            return list;
        }


        // find music by modelCopy
        public List<Music> FindMusicByModelCopy(ModelCopy mc)
        {
            string query = $"SELECT * FROM cds WHERE cdID = \"{mc.modelID}\";";
            List<Music> music = new List<Music>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int cdID = (int)dr["cdID"];
                            string type = dr["type"] + "";
                            string title = dr["title"] + "";
                            string artist = dr["artist"] + "";
                            string label = dr["label"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string asin = dr["asin"] + "";

                            music.Add(new Music(cdID, type, title, artist, label, releaseDate, asin));
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return music;
        }


        #endregion SearchMusic

        #endregion music

        #region movies

        /*
         * The following methods are made for the movie table
         */

        // Inserts a new movie into the database
        public void CreateMovie(Movie movie)
        {
            string query = $"INSERT INTO movies (title, director, language, subtitles, dubbed, releasedate, runtime) VALUES(\"{movie.Title}\", \"{movie.Director}\", \"{ movie.Language}\", \"{movie.Subtitles}\", \"{movie.Dubbed}\", \"{movie.ReleaseDate.ToShortDateString()}\", \"{movie.RunTime}\");";
            QuerySend(query);
        }

        //       AND MOVIEPRODUCER ASSOCIATIONS ARE DELETED TOO
        public void CreateMovies(params Movie[] movies)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO movies (title, director, language, subtitles, dubbed, releasedate, runtime) VALUES");
            for (int i = 0; i < movies.Length; ++i)
            {
                sb.Append($"(\"{movies[i].Title}\", \"{movies[i].Director}\", \"{ movies[i].Language}\", \"{movies[i].Subtitles}\", \"{movies[i].Dubbed}\", \"{movies[i].ReleaseDate.ToShortDateString()}\", \"{movies[i].RunTime}\"){(i + 1 < movies.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Update a movie's information in the database by MovieID
        public void UpdateMovie(Movie movie)
        {
            string query = $"UPDATE movies SET title = \"{movie.Title}\", director = \"{movie.Director}\", language = \"{movie.Language}\", subtitles = \"{movie.Subtitles}\", dubbed = \"{movie.Dubbed}\", releasedate = \"{movie.ReleaseDate.ToShortDateString()}\", runtime = \"{movie.RunTime}\" WHERE (movieID = \"{movie.MovieId}\");";
            QuerySend(query);
        }

        public void UpdateMovies(params Movie[] movies)
        {
            StringBuilder sb = new StringBuilder("UPDATE movies SET ");
            for (int i = 0; i < movies.Length; ++i)
            {
                sb.Append($"title = \"{movies[i].Title}\", director = \"{movies[i].Director}\", language = \"{movies[i].Language}\", subtitles = \"{movies[i].Subtitles}\", dubbed = \"{movies[i].Dubbed}\", releasedate = \"{movies[i].ReleaseDate.ToShortDateString()}\", runtime = \"{movies[i].RunTime}\" WHERE movieID = \"{movies[i].MovieId}\"{(i + 1 < movies.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Delete movie by movieId from the database
        public void DeleteMovie(Movie movie)
        {
            DeleteMovieActors(movie);
            DeleteMovieProducers(movie);
            string query = $"DELETE FROM movies WHERE (movieID = \"{movie.MovieId}\");";
            QuerySend(query);
        }

        public void DeleteMovies(params Movie[] movies)
        {
            for (int i = 0; i < movies.Length; i++)
            {
                DeleteMovieActors(movies[i]);
                DeleteMovieProducers(movies[i]);
            }

            StringBuilder sb = new StringBuilder("DELETE FROM movies ");

            for (int i = 0; i < movies.Length; ++i)
            {
                sb.Append($"WHERE movieID = \"{movies[i].MovieId}\"{(i + 1 < movies.Length ? "," : ";")}");
            }

            QuerySend(sb.ToString());
        }

        // Retrieve a movie information by id
        public Movie GetMovieById(int id)
        {
            string query = $"SELECT * FROM movies WHERE movieID = \" { id } \";";

            Movie movie = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        if (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return movie;
        }

        // Returns a list of all movies in the db converted to movie object.
        public List<Movie> GetAllMovies()
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = "SELECT * FROM movies;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        #region SearchMovies

        // search movie methods
        public List<Movie> SearchMoviesByTitle(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE LOWER(title) LIKE LOWER('%{MovieString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByDirector(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE LOWER(director) LIKE LOWER('%{MovieString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByLanguage(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE LOWER(language) LIKE LOWER('%{MovieString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesBySubtitles(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE LOWER(subtitles) LIKE LOWER('%{MovieString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByDubbed(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE LOWER(dubbed) LIKE LOWER('%{MovieString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByReleasedate(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE releasedate LIKE LOWER('%{MovieString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByRuntime(string MovieString)
        {
            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = $"SELECT * FROM movies WHERE runtime = \"{MovieString}\";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByProducer(string movieString)
        {
            string namestr = string.Join('|', movieString.Split(' ', '-'));

            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = "SELECT movies.movieID,title,director,language,subtitles,dubbed,releasedate,runtime " +
                "FROM movies INNER JOIN movieproducer ON movieproducer.movieID = movies.movieID " +
                "INNER JOIN person ON person.personID = movieproducer.personID " +
                $"WHERE LOWER(person.firstname) REGEXP(LOWER('{namestr}')) or LOWER(person.lastname) REGEXP(LOWER('{namestr}'));";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Movie> SearchMoviesByActor(string movieString)
        {
            string namestr = string.Join('|', movieString.Split(' ', '-'));

            //Create a list of unknown size to store the result
            List<Movie> list = new List<Movie>();
            Movie movie = null;
            string query = "SELECT movies.movieID,title,director,language,subtitles,dubbed,releasedate,runtime " +
                "FROM movies INNER JOIN movieactor ON movieactor.movieID = movies.movieID " +
                "INNER JOIN person ON person.personID = movieactor.personID " +
                $"WHERE LOWER(person.firstname) REGEXP(LOWER('{namestr}')) or LOWER(person.lastname) REGEXP(LOWER('{namestr}'));";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create movie object and store in list
                        while (dr.Read())
                        {
                            int movieId = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie = new Movie(movieId, title, director, language, subtitles, dubbed, releaseDate, runtime);
                            list.Add(movie);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }



        // find movie by modelCopy
        public List<Movie> FindMovieByModelCopy(ModelCopy mc)
        {
            string query = $"SELECT * FROM movies WHERE movieID = \"{mc.modelID}\";";
            List<Movie> movie = new List<Movie>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int movieID = (int)dr["movieID"];
                            string title = dr["title"] + "";
                            string director = dr["director"] + "";
                            string language = dr["language"] + "";
                            string subtitles = dr["subtitles"] + "";
                            string dubbed = dr["dubbed"] + "";
                            DateTime releaseDate = DateTime.Parse(dr["releasedate"] + "");
                            string runtime = dr["runtime"] + "";

                            movie.Add(new Movie(movieID, title, director, language, subtitles, dubbed, releaseDate, runtime));
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return movie;
        }

        #endregion SearchMovies

        #endregion movies

        #region person

        /*
         * The following methods are made for the person table
         */

        // Inserts a new person into the database
        public void CreatePerson(Person person)
        {
            string query = $"INSERT INTO person (firstname, lastname) VALUES(\"{person.FirstName}\", \"{person.LastName}\");";
            QuerySend(query);
        }

        public void CreatePeople(params Person[] people)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO person (firstname, lastname) VALUES");
            for (int i = 0; i < people.Length; ++i)
            {
                sb.Append($"(\"{people[i].FirstName}\", \"{people[i].LastName}\"){(i + 1 < people.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        public void UpdatePeople(params Person[] people)
        {
            StringBuilder sb = new StringBuilder("UPDATE person SET ");
            for (int i = 0; i < people.Length; ++i)
            {
                sb.Append($"firstname = \"{people[i].FirstName}\", lastname = \"{people[i].LastName}\" WHERE cdID = \"{people[i].PersonId}\"{(i + 1 < people.Length ? "," : ";")}");
            }
            Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        //       AND MOVIEPRODUCER ASSOCIATIONS ARE DELETED TOO
        public void DeletePeople(params Person[] people)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM person ");
            for (int i = 0; i < people.Length; ++i)
            {
                sb.Append($"WHERE personID = \"{ people[i].PersonId}\"{(i + 1 < people.Length ? "," : ";")}");
            }
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // Update a person's information in the database by PersonId
        public void UpdatePerson(Person person)
        {
            string query = $"UPDATE person SET firstname = \"{person.FirstName}\", lastname = \"{person.LastName}\", WHERE (personID = \"{person.PersonId}\");";
            QuerySend(query);
        }

        // Delete person by PersonId from the database
        public void DeletePerson(Person person)
        {
            string query = $"DELETE FROM person WHERE (personID = \"{ person.PersonId}\");";
            QuerySend(query);
        }

        // Retrieve a person information by id
        public Person GetPersonById(int id)
        {
            string query = $"SELECT * FROM person WHERE personID = \" {  id } \";";

            Person person = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        if (dr.Read())
                        {
                            int personId = (int)dr["personID"];
                            string firstname = dr["firstname"] + "";
                            string lastname = dr["lastname"] + "";

                            person = new Person(personId, firstname, lastname);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return person;
        }

        // Returns a list of all person in the db converted to person object.
        public List<Person> GetAllPerson()
        {
            //Create a list of unknown size to store the result
            List<Person> list = new List<Person>();
            Person person = null;
            string query = "SELECT * FROM person;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create music object and store in list
                        while (dr.Read())
                        {
                            int personId = (int)dr["personID"];
                            string firstname = dr["firstname"] + "";
                            string lastname = dr["lastname"] + "";

                            person = new Person(personId, firstname, lastname);

                            list.Add(person);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return list;
        }

        #region movieactor

        /*
         * The following methods are made for the movieActor table
         */

        // Inserts a new movie actor into the database
        public void CreateMovieActor(string mid, string pid)
        {
            string query = $"INSERT INTO movieactor (movieID, personID) VALUES(\"{mid}\", \"{pid}\");";
            QuerySend(query);
        }

        public void CreateMovieActors(int movieId, params int[] actorIds)
        {
            StringBuilder ma = new StringBuilder($"INSERT INTO movieactor (movieID, personID) VALUES ");
            IEnumerable<string> strings = actorIds.Select(id => $"({movieId}, {id})");
            ma.AppendJoin(',', strings);
            ma.Append(";");

            QuerySend(ma.ToString());
        }

        // Delete movie actor by movieID and personID from the database
        public void DeleteMovieActor(string mid, string pid)
        {
            string query = $"DELETE FROM movieactor WHERE (movieID = \"{mid}\" AND personID = \"{pid}\");";
            QuerySend(query);
        }

        // Delete all movie actors by movies array
        public void DeleteMovieActors(Movie movie)
        {
            string query = $"DELETE FROM movieactor WHERE (movieID = \"{movie.MovieId}\";";
            QuerySend(query);
        }

        public void DeleteMovieActors(int movieId, params int[] actorsIds)
        {
            StringBuilder sb = new StringBuilder($"DELETE FROM movieactor WHERE movieID =\"{movieId} \" AND personID IN (");
            sb.AppendJoin(',', actorsIds);
            sb.Append(");");
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // Get all movie actors from a specific movie
        public List<Person> GetAllMovieActors(int movieId)
        {
            //Create a list of unknown size to store the result
            List<Person> list = new List<Person>();
            string query = $"SELECT * FROM person WHERE personID = ANY (SELECT personID FROM movieactor WHERE (movieID = \"{movieId}\"));";
            Person person = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int personId = (int)dr["personID"];
                            string firstname = dr["firstname"] + "";
                            string lastname = dr["lastname"] + "";

                            person = new Person(personId, firstname, lastname);

                            list.Add(person);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return list;
        }

        #endregion movieactor

        #region movieproducer

        /*
         * The following methods are made for the movieProducer table
         */

        // Inserts a new movieProducer into the database
        public void CreateMovieProducer(string mid, string pid)
        {
            string query = $"INSERT INTO movieproducer(movieID, personID) VALUES(\"{mid}\", \"{ pid}\");";
            QuerySend(query);
        }

        //insert producers into the database
        public void CreateMovieProducers(int movieId, params int[] producerIds)
        {
            StringBuilder mp = new StringBuilder($"INSERT INTO movieproducer (movieID, personID) VALUES ");
            IEnumerable<string> strings = producerIds.Select(id => $"({movieId}, {id})");
            mp.AppendJoin(',', strings);
            mp.Append(";");

            QuerySend(mp.ToString());
        }

        // Delete movieProducer by movieID and personID from the database
        public void DeleteMovieProducer(string mid, string pid)
        {
            string query = $"DELETE FROM movieproducer WHERE (movieID = \"{mid}\" AND personID = \"{pid}\");";
            QuerySend(query);
        }

        // Delete all movieProducer by Movie object
        public void DeleteMovieProducers(Movie movie)
        {
            string query = $"DELETE FROM movieproducer WHERE (movieID = \"{movie.MovieId}\";";
            QuerySend(query);
        }

        public void DeleteMovieProducers(int movieId, params int[] producerIds)
        {
            StringBuilder sb = new StringBuilder($"DELETE FROM movieproducer WHERE movieID =\"{movieId} \" AND personID IN (");
            sb.AppendJoin(',', producerIds);
            sb.Append(");");
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // Get all movie producers from a specific movie
        public List<Person> GetAllMovieProducers(int movieId)
        {
            //Create a list of unknown size to store the result
            List<Person> list = new List<Person>();
            string query = $"SELECT * FROM person WHERE personID = ANY (SELECT personID FROM movieproducer WHERE (movieID = \"{movieId}\"));";
            Person person = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int personId = (int)dr["personID"];
                            string firstname = dr["firstname"] + "";
                            string lastname = dr["lastname"] + "";

                            person = new Person(personId, firstname, lastname);

                            list.Add(person);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return list;
        }

        #endregion movieproducer

        #endregion person

        #region books

        public void DeleteBook(Book book)
        {
            string query = $"DELETE FROM books WHERE (bookID = \"{book.BookId}\");";
            QuerySend(query);
        }

        // Deletes several books from the db
        public void DeleteBooks(params Book[] books)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM books WHERE bookID IN (");
            for (int i = 0; i < books.Length; ++i)
            {
                sb.Append($"{books[i].BookId}{(i + 1 < books.Length ? "," : ");")}");
            }
            QuerySend(sb.ToString());
        }

        // Returns a list of all clients in the db converted to client object.
        public List<Book> GetAllBooks()
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = "SELECT * FROM books;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);
                            //Console.Write(book);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        // Return book get from isbn 10
        public Book GetBooksByIsbn(string isbn)
        {
            string query = $"SELECT * FROM books WHERE isbn10 = \"{isbn}\";";
            Book book = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return book;
        }

        // Inserts a new book into the db
        public void CreateBook(Book book)
        {
            string query = $"INSERT INTO books (title, author, format, pages, publisher, date, language, isbn10, isbn13) VALUES(\"{book.Title}\", \"{book.Author}\", \"{book.Format}\", \"{book.Pages}\", \"{book.Publisher}\", \"{book.Date.ToShortDateString()}\", \"{book.Language}\",\"{book.Isbn10}\",\"{book.Isbn13}\")";

            QuerySend(query);
        }

        // Inserts several new books into the db
        public void CreateBooks(params Book[] books)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO books (title, author, format, pages, publisher, date, language, isbn10, isbn13) VALUES");
            for (int i = 0; i < books.Length; ++i)
            {
                sb.Append($"(\"{books[i].Title}\", \"{books[i].Author}\", \"{books[i].Format}\", \"{books[i].Pages}\", \"{books[i].Publisher}\", \"{books[i].Date.ToShortDateString()}\", \"{books[i].Language}\",\"{books[i].Isbn10}\",\"{books[i].Isbn13}\"){(i + 1 < books.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Update a book information in the database by book ID
        // We can add other function to update book
        public void UpdateBook(Book book)
        {
            string query = $"UPDATE books SET title = \"{book.Title}\", author = \"{book.Author}\", format = \"{book.Format}\", pages = \"{book.Pages}\", publisher = \"{book.Publisher}\", date = \"{book.Date.ToShortDateString()}\", language = \"{book.Language}\", isbn10 = \"{book.Isbn10}\", isbn13 = \"{book.Isbn13}\" WHERE (bookID = \"{book.BookId}\");";

            QuerySend(query);
        }

        //update books information
        public void UpdateBooks(params Book[] books)
        {
            StringBuilder sb = new StringBuilder("UPDATE books SET ");
            for (int i = 0; i < books.Length; ++i)
            {
                sb.Append($"title = \"{books[i].Title}\", Author = \"{books[i].Author}\", Format = \"{books[i].Format}\", Pages = \"{books[i].Pages}\", Publisher = \"{books[i].Publisher}\", date = \"{books[i].Date.ToShortDateString()}\",Language = \"{books[i].Language}\", ISBN10 = \"{books[i].Isbn10}\", ISBN13 = \"{books[i].Isbn13}\" WHERE (bookID = \"{books[i].BookId}\"){(i + 1 < books.Length ? "," : ";")}");
            }

            QuerySend(sb.ToString());
        }

        // Update a book information in the database by isbn10
        public void UpdateBookByIsbn(Book book, string isbn10)
        {
            string query = $"UPDATE books SET title = \"{book.Title}\", author = \"{book.Author}\", format = \"{book.Format}\", pages = \"{book.Pages}\", publisher = \"{book.Publisher}\", date = \"{book.Date.ToShortDateString()}\", language = \"{book.Language}\", isbn13 = \"{book.Isbn13}\" WHERE (isbn10 = \"{isbn10}\");";

            QuerySend(query);
        }

        // Delete a book information in db by isbn10
        public void DeleteBookByIsbn10(string isbn10)
        {
            string query = $"DELETE FROM books WHERE (isbn10 = \"{isbn10}\");";

            QuerySend(query);
        }

        public Book GetBookById(int id)
        {
            string query = $"SELECT * FROM books WHERE bookID = \" { id } \";";

            Book book = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        if (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            book = new Book(bookId, title, author, format,
                                pages, publisher, year, language, isbn10, isbn13);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return book;
        }

        public Book GetBookByIsbn10(string Isbn10)
        {
            string query = $"SELECT * FROM books WHERE isbn10 = \" { Isbn10 } \";";

            Book book = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        if (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["year"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            book = new Book(bookId, title, author, format,
                                pages, publisher, year, language, isbn10, isbn13);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return book;
        }

        public Book GetBookByIsbn13(string Isbn13)
        {
            string query = $"SELECT * FROM books WHERE isbn13 = \" { Isbn13 } \";";

            Book book = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create magazine object and store in list
                        if (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["year"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            book = new Book(bookId, title, author, format,
                                pages, publisher, year, language, isbn10, isbn13);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return book;
        }

        #region SearchBooks

        public List<Book> SearchBooksByIsbn10(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE isbn10 = \"{SearchString}\";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByIsbn13(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE isbn13 = \"{SearchString}\";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByTitle(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE LOWER(title) LIKE LOWER('%{SearchString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByAuthor(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE LOWER(author) LIKE LOWER('%{SearchString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByFormat(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE LOWER(format) LIKE LOWER('%{SearchString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByPages(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE pages = \"{SearchString}\";";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByPublisher(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE LOWER(publisher) LIKE LOWER('%{SearchString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByDate(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE LOWER(date) LIKE LOWER('%{SearchString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        public List<Book> SearchBooksByLanguage(string SearchString)
        {
            //Create a list of unknown size to store the result
            List<Book> books = new List<Book>();
            string query = $"SELECT * FROM books WHERE LOWER(language) LIKE LOWER('%{SearchString}%');";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int bookId = (int)dr["bookID"];
                            string title = dr["title"] + "";
                            string author = dr["author"] + "";
                            string format = dr["format"] + "";
                            int pages = (int)dr["pages"];
                            string publisher = dr["publisher"] + "";
                            DateTime year = DateTime.Parse(dr["date"] + "");
                            string language = dr["language"] + "";
                            string isbn10 = dr["isbn10"] + "";
                            string isbn13 = dr["isbn13"] + "";

                            Book book = new Book(bookId, title, author, format, pages, publisher, year, language, isbn10, isbn13);

                            books.Add(book);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return books;
        }

        #endregion SearchBooks
        #endregion books

        #region modelCopy

        // Deletes several books from the db
        public void DeleteModelCopies(params ModelCopy[] mcs)
        {
            StringBuilder sb = new StringBuilder("DELETE FROM modelcopy WHERE id IN (");
            for (int i = 0; i < mcs.Length; ++i)
            {
                sb.Append($"{mcs[i].id}{(i + 1 < mcs.Length ? "," : ");")}");
            }
            QuerySend(sb.ToString());
        }

        // Returns a list of all clients in the db converted to client object.
        public List<ModelCopy> GetAllModelCopies()
        {
            //Create a list of unknown size to store the result
            List<ModelCopy> mcs = new List<ModelCopy>();
            string query = "SELECT * FROM modelcopy;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        while (dr.Read())
                        {
                            int id = (int)dr["id"];
                            int modelID = (int)dr["modelID"];
                            int modelType = (int)dr["modelType"];
                            int borrowerID = (int)dr["borrowerID"];
                            DateTime borrowedDate = (DateTime)dr["borrowedDate"];
                            DateTime returnDate = (DateTime)dr["returnDate"];

                            ModelCopy mc = new ModelCopy { id = id, modelID = modelID, borrowerID = borrowerID, borrowedDate = borrowedDate, modelType = (Constants.TypeConstants.TypeEnum)modelType, returnDate = returnDate };
                            //Console.Write(book);

                            mcs.Add(mc);
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return mcs;
        }

        // Inserts several new books into the db
        public void CreateModelCopies(params ModelCopy[] mcs)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO modelcopy (modelID, modelType) VALUES");
            for (int i = 0; i < mcs.Length; ++i)
            {
                sb.Append($"(\"{mcs[i].modelID}\", \"{mcs[i].modelType}\"){(i + 1 < mcs.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        //update books information
        public void UpdateModelCopies(params ModelCopy[] mcs)
        {
            StringBuilder sb = new StringBuilder("UPDATE modelcopy SET ");
            for (int i = 0; i < mcs.Length; ++i)
            {
                sb.Append($"modelType = \"{mcs[i].modelType}\", modelID = \"{mcs[i].modelID}\", borrowerID = \"{mcs[i].borrowerID}\", borrowedDate = \"{mcs[i].borrowedDate}\", returnDate = \"{mcs[i].returnDate}\" WHERE (ID = \"{mcs[i].id}\"){(i + 1 < mcs.Length ? "," : ";")}");
            }

            QuerySend(sb.ToString());
        }

        public ModelCopy GetModelCopyById(int id)
        {
            string query = $"SELECT * FROM modelcopy WHERE ID = \" { id } \";";

            ModelCopy mc = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create book object and store in list
                        if (dr.Read())
                        {
                            int Id = (int)dr["id"];
                            int modelID = (int)dr["modelID"];
                            int modelType = (int)dr["modelType"];
                            int borrowerID = (int)dr["borrowerID"];
                            DateTime borrowedDate = (DateTime)dr["borrowedDate"];
                            DateTime returnDate = (DateTime)dr["returnDate"];

                            mc = new ModelCopy { id = id, modelID = modelID, borrowerID = borrowerID, borrowedDate = borrowedDate, modelType = (Constants.TypeConstants.TypeEnum)modelType, returnDate = returnDate };
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
            return mc;
        }

        //Find modelCopies of model by model ID, returns list of modelCopy
        public List<ModelCopy> FindModelCopiesOfModel(int modelId, Constants.TypeConstants.TypeEnum enumType, BorrowType borrowId = BorrowType.Any)
        {
            int mType = (int)enumType;
            string query = $"SELECT * FROM modelcopies WHERE modelID = \"{modelId}\" AND modelType = \"{mType}\"";
            switch (borrowId)
            {
                case BorrowType.Borrowed:
                    query += " AND NOT borrowerID IS NULL;";
                        break;
                case BorrowType.NotBorrowed:
                    query += " AND borrowerID IS NULL;";
                    break;
                case BorrowType.Any:
                    query += ";";
                    break;
            }
            List<ModelCopy> modelCopies = new List<ModelCopy>();

            //Open connection

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data
                        if (dr.Read())
                        {
                            int id = (int)dr["id"];
                            int modelType = (int)dr["modelType"];
                            int modelID = (int)dr["modelID"];
                            int borrowerID = (int)dr["borrowerID"];
                            DateTime borrowedDate = (DateTime)dr["borrowedDate"];
                            DateTime returnDate = (DateTime)dr["returnDate"];

                            modelCopies.Add(new ModelCopy
                            {
                                id = id,
                                modelType = (TheLibraryIsOpen.Constants.TypeConstants.TypeEnum)modelType,
                                modelID = modelID,
                                borrowerID = borrowerID,
                                borrowedDate = borrowedDate,
                                returnDate = returnDate
                            });
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return modelCopies;
        }


        //Find modelCopies of client by Client ID, returns list of modelCopy
        public List<ModelCopy> FindModelCopiesOfClient(int clientId)
        {
            string query = $"SELECT * FROM modelcopies WHERE borrowerID = \"{clientId}\";";
            List<ModelCopy> modelCopies = new List<ModelCopy>();

            //Open connection

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data
                        if (dr.Read())
                        {
                            int id = (int)dr["id"];
                            int modelType = (int)dr["modelType"];
                            int modelID = (int)dr["modelID"];
                            int borrowerID = (int)dr["borrowerID"];
                            DateTime borrowedDate = (DateTime)dr["borrowedDate"];
                            DateTime returnDate = (DateTime)dr["returnDate"];

                            modelCopies.Add(new ModelCopy
                            {
                                id = id,
                                modelType = (TheLibraryIsOpen.Constants.TypeConstants.TypeEnum)modelType,
                                modelID = modelID,
                                borrowerID = borrowerID,
                                borrowedDate = borrowedDate,
                                returnDate = returnDate
                            });
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return modelCopies;
        }


        //counts number of copies borrowed of a specific model

        public int CountModelCopiesOfModel(int modelId, int mType, BorrowType borrowId)
        {
            string query = null;
            switch (borrowId)
            {
                case BorrowType.Borrowed:
                    query = $"SELECT COUNT(modelID) FROM modelcopies WHERE modelID = \"{modelId}\" AND modelType = \"{mType}\" AND NOT borrowerID IS NULL;";
                    break;
                case BorrowType.NotBorrowed:
                    query = $"SELECT COUNT(modelID) FROM modelcopies WHERE modelID = \"{modelId}\" AND modelType = \"{mType}\" AND borrowerID IS NULL;";
                    break;
                case BorrowType.Any:
                    query = $"SELECT COUNT(modelID) FROM modelcopies WHERE modelID = \"{modelId}\" AND modelType = \"{mType}\";";
                    break;
            }
            int count = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    count = Convert.ToInt32(cmd.ExecuteScalar());

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return count;
        }

        #endregion

        #region logs

        public List<Log> GetAllLogs()
        {
            //Create a list of unknown size to store the result
            List<Log> list = new List<Log>();
            string query = "SELECT * FROM logs;";

            //Open connection

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        while (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];

                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Log> GetLogsByDate(DateTime dateTime, bool exact)
        {
            List<Log> list = new List<Log>();
            string query = "";
            if (!exact)
            {
                string dateString = dateTime.ToShortDateString();
                query = $"SELECT * FROM logs WHERE DATE(transactionTime) = '{dateString}';";
            }
            else
            {
                string dateTimeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
                query = $"SELECT * FROM logs WHERE transactionTime = '{dateTimeString}';";
            }

            //Open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];


                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;

        }

        public List<Log> GetLogsByClientID(int id)
        {
            List<Log> list = new List<Log>();
            string query = $"SELECT * FROM logs WHERE clientID = \"{id}\";";

            //Open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];


                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Log> GetLogsByCopyID(int copyID)
        {
            List<Log> list = new List<Log>();
            string query = $"SELECT * FROM logs WHERE modelCopyID = \"{copyID}\";";

            //Open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];


                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Log> GetLogsByTransaction(TransactionType transac)
        {
            List<Log> list = new List<Log>();
            string query = $"SELECT * FROM logs WHERE transaction = \"{(int)transac}\";";

            //Open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];


                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Log> GetLogsByModelTypeAndId(TypeEnum type, int id)
        {
            List<Log> list = new List<Log>();
            string query = $"SELECT logs.logID, logs.clientID, logs.modelCopyID, logs.transaction, logs.transactionTime FROM logs INNER JOIN modelcopies ON modelcopies.id = logs.modelCopyID WHERE modelcopies.modelID = \"{id}\" AND modelcopies.modelType = \"{(int)type}\" ;";

            //Open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];


                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public List<Log> GetLogsByPeriod(DateTime dateStart, DateTime dateEnd, bool exact)
        {
            List<Log> list = new List<Log>();
            string query = "";
            if (!exact)
            {
                String dateStartString = dateStart.ToShortDateString();
                String dateEndString = dateEnd.ToShortDateString();
                query = $"SELECT * FROM logs WHERE DATE(transactionTime) BETWEEN '{dateStartString}' AND '{dateEndString}';";
            }
            else
            {
                String dateStartString = dateStart.ToString("yyyy-MM-dd HH:mm:ss");
                String dateEndString = dateEnd.ToString("yyyy-MM-dd HH:mm:ss");
                query = $"SELECT * FROM logs WHERE transactionTime BETWEEN '{dateStartString}' AND '{dateEndString}';";
            }

            //Open connection
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    //Create a data reader and Execute the command
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        //Read the data, create client object and store in list
                        if (dr.Read())
                        {
                            int logID = (int)dr["logID"];
                            int clientID = (int)dr["clientID"];
                            int modelCopyID = (int)dr["modelCopyID"];
                            TransactionType transaction = (TransactionType)Enum.Parse(typeof(TransactionType), ((int)dr["transaction"]).ToString());
                            DateTime transactionTime = (DateTime)dr["transactionTime"];


                            list.Add(new Log(logID, clientID, modelCopyID, transaction, transactionTime));
                        }
                    }

                }
                catch (Exception e) { Console.WriteLine(e); }
            }
            return list;
        }

        public void AddLog(Log log)
        {
            string query = $"INSERT INTO logs (clientID, modelCopyID, transaction, transactionTime) VALUES(\"{log.ClientID}\", \"{log.ModelCopyID}\", \"{(int)log.Transaction}\", \"{log.TransactionTime.ToString("yyyy-MM-dd HH:mm:ss")}\");";
            QuerySend(query);
        }

        public void DeleteLog(Log log)
        {
            string query = $"DELETE FROM logs WHERE (logID = \"{log.LogID}\");";
            QuerySend(query);
        }


        public void AddLogs(params Log[] logs)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO logs (clientID, modelCopyID, transaction, transactionTime) VALUES");
            for (int i = 0; i < logs.Length; ++i)
            {
                sb.Append($"(\"{logs[i].ClientID}\", \"{logs[i].ModelCopyID}\", \"{(int)logs[i].Transaction}\", \"{logs[i].TransactionTime.ToString("yyyy-MM-dd HH:mm:ss")}\"){(i + 1 < logs.Length ? ", " : ";")}");
            }
            QuerySend(sb.ToString());
        }

        public void DeleteLogs(params Log[] logs)
        {
            StringBuilder sb = new StringBuilder($"DELETE FROM logs WHERE ");
            for (int i = 0; i < logs.Length; ++i)
            {
                sb.Append($"logID = \"{logs[i].LogID}\") {(i + 1 < logs.Length ? " OR " : ";")}");
            }

            QuerySend(sb.ToString());
        }

        public void UpdateLogs(params Log[] logs)
        {
            StringBuilder sb = new StringBuilder("UPDATE logs set");
            for (int i = 0; i < logs.Length; ++i)
            {
                sb.Append($"clientID = \"{logs[i].ClientID}\", modelCopyID = \"{logs[i].ModelCopyID}\",transaction = \"{(int)logs[i].Transaction}\", transactionTime = \"{logs[i].TransactionTime.ToString("yyyy-MM-dd HH:mm:ss")}\"){(i + 1 < logs.Length ? ", " : ";")}");
            }
            QuerySend(sb.ToString());
        }

        public void ClearAllLogsBefore(DateTime date)
        {
            StringBuilder sb = new StringBuilder($"DELETE FROM logs WHERE transactionDate <= \"{date.ToString("yyyy-MM-dd HH:mm:ss")}\"");

            QuerySend(sb.ToString());
        }

        public List<Log> TransactionUpdate(int clientID, List<Log> transactionList)
        {
            
            List<Log> logs = null;
            List<ModelCopy> items = null;

            string query = $"SELECT MAX(logID) FROM logs";

            int lastLogID = int.Parse(query);

            for(int i = 0; i< transactionList.Count; i++)
            {
                
                switch (transactionList[i].Transaction)
                {
                    case TransactionType.Borrow:
                        {
                            Log newLog = new Log();
                           
                            newLog.ClientID = clientID;
                            newLog.ModelID = transactionList[i].ModelID;
                            newLog.ModelType = transactionList[i].ModelType;
                            newLog.TransactionTime = DateTime.Now;

                            items = FindModelCopiesOfModel(transactionList[i].ModelID, transactionList[i].ModelType, BorrowType.Borrowed);

                            if (items.Count > 0)
                            {
                                newLog.LogID = lastLogID;

                                items[0].borrowerID = clientID;
                                items[0].borrowedDate = newLog.TransactionTime;
                                items[0].returnDate = items[0].borrowedDate.AddDays(7.0d);
                                lastLogID++;

                                newLog.ModelCopyID = items[0].id;

                                newLog.Transaction = transactionList[i].Transaction;
                                AddLog(newLog);
                                logs.Add(newLog);
                            }
                            
                            break;
                        }
                    case TransactionType.Return:
                        {
                            Log newLog = new Log();
                            
                            newLog.ClientID = clientID;
                            newLog.ModelID = transactionList[i].ModelID;
                            newLog.ModelType = transactionList[i].ModelType;
                            newLog.TransactionTime = DateTime.Now;

                            items = FindModelCopiesOfModel(transactionList[i].ModelID, transactionList[i].ModelType, BorrowType.Borrowed);

                            if (items.Count > 0)
                            {
                                newLog.LogID = lastLogID;

                                items[0].borrowerID = 0;
                                items[0].borrowedDate = new DateTime();
                                items[0].returnDate = new DateTime();
                                lastLogID++;
                                
                                newLog.ModelCopyID = items[0].id;
                                
                                newLog.Transaction = transactionList[i].Transaction;
                                AddLog(newLog);
                                logs.Add(newLog);
                            }

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

                return logs;
        }
        #endregion

    }
}
