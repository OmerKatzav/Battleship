using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class GameStartResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1: Is it your turn
         * 2 to end: opponent nickname
        */

        public MessageTypes MessageType => MessageTypes.GameStartResponse;
        public bool IsYourTurn { get; set; }
        public string OpponentUsername { get; set; }

        public GameStartResponse(bool IsYourTurn,string OpponentNickname)
        {
            this.IsYourTurn = IsYourTurn;
            this.OpponentUsername = OpponentNickname;
        }

        public GameStartResponse(byte[] response)
        {
            IsYourTurn = response[1] == 1;
            OpponentUsername = Encoding.UTF8.GetString(response, 2, response.Length - 2);
        }

        public byte[] ToBytes()
        {
            byte[] nicknameBytes = Encoding.UTF8.GetBytes(OpponentUsername);
            byte[] response = new byte[2 + nicknameBytes.Length];
            response[0] = (byte)MessageType;
            response[1] = (byte)(IsYourTurn ? 1 : 0);
            Array.Copy(nicknameBytes, 0, response, 2, nicknameBytes.Length);
            return response;
        }
    }
}
