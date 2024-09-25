using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipObjects
{
    public class GameInfo
    {
        /*
         * Byte Structure:
         * 0-3: game id
         * 4: is custom
         * 5-end: player 1 username
        */
        public int GameId { get; set; }
        public bool IsCustom { get; set; }
        public string Player1Username { get; set; }

        public GameInfo(int gameId, bool isCustom, string player1Username)
        {
            GameId = gameId;
            IsCustom = isCustom;
            Player1Username = player1Username;
        }

        public GameInfo(byte[] bytes, int offset=0, int length=0)
        {
            if (length == 0)
                length = bytes.Length;
            GameId = BitConverter.ToInt32(bytes, offset);
            IsCustom = bytes[offset + 4] == 1;
            Player1Username = Encoding.UTF8.GetString(bytes, offset + 5, length - 5);
        }

        public GameInfo(Game game)
        {
            GameId = game.GameId;
            IsCustom = game.IsCustom;
            Player1Username = game.Player1.Username;
        }

        public byte[] ToBytes()
        {
            byte[] usernameBytes = Encoding.UTF8.GetBytes(Player1Username);
            byte[] bytes = new byte[5 + usernameBytes.Length];
            Array.Copy(BitConverter.GetBytes(GameId), 0, bytes, 0, 4);
            bytes[4] = Convert.ToByte(IsCustom);
            Array.Copy(usernameBytes, 0, bytes, 5, usernameBytes.Length);
            return bytes;
        }

        public override string ToString()
        {
            string isCustomString = IsCustom ? "Custom Game" : "Default Game";
            return $"Game Id: {GameId}, {isCustomString}, User Inside: {Player1Username}";
        }
    }
}
