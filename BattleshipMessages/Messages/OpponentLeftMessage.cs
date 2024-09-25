using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class OpponentLeftMessage : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
        */
        public MessageTypes MessageType => MessageTypes.OpponentLeftMessage;

        public OpponentLeftMessage()
        {
        }

        public OpponentLeftMessage(byte[] response)
        {
        }

        public byte[] ToBytes()
        {
            return new byte[1] { (byte)MessageType };
        }
    }
}
