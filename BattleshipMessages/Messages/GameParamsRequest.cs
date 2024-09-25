using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages
{
    public class GameParamsRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to 4: game id (optional)
        */
        public MessageTypes MessageType => MessageTypes.GameParamsRequest;
        public int GameId { get; set; }

        public GameParamsRequest(int GameId=-1)
        {
            this.GameId = GameId;
        }

        public GameParamsRequest(byte[] request)
        {
            if (request.Length > 1)
            {
                GameId = BitConverter.ToInt32(request, 1);
            }
            else
            {
                GameId = -1;
            }
        }

        public byte[] ToBytes()
        {
            if (GameId == -1) return new byte[1] { (byte)MessageType };
            byte[] gameIdBytes = BitConverter.GetBytes(GameId);
            byte[] response = new byte[1 + gameIdBytes.Length];
            response[0] = (byte)MessageType;
            Array.Copy(gameIdBytes, 0, response, 1, gameIdBytes.Length);
            return response;
        }
    }
}
