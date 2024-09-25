using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleshipObjects;
using BattleshipUtils;

namespace BattleshipMessages.Messages
{
    public class GameStartRequest : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1: parameters
         * 2 to 5: locations offset
         * 
         * Parameters:
         * 0: is custom game
         * 1: is joining game
         * 
         * Custom Game:
         * 6: board width - 1
         * 7: board height - 1
         * 8 to (locationsOffset-1): ships
         * locationsOffset to end: ship locations
         * 
         * 
         * Normal Game:
         * 6 to (locationsOffset-1): ships
         * locationsOffset to end: ship locations
         * 
         * Joining Game:
         * 6 to 9: game ID
         * 10 to (locationsOffset-1): ships
         * locationsOffset to end: ship locations
        */

        public MessageTypes MessageType => MessageTypes.GameStartRequest;
        public bool IsCustomGame { get; set; }
        public bool IsJoiningGame { get; set; }
        public (byte, byte)[] ShipLocations { get; set; }
        public int GameID { get; set; }
        public int BoardWidth { get; set; }
        public int BoardHeight { get; set; }
        public ShipArray Ships { get; set; }
        

        public GameStartRequest(bool IsCustomGame, bool IsJoiningGame, ShipArray Ships, (byte, byte)[] ShipLocations, int GameID=-1, int BoardWidth=-1, int BoardHeight=-1)
        {
            this.IsCustomGame = IsCustomGame;
            this.IsJoiningGame = IsJoiningGame;
            this.Ships = Ships;
            this.ShipLocations = ShipLocations;
            this.GameID = GameID;
            this.BoardWidth = BoardWidth;
            this.BoardHeight = BoardHeight;
        }

        public GameStartRequest(byte[] request)
        {
            IsCustomGame = BitUtils.GetBit(request[1], 0);
            IsJoiningGame = BitUtils.GetBit(request[1], 1);
            int locationsOffset = BitConverter.ToInt32(request, 2);
            int shipOffset = IsCustomGame ? 8 : IsJoiningGame ? 10 : 6;
            Ships = new ShipArray(request, shipOffset, locationsOffset - shipOffset);
            if (IsCustomGame)
            {
                BoardWidth = request[6] + 1;
                BoardHeight = request[7] + 1;
            }
            else if (IsJoiningGame)
            {
                GameID = BitConverter.ToInt32(request, 6);
            }
            ShipLocations = new (byte, byte)[(request.Length - locationsOffset) / 2];
            for (int i = 0; i < ShipLocations.Length; i++)
            {
                ShipLocations[i] = (request[locationsOffset + i * 2], request[locationsOffset + i * 2 + 1]);
            }
        }

        public byte[] ToBytes()
        {
            int shipOffset = IsCustomGame ? 8 : IsJoiningGame ? 10 : 6;
            byte[] shipBytes = Ships.ToBytes();
            int locationsOffset = shipOffset + shipBytes.Length;
            byte[] request = new byte[locationsOffset + ShipLocations.Length * 2];
            request[0] = (byte)MessageType;
            request[1] = BitUtils.SetBit(request[1], 0, IsCustomGame);
            request[1] = BitUtils.SetBit(request[1], 1, IsJoiningGame);
            Array.Copy(BitConverter.GetBytes(locationsOffset), 0, request, 2, 4);
            Array.Copy(shipBytes, 0, request, shipOffset, shipBytes.Length);
            if (IsCustomGame)
            {
                request[6] = (byte)(BoardWidth - 1);
                request[7] = (byte)(BoardHeight - 1);
            }
            else if (IsJoiningGame)
            {
                Array.Copy(BitConverter.GetBytes(GameID), 0, request, 6, 4);
            }
            for (int i = 0; i < ShipLocations.Length; i++)
            {
                request[locationsOffset + i * 2] = ShipLocations[i].Item1;
                request[locationsOffset + i * 2 + 1] = ShipLocations[i].Item2;
            }
            return request;
        }
    }
}
