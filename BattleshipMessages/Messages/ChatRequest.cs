using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public class ChatRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to end: message
        */

        public MessageTypes MessageType => MessageTypes.ChatRequest;
        public string Message { get; set; }

        public ChatRequest(string Message)
        {
            this.Message = Message;
        }

        public ChatRequest(byte[] request)
        {
            Message = Encoding.UTF8.GetString(request, 1, request.Length - 1);
        }

        public byte[] ToBytes()
        {
            byte[] request = new byte[1 + Message.Length];
            request[0] = (byte)MessageType;
            Array.Copy(Encoding.UTF8.GetBytes(Message), 0, request, 1, Message.Length);
            return request;
        }
    }
}
