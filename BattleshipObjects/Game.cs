using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipObjects
{
    public class Game
    {
        public User Player1 { get; }
        public User Player2 { get; set; }
        public Board Board1 { get; }
        public Board Board2 { get; set; }
        public bool Player1Turn { get; set; }
        public bool IsCustom { get; }
        public int GameId { get; }

        public Game(User Player1, User Player2, Board Board1, Board Board2, bool IsCustomGame=false, int GameId=-1)
        {
            this.Player1 = Player1;
            this.Player2 = Player2;
            this.Board1 = Board1;
            this.Board2 = Board2;
            this.IsCustom = IsCustomGame;
            Random rnd = new Random();
            if (GameId == -1)
            {
                GameId = rnd.Next();
            }
            this.GameId = GameId;
            Player1Turn = rnd.Next(2) == 1;
        }

        public bool Hit((byte, byte) location)
        {
            Board board = Player1Turn ? Board2 : Board1;
            Player1Turn = !Player1Turn;
            return board.Hit(location);
        }

        public Ship GetShipSunkByLastHit()
        {
            Board board = Player1Turn ? Board1 : Board2;
            return board.GetShipSunkByLastHit();
        }

        public bool IsGameOver()
        {
            return Board1.IsGameOver() || Board2.IsGameOver();
        }
    }
}
