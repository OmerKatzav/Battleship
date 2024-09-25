using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class PingResponse : IMessage
    {
        /*
         * Byte Stucture:
         * 0: Message Type
        */
        public MessageTypes MessageType => MessageTypes.PingResponse;

        public PingResponse()
        {

        }

        public PingResponse(byte[] response)
        {

        }

        public byte[] ToBytes()
        {
            return new byte[] { (byte)MessageType };
        }
    }
}
