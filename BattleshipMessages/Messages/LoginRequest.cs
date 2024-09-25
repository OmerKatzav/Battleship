using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public class LoginRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to 4: password offset
         * 5 to (passwordOffset-1): username
         * passwordOffset to end: password
        */

        public MessageTypes MessageType => MessageTypes.LoginRequest;
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginRequest(string Username, string Password)
        {
            this.Username = Username;
            this.Password = Password;
        }

        public LoginRequest(byte[] request)
        {
            int passwordOffset = BitConverter.ToInt32(request, 1);
            Username = Encoding.UTF8.GetString(request, 5, passwordOffset - 5);
            Password = Encoding.UTF8.GetString(request, passwordOffset, request.Length - passwordOffset);
        }

        public byte[] ToBytes()
        {
            byte[] usernameBytes = Encoding.UTF8.GetBytes(Username);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);
            byte[] request = new byte[5 + usernameBytes.Length + passwordBytes.Length];
            request[0] = (byte)MessageType;
            Array.Copy(BitConverter.GetBytes(5 + usernameBytes.Length), 0, request, 1, 4);
            Array.Copy(usernameBytes, 0, request, 5, usernameBytes.Length);
            Array.Copy(passwordBytes, 0, request, 5 + usernameBytes.Length, passwordBytes.Length);
            return request;
        }
    }
}
