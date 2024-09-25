using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipObjects;

namespace BattleshipMessages.Messages
{
    public class GameParamsResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1: board width - 1
         * 2: board height - 1
         * 3 to end: ships
        */
        public MessageTypes MessageType => MessageTypes.GameParamsResponse;
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public ShipArray Ships { get; set; }

        public GameParamsResponse(int BoardWidth, int BoardHeight, ShipArray Ships)
        {
            this.BoardWidth = BoardWidth;
            this.BoardHeight = BoardHeight;
            this.Ships = Ships;
        }

        public GameParamsResponse(byte[] response)
        {
            BoardWidth = response[1] + 1;
            BoardHeight = response[2] + 1;
            Ships = new ShipArray(response, 3);
        }

        public byte[] ToBytes()
        {
            byte[] shipBytes = Ships.ToBytes();
            byte[] response = new byte[3 + shipBytes.Length];
            response[0] = (byte)MessageType;
            response[1] = (byte)(BoardWidth - 1);
            response[2] = (byte)(BoardHeight - 1);
            Array.Copy(shipBytes, 0, response, 3, shipBytes.Length);
            return response;
        }
    }
}
