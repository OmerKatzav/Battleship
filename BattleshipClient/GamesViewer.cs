using BattleshipMessages;
using BattleshipMessages.Messages;
using BattleshipObjects;
using System;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class GamesViewer : Form
    {
        readonly Client Client;
        GameInfo[] Games;
        readonly string Username;
        int GameID;
        bool IsClosing = false;

        public GamesViewer(Client Client, OpenGamesResponse response, string username)
        {
            InitializeComponent();
            this.Client = Client;
            Games = response.Games;
            InitializeGamesBox();
            Client.ExceptionReceived += Client_ExceptionReceived;
            Client.Disconnected += Client_Disconnected;
            Client.MessageReceived += Client_MessageReceived;
            Username = username;
            FormClosing += GamesViewer_FormClosing;
        }

        private void InitializeGamesBox()
        {
            GamesBox.Items.Clear();
            foreach (GameInfo game in Games)
            {
                GamesBox.Items.Add(game.ToString());
            }
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            Client.Send(new OpenGamesRequest());
        }

        private void JoinBtn_Click(object sender, EventArgs e)
        {
            if (GamesBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a game to join");
                return;
            }
            GameID = Games[GamesBox.SelectedIndex].GameId;
            Client.Send(new GameParamsRequest(GameID));
        }

        private void StartGameEditor(GameParamsResponse response)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected += AlternateClient_Disconnected;
            Client.ExceptionReceived += AlternateClient_ExceptionReceived;
            Hide();
            GameEditor gameEditor = new GameEditor(Client, response, Username, true, GameID);
            gameEditor.ShowDialog();
            Show();
            if (!IsClosing)
            {
                Client.Disconnected -= AlternateClient_Disconnected;
                Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
                Client.Disconnected += Client_Disconnected;
                Client.ExceptionReceived += Client_ExceptionReceived;
                Client.MessageReceived += Client_MessageReceived;
            }
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
            if (e.MessageType == MessageTypes.OpenGamesResponse)
            {
                OpenGamesResponse response = (OpenGamesResponse)e;
                Games = response.Games;
                BeginInvoke(new MethodInvoker(InitializeGamesBox));
            }
            else if (e.MessageType == MessageTypes.GameParamsResponse)
            {
                GameParamsResponse response = (GameParamsResponse)e;
                BeginInvoke(new MethodInvoker(() => StartGameEditor(response)));
            }
        }

        private void GamesViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsClosing = true;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= AlternateClient_Disconnected;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
        }
    }
}
