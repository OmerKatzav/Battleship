using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipObjects;

namespace BattleshipMessages.Messages
{
    public class RegisterRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to 4: password offset
         * 5 to (passwordOffset-1): user
         * passwordOffset to end: password
        */
        public MessageTypes MessageType => MessageTypes.RegisterRequest;
        public User User { get; set; }
        public string Password { get; set; }

        public RegisterRequest(User User, string Password)
        {
            this.User = User;
            this.Password = Password;
        }

        public RegisterRequest(byte[] request)
        {
            int passwordOffset = BitConverter.ToInt32(request, 1);
            User = new User(request, 5, passwordOffset-5);
            Password = Encoding.UTF8.GetString(request, passwordOffset, request.Length - passwordOffset);
        }

        public byte[] ToBytes()
        {
            byte[] userBytes = User.ToBytes();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(Password);
            byte[] request = new byte[5 + userBytes.Length + passwordBytes.Length];
            request[0] = (byte)MessageType;
            Array.Copy(BitConverter.GetBytes(5 + userBytes.Length), 0, request, 1, 4);
            Array.Copy(userBytes, 0, request, 5, userBytes.Length);
            Array.Copy(passwordBytes, 0, request, 5 + userBytes.Length, passwordBytes.Length);
            return request;
        }
    }
}
