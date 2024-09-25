using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BattleshipObjects;
using BattleshipUtils;

namespace BattleshipMessages
{
    public class GameResponse : IMessage
    {
        /*
         * Byte Structure:
         * 0: message type
         * 1: parameters
         * 2: x
         * 3: y
         * 4: sunk ship x (if sunk by player)
         * 5: sunk ship y (if sunk by player)
         * 6 to end: sunk ship (if sunk by player)
         * 
         * Parameters:
         * 0: is this hit by the player
         * 1: did it hit
         * 2: did it sink a ship
         * 3: is the game over
        */

        public MessageTypes MessageType => MessageTypes.GameResponse;
        public bool IsPlayer { get; set; }
        public bool Hit { get; set; }
        public bool Sink { get; set; }
        public bool GameOver { get; set; }
        public (byte, byte) Location { get; set; }
        public byte SunkShipX { get; set; }
        public byte SunkShipY { get; set; }
        public Ship SunkShip { get; set; }

        public GameResponse(bool IsPlayer, bool Hit, bool Sink, bool GameOver, (byte, byte) Location, byte SunkShipX=0, byte SunkShipY=0, Ship SunkShip=null)
        {
            this.IsPlayer = IsPlayer;
            this.Hit = Hit;
            this.Sink = Sink;
            this.GameOver = GameOver;
            this.Location = Location;
            this.SunkShipX = SunkShipX;
            this.SunkShipY = SunkShipY;
            this.SunkShip = SunkShip;
        }

        public GameResponse(byte[] response)
        {
            byte parameters = response[1];
            IsPlayer = BitUtils.GetBit(parameters, 0);
            Hit = BitUtils.GetBit(parameters, 1);
            Sink = BitUtils.GetBit(parameters, 2);
            GameOver = BitUtils.GetBit(parameters, 3);
            byte x = response[2];
            byte y = response[3];
            Location = (x, y);
            if (Sink && IsPlayer)
            {
                SunkShipX = response[4];
                SunkShipY = response[5];
                SunkShip = new Ship(response, 6);
            }
        }

        public byte[] ToBytes()
        {
            byte[] response = new byte[4];
            if (Sink && IsPlayer)
            {
                byte[] shipBytes = SunkShip.ToBytes();
                response = new byte[6 + shipBytes.Length];
                response[4] = SunkShipX;
                response[5] = SunkShipY;
                Array.Copy(shipBytes, 0, response, 6, shipBytes.Length);
            }
            response[0] = (byte)MessageType;
            byte parameters = 0;
            parameters = BitUtils.SetBit(parameters, 0, IsPlayer);
            parameters = BitUtils.SetBit(parameters, 1, Hit);
            parameters = BitUtils.SetBit(parameters, 2, Sink);
            parameters = BitUtils.SetBit(parameters, 3, GameOver);
            response[1] = parameters;
            response[2] = Location.Item1;
            response[3] = Location.Item2;
            return response;
        }
    }

}
