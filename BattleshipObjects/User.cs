using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipObjects
{
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public Genders Gender { get; set; }

        public User(string Username, string Email, string FirstName, string LastName, string City, Genders Gender)
        {
            this.Username = Username;
            this.Email = Email;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.City = City;
            this.Gender = Gender;
        }

        public User(byte[] bytes, int offset=0, int length=-1)
        {
            if (length == -1)
                length = bytes.Length - offset;
            int emailOffset = BitConverter.ToInt32(bytes, offset);
            int firstNameOffset = BitConverter.ToInt32(bytes, offset + 4);
            int lastNameOffset = BitConverter.ToInt32(bytes, offset + 8);
            int cityOffset = BitConverter.ToInt32(bytes, offset + 12);
            Username = Encoding.UTF8.GetString(bytes, offset + 16, emailOffset - 16);
            Email = Encoding.UTF8.GetString(bytes, offset + emailOffset, firstNameOffset - emailOffset);
            FirstName = Encoding.UTF8.GetString(bytes, offset + firstNameOffset, lastNameOffset - firstNameOffset);
            LastName = Encoding.UTF8.GetString(bytes, offset + lastNameOffset, cityOffset - lastNameOffset);
            City = Encoding.UTF8.GetString(bytes, offset + cityOffset, length - cityOffset - 1);
            Gender = (Genders)bytes[offset + length - 1];
        }

        public byte[] ToBytes()
        {
            byte[] usernameBytes = Encoding.UTF8.GetBytes(Username);
            byte[] emailBytes = Encoding.UTF8.GetBytes(Email);
            byte[] firstNameBytes = Encoding.UTF8.GetBytes(FirstName);
            byte[] lastNameBytes = Encoding.UTF8.GetBytes(LastName);
            byte[] cityBytes = Encoding.UTF8.GetBytes(City);
            int emailOffset = 16 + usernameBytes.Length;
            int firstNameOffset = emailOffset + emailBytes.Length;
            int lastNameOffset = firstNameOffset + firstNameBytes.Length;
            int cityOffset = lastNameOffset + lastNameBytes.Length;
            int length = cityOffset + cityBytes.Length + 1;
            byte[] bytes = new byte[length];
            Array.Copy(BitConverter.GetBytes(emailOffset), 0, bytes, 0, 4);
            Array.Copy(BitConverter.GetBytes(firstNameOffset), 0, bytes, 4, 4);
            Array.Copy(BitConverter.GetBytes(lastNameOffset), 0, bytes, 8, 4);
            Array.Copy(BitConverter.GetBytes(cityOffset), 0, bytes, 12, 4);
            Array.Copy(usernameBytes, 0, bytes, 16, usernameBytes.Length);
            Array.Copy(emailBytes, 0, bytes, emailOffset, emailBytes.Length);
            Array.Copy(firstNameBytes, 0, bytes, firstNameOffset, firstNameBytes.Length);
            Array.Copy(lastNameBytes, 0, bytes, lastNameOffset, lastNameBytes.Length);
            Array.Copy(cityBytes, 0, bytes, cityOffset, cityBytes.Length);
            bytes[length - 1] = (byte)Gender;
            return bytes;
        }

        public bool IsEquals(User user)
        {
            return Username == user.Username && Email == user.Email && FirstName == user.FirstName && LastName == user.LastName && City == user.City && Gender == user.Gender;
        }
    }
}
