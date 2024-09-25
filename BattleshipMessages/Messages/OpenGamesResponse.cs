using BattleshipObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipMessages.Messages
{
    public class OpenGamesResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1 to 4: 2nd game offset
         * 5 to (nextGameOffset-1): 1st game info
         * this pattern onwards
        */

        public MessageTypes MessageType => MessageTypes.OpenGamesResponse;
        public GameInfo[] Games { get; set; }

        public OpenGamesResponse(GameInfo[] Games)
        {
            this.Games = Games;
        }

        public OpenGamesResponse(byte[] response)
        {
            int offset = 1;
            List<GameInfo> games = new List<GameInfo>();
            while (offset < response.Length)
            {
                int nextGameOffset = BitConverter.ToInt32(response, offset);
                offset += 4;
                games.Add(new GameInfo(response, offset, nextGameOffset - offset));
                offset = nextGameOffset;
            }
            this.Games = games.ToArray();
        }

        public byte[] ToBytes()
        {
            List<byte> response = new List<byte> { (byte)MessageType };
            foreach (GameInfo game in Games)
            {
                byte[] gameBytes = game.ToBytes();
                response.AddRange(BitConverter.GetBytes(response.Count + gameBytes.Length + 4));
                response.AddRange(gameBytes);
            }
            return response.ToArray();
        }
    }
}
