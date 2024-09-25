using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class OpenGamesRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
        */

        public MessageTypes MessageType => MessageTypes.OpenGamesRequest;
        
        public OpenGamesRequest()
        {

        }

        public OpenGamesRequest(byte[] request)
        {

        }

        public byte[] ToBytes()
        {
            return new byte[] { (byte)MessageType };
        }
    }
}
