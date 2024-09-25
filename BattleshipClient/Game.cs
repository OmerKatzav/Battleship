using BattleshipMessages;
using BattleshipMessages.Messages;
using BattleshipObjects;
using BattleshipUtils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class Game : Form
    {
        readonly Client Client;
        readonly string Username;
        readonly string OpponentUsername;
        readonly Board Board;
        readonly OpponentBoard OpponentBoard;
        bool IsPlayer;

        public Game(Client Client, GameStartResponse response, Board Board, string Username)
        {
            InitializeComponent();
            this.Client = Client;
            this.Board = Board;
            this.Username = Username;
            OpponentUsername = response.OpponentUsername;
            UsernameLabel.Text = "You: " + Username;
            OpponentUsernameLabel.Text = "Opponent: " + OpponentUsername;
            IsPlayer = response.IsYourTurn;
            UpdateTurnLabel();
            OpponentBoard = new OpponentBoard(Board.Width, Board.Height);
            Client.MessageReceived += Client_MessageReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
            OpponentBoardBox.Image = OpponentBoard.GetImage(OpponentBoardBox.Width, OpponentBoardBox.Height);
            OpponentBoardBox.MouseMove += OpponentBoardBox_MouseMove;
            OpponentBoardBox.Click += OpponentBoardBox_Click;
            FormClosing += Game_FormClosing;
        }

        private void UpdateTurnLabel()
        {
            if (IsPlayer)
            {
                PlayerTurnLabel.Text = "Your Turn";
            }
            else
            {
                PlayerTurnLabel.Text = "Opponent's Turn";
            }
        }

        private void SendBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ChatBox.Text))
            {
                return;
            }
            Client.Send(new ChatRequest(ChatBox.Text));
            ChatDisplayBox.Items.Add(Username + ": " + ChatBox.Text);
            ChatBox.Text = "";
        }

        private void OpponentBoardBox_MouseMove(object sender, MouseEventArgs e)
        {
            OpponentBoardBox.Image = OpponentBoard.GetImage(OpponentBoardBox.Width, OpponentBoardBox.Height);
            if (!IsPlayer)
            {
                return;
            }
            Point point = PictureBoxUtils.GetPointHoveringOn(OpponentBoardBox, OpponentBoardBox.PointToScreen(e.Location), OpponentBoard.Width, OpponentBoard.Height);
            if (point == null || point.X < 0 || point.X >= OpponentBoard.Width || point.Y < 0 || point.Y >= OpponentBoard.Height)
            {
                return;
            }
            if (OpponentBoard.Shots.ContainsKey(((byte)point.X, (byte)point.Y)) || OpponentBoard.IsOutlineAt((byte)point.X, (byte)point.Y))
            {
                return;
            }
            using (Graphics graphics = Graphics.FromImage(OpponentBoardBox.Image))
            {
                using (Brush brush = new SolidBrush(Color.LightBlue))
                {
                    graphics.FillRectangle(brush, point.X * OpponentBoardBox.Width / OpponentBoard.Width, point.Y * OpponentBoardBox.Height / OpponentBoard.Height, OpponentBoardBox.Width / OpponentBoard.Width, OpponentBoardBox.Height / OpponentBoard.Height);
                }
            }
        }

        private void OpponentBoardBox_Click(object sender, EventArgs e)
        {
            OpponentBoardBox.Image = OpponentBoard.GetImage(OpponentBoardBox.Width, OpponentBoardBox.Height);
            if (!IsPlayer)
            {
                return;
            }
            Point point = PictureBoxUtils.GetPointHoveringOn(OpponentBoardBox, OpponentBoardBox.PointToScreen(((MouseEventArgs)e).Location), OpponentBoard.Width, OpponentBoard.Height);
            if (point == null || point.X < 0 || point.X >= OpponentBoard.Width || point.Y < 0 || point.Y >= OpponentBoard.Height)
            {
                return;
            }
            if (OpponentBoard.Shots.ContainsKey(((byte)point.X, (byte)point.Y)) || OpponentBoard.IsOutlineAt((byte)point.X, (byte)point.Y))
            {
                return;
            }
            if (point.X < 0 || point.Y < 0 || point.X >= OpponentBoard.Width || point.Y >= OpponentBoard.Height)
            {
                return;
            }
            Client.Send(new GameRequest(((byte)point.X, (byte)point.Y)));
            IsPlayer = false;
            UpdateTurnLabel();
        }

        private void Client_ExceptionReceived(object sender, Exception e)
        {
            BeginInvoke(new Action(() => MessageBox.Show("An error occurred: " + e.Message)));
            BeginInvoke(new Action(Close));
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() => MessageBox.Show("Disconnected from server")));
            BeginInvoke(new Action(Close));
        }

        private void Client_MessageReceived(object sender, IMessage e)
        {
            if (e.MessageType == MessageTypes.GameResponse)
            {
                GameResponse response = (GameResponse)e;
                BeginInvoke(new Action(() => HandleGameResponse(response)));
            }
            else if (e.MessageType == MessageTypes.ChatResponse)
            {
                ChatResponse response = (ChatResponse)e;
                BeginInvoke(new Action(() => HandleChatResponse(response)));
            }
            else if (e.MessageType == MessageTypes.OpponentLeftMessage)
            {
                BeginInvoke(new Action(HandleOpponentLeftMessage));
            }
        }

        private void HandleGameResponse(GameResponse response)
        {
            if (response.IsPlayer)
            {
                OpponentBoard.Shots.Add(response.Location, response.Hit);
                if (response.Sink)
                {
                    OpponentBoard.KnownShips.Add(response.SunkShip, (response.SunkShipX, response.SunkShipY));
                }
                OpponentBoardBox.Image = OpponentBoard.GetImage(OpponentBoardBox.Width, OpponentBoardBox.Height);
            }
            else
            {
                Board.Shots.Add(response.Location);
                BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
                IsPlayer = true;
                UpdateTurnLabel();
            }
            if (response.GameOver)
            {
                if (response.IsPlayer)
                {
                    MessageBox.Show("You win!");
                }
                else
                {
                    MessageBox.Show("You lose!");
                }
                Close();
            }
        }

        private void HandleChatResponse(ChatResponse response)
        {
            ChatDisplayBox.Items.Add(OpponentUsername + ": " + response.Message);
        }

        private void HandleOpponentLeftMessage()
        {
            MessageBox.Show("Opponent left the game");
            Close();
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
        }
    }
}
