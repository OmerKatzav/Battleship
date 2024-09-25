using BattleshipMessages;
using BattleshipMessages.Messages;
using BattleshipObjects;
using BattleshipUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class GameEditor : Form
    {
        readonly Client Client;
        readonly string Username;
        readonly bool IsJoiningGame;
        readonly int GameID;
        readonly Board Board;
        Dictionary<Ship, int> LeftShips;
        readonly int DefaultWidth;
        readonly int DefaultHeight;
        readonly ShipArray DefaultShips;

        public GameEditor(Client Client, GameParamsResponse response, string Username, bool IsJoiningGame, int GameID=-1)
        {
            InitializeComponent();
            this.Client = Client;
            Client.MessageReceived += Client_MessageReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            this.Username = Username;
            this.IsJoiningGame = IsJoiningGame;
            this.GameID = GameID;
            DefaultWidth = response.BoardWidth;
            DefaultHeight = response.BoardHeight;
            DefaultShips = response.Ships;
            Board = new Board(response.BoardWidth, response.BoardHeight, new Dictionary<Ship, (byte, byte)>());
            BoardXControl.Value = response.BoardWidth;
            BoardYControl.Value = response.BoardHeight;
            LeftShips = response.Ships.GetShipCounts();
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
            BoardBox.MouseMove += BoardBox_MouseMove;
            BoardBox.MouseClick += BoardBox_MouseClick;
            if (IsJoiningGame)
            {
                StartBtn.Text = "Join Game";
                CustomShipBtn.Enabled = false;
                DeleteBtn.Enabled = false;
                AddBtn.Enabled = false;
                RemoveBtn.Enabled = false;
                BoardXControl.Enabled = false;
                BoardYControl.Enabled = false;
            }
            FormClosing += GameEditor_FormClosing;
            InitializeShipListView();
        }

        private Bitmap NormalizeImage(Bitmap image, int width, int height)
        {
            Bitmap normalizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(normalizedImage))
            {
                graphics.DrawImage(image, 0, 0);
            }
            return normalizedImage;
        }

        private void InitializeShipListView()
        {
            ShipListView.Items.Clear();
            ShipListView.LargeImageList = new ImageList
            {
                ImageSize = new Size(50, 50)
            };
            foreach (Ship ship in LeftShips.Keys)
            {
                ShipListView.LargeImageList.Images.Add(NormalizeImage(ship.GetImage(50, 50), 50, 50));
                ShipListView.Items.Add(new ListViewItem(LeftShips[ship].ToString(), ShipListView.LargeImageList.Images.Count - 1));
            }
        }

        private (byte, byte) GetLocationFromMouse(Point MouseCoordinates, out bool success)
        {
            Point point = PictureBoxUtils.GetPointHoveringOn(BoardBox, BoardBox.PointToScreen(MouseCoordinates), Board.Width, Board.Height);
            success = !(point == null || point.X < 0 || point.Y < 0 || point.X >= Board.Width || point.Y >= Board.Height);
            return ((byte)point.X, (byte)point.Y);
        }

        private void DrawShip(Ship ship, (byte, byte) location, Color color=default)
        {
            if (color == default) color = Color.LightBlue;
            Bitmap boardImage = new Bitmap(BoardBox.Image);
            int scale = boardImage.Width / Board.Width;
            using (Graphics graphics = Graphics.FromImage(boardImage))
            {
                using (Brush brush = new SolidBrush(color))
                {
                    for (byte x = 0; x < Board.Width; x++)
                    {
                        for (byte y = 0; y < Board.Height; y++)
                        {
                            if (ship.IsAt((x, y), location))
                            {
                                graphics.FillRectangle(brush, x * scale, y * scale, scale, scale);
                            }

                        }
                    }
                }
            }
            BoardBox.Image = boardImage;
        }

        private void RotateShip(Ship ship)
        {
            ship.Rotate();
            InitializeShipListView();
        }

        private void AddShip(Ship ship)
        {
            LeftShips[ship]++;
            InitializeShipListView();
        }

        private void RemoveShip(Ship ship)
        {
            if (LeftShips[ship] > 0)
            {
                LeftShips[ship]--;
            }
            InitializeShipListView();
        }

        private void DeleteShip(Ship ship)
        {
            Board.RemoveShipType(ship);
            LeftShips.Remove(ship);
            InitializeShipListView();
        }

        private void BoardBox_MouseMove(object sender, MouseEventArgs e)
        {
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
            (byte, byte) location = GetLocationFromMouse(e.Location, out bool success);
            if (!success) return;
            if (ShipListView.SelectedItems.Count == 0)
            {
                Ship selectedShip = Board.ShipAt(location);
                if (selectedShip != null)
                {
                    DrawShip(selectedShip, Board.Ships[selectedShip], Color.Red);
                }
                return;
            }
            Ship ship = LeftShips.Keys.ElementAt(ShipListView.SelectedIndices[0]);
            if (ship == null) return;
            if (LeftShips[ship] == 0) return;
            if (!Board.IsValidLocation(ship, location)) return;
            DrawShip(ship, location);
        }

        private void BoardBox_MouseClick(object sender, MouseEventArgs e)
        {
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
            (byte, byte) location = GetLocationFromMouse(e.Location, out bool success);
            if (!success) return;
            if (ShipListView.SelectedItems.Count == 0)
            {
                Ship selectedShip = Board.ShipAt(location);
                if (selectedShip != null)
                {
                    Board.RemoveShip(selectedShip);
                    Ship keyShip = LeftShips.Keys.FirstOrDefault(s => s.IsEquals(selectedShip));
                    if (keyShip != null)
                    {
                        AddShip(keyShip);
                    }
                    else
                    {
                        LeftShips[selectedShip] = 1;
                        InitializeShipListView();
                    }
                }
                return;
            }
            Ship ship = LeftShips.Keys.ElementAt(ShipListView.SelectedIndices[0]);
            if (ship == null) return;
            if (LeftShips[ship] == 0) return;
            if (!Board.IsValidLocation(ship, location)) return;
            Board.AddShip(ship.Clone(), location);
            LeftShips[ship]--;
            InitializeShipListView();
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
        }

        private void BoardXControl_ValueChanged(object sender, EventArgs e)
        {
            Board.Width = (int)BoardXControl.Value;
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
        }

        private void BoardYControl_ValueChanged(object sender, EventArgs e)
        {
            Board.Height = (int)BoardYControl.Value;
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            if (!Board.IsValid() || LeftShips.Values.Sum() > 0)
            {
                MessageBox.Show("Invalid Board");
                return;
            }
            bool isDefault = IsJoiningGame || (new ShipArray(Board.Ships.Keys.ToArray()).IsEquals(DefaultShips) && Board.Width == DefaultWidth && Board.Height == DefaultHeight);
            Client.Send(new GameStartRequest(!isDefault, IsJoiningGame, new ShipArray(Board.Ships.Keys.ToArray()), Board.Ships.Values.ToArray(), GameID, Board.Width, Board.Height));
        }

        private void CustomShipBtn_Click(object sender, EventArgs e)
        {
            ShipCreator creator = new ShipCreator();
            creator.ShowDialog();
            if (creator.DialogResult == DialogResult.OK)
            {
                Ship ship = creator.Ship;
                bool found = false;
                foreach (Ship otherShip in LeftShips.Keys)
                {
                    if (ship.IsEquals(otherShip))
                    {
                        LeftShips[otherShip]++;
                        found = true;
                        break;
                    }
                }
                if (!found)
                    LeftShips[ship] = 1;
                InitializeShipListView();
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (ShipListView.SelectedItems.Count == 0) return;
            Ship ship = LeftShips.ElementAt(ShipListView.SelectedIndices[0]).Key;
            DeleteShip(ship);
        }

        private void RotateBtn_Click(object sender, EventArgs e)
        {
            if (ShipListView.SelectedItems.Count == 0) return;
            Ship ship = LeftShips.ElementAt(ShipListView.SelectedIndices[0]).Key;
            RotateShip(ship);
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (ShipListView.SelectedItems.Count == 0) return;
            Ship ship = LeftShips.ElementAt(ShipListView.SelectedIndices[0]).Key;
            AddShip(ship);
        }

        private void RemoveBtn_Click(object sender, EventArgs e)
        {
            if (ShipListView.SelectedItems.Count == 0) return;
            Ship ship = LeftShips.ElementAt(ShipListView.SelectedIndices[0]).Key;
            RemoveShip(ship);
        }

        private void DefaultBtn_Click(object sender, EventArgs e)
        {
            Board.Width = DefaultWidth;
            Board.Height = DefaultHeight;
            Board.Ships = new Dictionary<Ship, (byte, byte)>();
            LeftShips = DefaultShips.GetShipCounts();
            BoardXControl.Value = DefaultWidth;
            BoardYControl.Value = DefaultHeight;
            BoardBox.Image = Board.GetImage(BoardBox.Width, BoardBox.Height);
            InitializeShipListView();
        }

        private void StartGame(GameStartResponse response)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected += AlternateClient_Disconnected;
            Client.ExceptionReceived += AlternateClient_ExceptionReceived;
            Game game = new Game(Client, response, Board, Username);
            Hide();
            game.ShowDialog();
            Close();
        }

        private void LockControls()
        {
            BoardXControl.Enabled = false;
            BoardYControl.Enabled = false;
            StartBtn.Enabled = false;
            CustomShipBtn.Enabled = false;
            DeleteBtn.Enabled = false;
            RotateBtn.Enabled = false;
            AddBtn.Enabled = false;
            RemoveBtn.Enabled = false;
            DefaultBtn.Enabled = false;
            ShipListView.Enabled = false;
            BoardBox.MouseMove -= BoardBox_MouseMove;
            BoardBox.MouseClick -= BoardBox_MouseClick;
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

        private void AlternateClient_ExceptionReceived(object sender, Exception e)
        {
            BeginInvoke(new Action(Close));
        }

        private void AlternateClient_Disconnected(object sender, EventArgs e)
        {
            BeginInvoke(new Action(Close));
        }

        private void Client_MessageReceived(object sender, IMessage e)
        {
            if (e.MessageType == MessageTypes.GameStartResponse)
            {
                GameStartResponse response = (GameStartResponse)e;
                BeginInvoke(new Action(() => StartGame(response)));
            }
            if (e.MessageType == MessageTypes.WaitingForOpponentMessage)
            {
                WaitingForOpponentMessage message = (WaitingForOpponentMessage)e;
                BeginInvoke(new Action(() => MessageBox.Show("Waiting for opponent, Game ID: " + message.GameID)));
                BeginInvoke(new Action(LockControls));
            }
        }

        private void GameEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected -= AlternateClient_Disconnected;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
        }
    }
}
