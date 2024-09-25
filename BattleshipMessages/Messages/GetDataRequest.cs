using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class GetDataRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: Message type
        */

        public MessageTypes MessageType => MessageTypes.GetDataRequest;

        public GetDataRequest()
        {

        }

        public GetDataRequest(byte[] request)
        {
            
        }

        public byte[] ToBytes()
        {
            return new byte[] { (byte)MessageType };
        }
    }
}
