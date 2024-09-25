using BattleshipObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class GetDataResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: Message type
         * 1 to end: User
        */
        public MessageTypes MessageType => MessageTypes.GetDataResponse;
        public User User { get; set; }

        public GetDataResponse(User User)
        {
            this.User = User;
        }

        public GetDataResponse(byte[] response)
        {
            User = new User(response, 1);
        }

        public byte[] ToBytes()
        {
            byte[] userBytes = User.ToBytes();
            byte[] response = new byte[1 + userBytes.Length];
            response[0] = (byte)MessageType;
            Array.Copy(userBytes, 0, response, 1, userBytes.Length);
            return response;
        }
    }
}
