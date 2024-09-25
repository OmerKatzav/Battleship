using BattleshipObjects;
using BattleshipUtils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipServer
{
    public class Database
    {
        readonly SqlConnection Connection;

        public Database(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        public void Connect()
        {
            Connection.Open();
        }

        public void Disconnect()
        {
            Connection.Close();
        }

        public bool IsExistingUser(string username)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM UserData WHERE Username = @Username", Connection);
            command.Parameters.AddWithValue("@Username", username);
            SqlDataReader reader = command.ExecuteReader();
            bool result = reader.HasRows;
            reader.Close();
            return result;
        }

        public bool IsExistingEmail(string email)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM UserData WHERE Email = @Email", Connection);
            command.Parameters.AddWithValue("@Email", email);
            SqlDataReader reader = command.ExecuteReader();
            bool result = reader.HasRows;
            reader.Close();
            return result;
        }

        public bool CheckPassword(string username, string password)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM LoginData WHERE Username = @Username", Connection);
            command.Parameters.AddWithValue("@Username", username);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            byte[] hash = (byte[])reader["PasswordHash"];
            byte[] salt = (byte[])reader["PasswordSalt"];
            reader.Close();
            byte[] inputHash = HashUtils.Hash(password, salt);
            return hash.SequenceEqual(inputHash);
        }

        public void AddUser(User user, string password)
        {
            SqlCommand dataCommand = new SqlCommand("INSERT INTO UserData (Username, Email, FirstName, LastName, City, Gender) VALUES (@Username, @Email, @FirstName, @LastName, @City, @Gender)", Connection);
            dataCommand.Parameters.AddWithValue("@Username", user.Username);
            dataCommand.Parameters.AddWithValue("@Email", user.Email);
            dataCommand.Parameters.AddWithValue("@FirstName", user.FirstName);
            dataCommand.Parameters.AddWithValue("@LastName", user.LastName);
            dataCommand.Parameters.AddWithValue("@City", user.City);
            dataCommand.Parameters.AddWithValue("@Gender", (byte)user.Gender);
            dataCommand.ExecuteNonQuery();
            byte[] salt = new byte[16];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            byte[] hash = HashUtils.Hash(password, salt);
            SqlCommand loginCommand = new SqlCommand("INSERT INTO LoginData (Username, PasswordHash, PasswordSalt) VALUES (@Username, @PasswordHash, @PasswordSalt)", Connection);
            loginCommand.Parameters.AddWithValue("@Username", user.Username);
            loginCommand.Parameters.AddWithValue("@PasswordHash", hash);
            loginCommand.Parameters.AddWithValue("@PasswordSalt", salt);
            loginCommand.ExecuteNonQuery();
        }

        public User GetUserData(string username)
        {
            SqlCommand command = new SqlCommand("SELECT * FROM UserData WHERE Username = @Username", Connection);
            command.Parameters.AddWithValue("@Username", username);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            string email = (string)reader["Email"];
            string firstName = (string)reader["FirstName"];
            string lastName = (string)reader["LastName"];
            string city = (string)reader["City"];
            Genders gender = (Genders)((byte[])reader["Gender"])[0];
            reader.Close();
            return new User(username, email, firstName, lastName, city, gender);
        }
    }
}
