using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public class ChatResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to end: message
        */

        public MessageTypes MessageType => MessageTypes.ChatResponse;
        public string Message { get; set; }

        public ChatResponse(string Message) 
        {
            this.Message = Message;
        }

        public ChatResponse(byte[] response)
        {
            Message = Encoding.UTF8.GetString(response, 1, response.Length - 1);
        }

        public byte[] ToBytes()
        {
            byte[] response = new byte[1 + Message.Length];
            response[0] = (byte)MessageType;
            Array.Copy(Encoding.UTF8.GetBytes(Message), 0, response, 1, Message.Length);
            return response;
        }
    }
}
