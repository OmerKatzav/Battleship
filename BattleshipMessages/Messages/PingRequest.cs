using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class PingRequest : IMessage
    {
        /*
         * Byte Stucture:
         * 0: Message Type
        */
        public MessageTypes MessageType => MessageTypes.PingRequest;

        public PingRequest()
        {

        }

        public PingRequest(byte[] request)
        {

        }

        public byte[] ToBytes()
        {
            return new byte[] { (byte)MessageType };
        }
    }
}
