using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BattleshipServer
{
    public static class RegisterFilter
    {
        public static string[] Cities = GetCities();

        public static string[] GetCities(string filename= "C:\\Users\\owner\\source\\repos\\Battleship\\BattleshipServer\\cities.txt")
        {
            try
            {
                return System.IO.File.ReadAllLines(filename);
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error reading cities file: " + e.Message);
                return new string[0];
            }
        }

        public static bool CheckUsername(string username)
        {
            return Regex.IsMatch(username, @"[a-zA-Z0-9_]{4,16}$");
        }

        public static bool CheckPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$");
        }

        public static bool CheckEmail(string email)
        {
            return Regex.IsMatch(email, @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])");
        }

        public static bool CheckFirstName(string firstName)
        {
            return Regex.IsMatch(firstName, @"^[a-zA-Z]{2,16}$");
        }

        public static bool CheckLastName(string lastName)
        {
            return Regex.IsMatch(lastName, @"^[a-zA-Z]{2,16}$");
        }

        public static bool CheckCity(string city)
        {
            return Cities.Contains(city);
        }
    }
}
