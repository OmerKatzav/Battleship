using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class RegisterResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1: success
         * 2 to end: error message (if failed)
        */

        public MessageTypes MessageType => MessageTypes.RegisterResponse;
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public RegisterResponse(bool Success, string ErrorMessage=null) 
        { 
            this.Success = Success;
            this.ErrorMessage = ErrorMessage;
        }

        public RegisterResponse(byte[] response)
        {
            Success = response[1] == 1;
            if (!Success) ErrorMessage = Encoding.UTF8.GetString(response, 2, response.Length - 2);
            else ErrorMessage = null;
        }

        public byte[] ToBytes()
        {
            if (Success) return new byte[] { (byte)MessageType, 1 };
            byte[] errorMessageBytes = Encoding.UTF8.GetBytes(ErrorMessage);
            byte[] response = new byte[2 + errorMessageBytes.Length];
            response[0] = (byte)MessageType;
            response[1] = 0;
            Array.Copy(errorMessageBytes, 0, response, 2, errorMessageBytes.Length);
            return response;
        }
    }
}
