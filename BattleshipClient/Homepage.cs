using BattleshipMessages;
using BattleshipMessages.Messages;
using System;
using System.Text;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class Homepage : Form
    {
        readonly Client Client;
        readonly string Username;

        public Homepage(Client Client, string Username)
        {
            InitializeComponent();
            this.Client = Client;
            this.Username = Username;
            WelcomeLabel.Text = "Welcome, " + Username;
            Client.MessageReceived += Client_MessageReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            FormClosing += Homepage_FormClosing;
        }

        private void Homepage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected -= AlternateClient_Disconnected;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            Client.Send(new GameParamsRequest());
        }

        private void OpenGamesBtn_Click(object sender, EventArgs e)
        {
            Client.Send(new OpenGamesRequest());
        }

        private void ViewDataBtn_Click(object sender, EventArgs e)
        {
            Client.Send(new GetDataRequest());
        }

        private void ViewData(GetDataResponse response)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Username: " + response.User.Username);
            builder.AppendLine("First Name: " + response.User.FirstName);
            builder.AppendLine("Last Name: " + response.User.LastName);
            builder.AppendLine("Email: " + response.User.Email);
            builder.AppendLine("City: " + response.User.City);
            builder.AppendLine("Gender: " + response.User.Gender.ToString());
            MessageBox.Show(builder.ToString());
        }

        private void StartGameEditor(GameParamsResponse response)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected += AlternateClient_Disconnected;
            Client.ExceptionReceived += AlternateClient_ExceptionReceived;
            GameEditor editor = new GameEditor(Client, response, Username, false);
            Hide();
            editor.ShowDialog();
            Show();
            Client.Disconnected -= AlternateClient_Disconnected;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            Client.MessageReceived += Client_MessageReceived;
        }

        private void StartGamesViewer(OpenGamesResponse response)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected += AlternateClient_Disconnected;
            Client.ExceptionReceived += AlternateClient_ExceptionReceived;
            GamesViewer viewer = new GamesViewer(Client, response, Username);
            Hide();
            viewer.ShowDialog();
            Show();
            Client.Disconnected -= AlternateClient_Disconnected;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            Client.MessageReceived += Client_MessageReceived;
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
            if (e.MessageType == MessageTypes.GameParamsResponse)
            {
                GameParamsResponse response = (GameParamsResponse)e;
                BeginInvoke(new Action(() => StartGameEditor(response)));
            }
            else if (e.MessageType == MessageTypes.OpenGamesResponse)
            {
                OpenGamesResponse response = (OpenGamesResponse)e;
                BeginInvoke(new Action(() => StartGamesViewer(response)));
            }
            else if (e.MessageType == MessageTypes.GetDataResponse)
            {
                GetDataResponse response = (GetDataResponse)e;
                BeginInvoke(new Action(() => ViewData(response)));
            }
        }
    }
}
