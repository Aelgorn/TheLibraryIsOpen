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

    One things for query language:
    Don't put space between {}. Ex : \"{ isbn13 }\" is wrong, and \"{isbn13}\" is right
 */
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading;
using TheLibraryIsOpen.Models.DBModels;

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

        // Selects a client by id and returns a client object.
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
        #endregion

        #region magazines
        /*
         *  Magazine Table methods
         */

        public void CreateMagazine(Magazine magazine)
        {
            string query =
                $"INSERT INTO magazines (title, publisher, language, date, isbn10, isbn13) VALUES(\"{magazine.Title}\",\"{magazine.Publisher}\",\"{magazine.Language}\",\"{magazine.Date}\",\"{magazine.Isbn10}\",\"{magazine.Isbn13}\");";

            QuerySend(query);
        }


        public void CreateMagazines(params Magazine[] magazines)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO magazines (title, publisher, language, date, isbn10, isbn13) VALUES");
            for (int i = 0; i < magazines.Length; ++i)
            {
                sb.Append($"(\"{magazines[i].Title}\",\"{magazines[i].Publisher}\",\"{magazines[i].Language}\",\"{magazines[i].Date}\",\"{magazines[i].Isbn10}\",\"{magazines[i].Isbn13}\"){(i + 1 < magazines.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }


        public void UpdateMagazines(params Magazine[] magazines)
        {
            StringBuilder sb = new StringBuilder("UPDATE magazines SET ");
            for (int i = 0; i < magazines.Length; ++i)
            {
                sb.Append($"title = \"{magazines[i].Title}\", publisher = \"{magazines[i].Publisher}\", language = \"{magazines[i].Language}\", date = \"{magazines[i].Date}\", isbn10 = \"{magazines[i].Isbn10}\", isbn13 = \"{magazines[i].Isbn13}\" WHERE (magazineID = \"{magazines[i].MagazineId}\"){(i + 1 < magazines.Length ? "," : ";")}");
            }
            // Console.WriteLine(sb.ToString());
            QuerySend(sb.ToString());
        }

        // need improve
        public void UpdateMagazine(Magazine magazine)
        {
            string query = $"UPDATE magazines SET title = \"{magazine.Title}\", publisher = \"{magazine.Publisher}\", language = \"{magazine.Language}\", date = \"{magazine.Date}\", isbn10 = \"{magazine.Isbn10}\", isbn13 = \"{magazine.Isbn13}\" WHERE (magazineID = \"{magazine.MagazineId}\");";

            QuerySend(query);

        }

        // update magazine by ID
        public void UpdateMagazine(Magazine magazine, int magazineID)
        {
            string query = $"UPDATE magazines SET title = \"{magazine.Title}\", publisher = \"{magazine.Publisher}\", language = \"{magazine.Language}\", date = \"{magazine.Date}\", isbn10 = \"{magazine.Isbn10}\", isbn13 = \"{magazine.Isbn13}\" WHERE (magazineID = \"{magazineID}\");";

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
                            string date = dr["date"] + "";
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
                            string date = dr["date"] + "";
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
        #endregion
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
                sb.Append($"(\"{music[i].Type}\", \"{music[i].Title}\", \"{music[i].Artist}\", \"{music[i].Label}\", \"{music[i].ReleaseDate}\", \"{music[i].Asin}\"){(i + 1 < music.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Update a music's information in the database by MusicId
        public void UpdateMusic(params Music[] music)
        {

            StringBuilder sb = new StringBuilder("UPDATE cds SET ");
            for (int i = 0; i < music.Length; ++i)
            {
                sb.Append($"type = \"{music[i].Type}\", title = \"{music[i].Title}\",artist = \"{music[i].Artist}\", label = \"{music[i].Label}\", releasedate = \"{music[i].ReleaseDate}\", asin = \"{music[i].Asin}\" WHERE cdID = \"{music[i].MusicId}\"{(i + 1 < music.Length ? "," : ";")}");
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
                            string releaseDate = dr["releasedate"] + "";
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
                            string releaseDate = dr["releasedate"] + "";
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

        #endregion
        #region movies
        /*
         * The following methods are made for the movie table
         */

        // Inserts a new movie into the database
        public void CreateMovie(Movie movie)
        {
            string query = $"INSERT INTO movies (title, language, subtitles, dubbed, releasedate, runtime) VALUES(\"{movie.Title}\", \"{ movie.Language}\", \"{movie.Subtitles}\", \"{movie.Dubbed}\", \"{movie.ReleaseDate}\", \"{movie.RunTime}\");";
            QuerySend(query);
        }

        //       AND MOVIEPRODUCER ASSOCIATIONS ARE DELETED TOO
        public void CreateMovies(params Movie[] movies)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO movies (title, language, subtitles, dubbed, releasedate, runtime) VALUES");
            for (int i = 0; i < movies.Length; ++i)
            {
                sb.Append($"(\"{movies[i].Title}\", \"{ movies[i].Language}\", \"{movies[i].Subtitles}\", \"{movies[i].Dubbed}\", \"{movies[i].ReleaseDate}\", \"{movies[i].RunTime}\"){(i + 1 < movies.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Update a movie's information in the database by MovieID
        public void UpdateMovie(Movie movie)
        {
            string query = $"UPDATE movies SET title = \"{movie.Title}\", director = \"{movie.Director}\", language = \"{movie.Language}\", subtitles = \"{movie.Subtitles}\", dubbed = \"{movie.Dubbed}\", releasedate = \"{movie.ReleaseDate}\", runtime = \"{movie.RunTime}\" WHERE (movieID = \"{movie.MovieId}\");";
            QuerySend(query);
        }

        public void UpdateMovies(params Movie[] movies)
        {
            StringBuilder sb = new StringBuilder("UPDATE movies SET ");
            for (int i = 0; i < movies.Length; ++i)
            {
                sb.Append($"title = \"{movies[i].Title}\", director = \"{movies[i].Director}\", language = \"{movies[i].Language}\", subtitles = \"{movies[i].Subtitles}\", dubbed = \"{movies[i].Dubbed}\"WHERE cdID = \"{movies[i].MovieId}\"{(i + 1 < movies.Length ? "," : ";")}");
            }
            Console.WriteLine(sb.ToString());
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
                            string releaseDate = dr["releasedate"] + "";
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
                            string releaseDate = dr["releasedate"] + "";
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
            for (int i = 0; i < actorIds.Length; i++)
            {
                ma.Append($"({movieId}, {actorIds[i]})");
            }
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
        #endregion
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
            for (int i = 0; i < producerIds.Length; i++)
            {
                mp.Append($"({movieId}, {producerIds[i]})");
            }
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
        #endregion
        #endregion
        #endregion
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
                            string year = dr["date"] + "";
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
                            string year = dr["date"] + "";
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

            string query = $"INSERT INTO books (title, author, format, pages, publisher, date, language, isbn10, isbn13) VALUES(\"{book.Title}\", \"{book.Author}\", \"{book.Format}\", \"{book.Pages}\", \"{book.Publisher}\", \"{book.Date}\", \"{book.Language}\",\"{book.Isbn10}\",\"{book.Isbn13}\")";

            QuerySend(query);
        }

        // Inserts several new books into the db
        public void CreateBooks(params Book[] books)
        {
            StringBuilder sb = new StringBuilder("INSERT INTO books (title, author, format, pages, publisher, date, language, isbn10, isbn13) VALUES");
            for (int i = 0; i < books.Length; ++i)
            {
                sb.Append($"(\"{books[i].Title}\", \"{books[i].Author}\", \"{books[i].Format}\", \"{books[i].Pages}\", \"{books[i].Publisher}\", \"{books[i].Date}\", \"{books[i].Language}\",\"{books[i].Isbn10}\",\"{books[i].Isbn13}\"){(i + 1 < books.Length ? "," : ";")}");
            }
            QuerySend(sb.ToString());
        }

        // Update a book information in the database by book ID
        // We can add other function to update book
        public void UpdateBook(Book book)
        {
            string query = $"UPDATE books SET title = \"{book.Title}\", author = \"{book.Author}\", format = \"{book.Format}\", pages = \"{book.Pages}\", publisher = \"{book.Publisher}\", date = \"{book.Date}\", language = \"{book.Language}\", isbn10 = \"{book.Isbn10}\", isbn13 = \"{book.Isbn13}\" WHERE (bookID = \"{book.BookId}\");";

            QuerySend(query);
        }

        //update books information
        public void UpdateBooks(params Book[] books)
        {
            StringBuilder sb = new StringBuilder("UPDATE books SET ");
            for (int i = 0; i < books.Length; ++i)
            {
                sb.Append($"title = \"{books[i].Title}\", Author = \"{books[i].Author}\", Format = \"{books[i].Format}\", Pages = \"{books[i].Pages}\", Publisher = \"{books[i].Publisher}\", Language = \"{books[i].Language}\" ISBN-10 = \"{books[i].Isbn10}\", ISBN-13 = \"{books[i].Isbn13}\" WHERE (bookID = \"{books[i].BookId}\"){(i + 1 < books.Length ? "," : ";")}");
            }

            QuerySend(sb.ToString());
        }

        // Update a book information in the database by isbn10
        public void UpdateBookByIsbn(Book book, string isbn10)
        {
            string query = $"UPDATE books SET title = \"{book.Title}\", author = \"{book.Author}\", format = \"{book.Format}\", pages = \"{book.Pages}\", publisher = \"{book.Publisher}\", date = \"{book.Date}\", language = \"{book.Language}\", isbn13 = \"{book.Isbn13}\" WHERE (isbn10 = \"{isbn10}\");";

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
                            string year = dr["date"] + "";
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
                            string year = dr["year"] + "";
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
                            string year = dr["year"] + "";
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
            string query = $"SELECT * FROM books WHERE isbn10 = \" { SearchString } \";";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE isbn13 = \" { SearchString } \";";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE LOWER(title) = LOWER(\" { SearchString } \");";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE LOWER(author) = LOWER(\" { SearchString } \");";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE LOWER(format) = LOWER(\" { SearchString } \");";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE pages = \" { SearchString } \";";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE LOWER(publisher) = LOWER(\" { SearchString } \");";

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
                            string year = dr["date"] + "";
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
            string query = $"SELECT * FROM books WHERE LOWER(language) = LOWER(\" { SearchString } \");";

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
                            string year = dr["date"] + "";
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
        #endregion






        #endregion
    }
}