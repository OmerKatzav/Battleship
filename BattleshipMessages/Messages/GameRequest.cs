using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public class GameRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1: x
         * 2: y
        */

        public MessageTypes MessageType => MessageTypes.GameRequest;
        public (byte, byte) Location { get; set; }

        public GameRequest((byte, byte) Location)
        {
            this.Location = Location;
        }

        public GameRequest(byte[] request)
        {
            byte x = request[1];
            byte y = request[2];
            Location = (x, y);
        }

        public byte[] ToBytes()
        {
            byte[] request = new byte[3];
            request[0] = (byte)MessageType;
            request[1] = Location.Item1;
            request[2] = Location.Item2;
            return request;
        }
    }
}
