using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class WaitingForOpponentMessage : IMessage
    {
        /*
         * Byte Structure:
         * 0: Message ID
         * 1 to 4: Game ID
        */

        public MessageTypes MessageType => MessageTypes.WaitingForOpponentMessage;
        public int GameID { get; set; }

        public WaitingForOpponentMessage(int gameID)
        {
            GameID = gameID;
        }

        public WaitingForOpponentMessage(byte[] bytes)
        {
            GameID = BitConverter.ToInt32(bytes, 1);
        }

        public byte[] ToBytes()
        {
            byte[] message = new byte[5];
            message[0] = (byte)MessageType;
            BitConverter.GetBytes(GameID).CopyTo(message, 1);
            return message;
        }
    }
}
