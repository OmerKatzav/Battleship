using BattleshipMessages;
using BattleshipMessages.Messages;
using System;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class ServerConnect : Form
    {
        Client Client;
        bool Connected = false;
        bool IsClosing = false;

        public ServerConnect()
        {
            InitializeComponent();
            FormClosing += ServerConnect_FormClosing;
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            string address = AddressBox.Text.Trim();
            string port = PortBox.Text.Trim();
            if (string.IsNullOrEmpty(address))
            {
                address = "localhost";
            }
            if (string.IsNullOrEmpty(port))
            {
                port = "1234";
            }
            if (!int.TryParse(port, out int portNumber))
            {
                MessageBox.Show("Invalid Port Number");
                return;
            }
            Client = new Client(address, portNumber);
            Client.ExceptionReceived += Client_ExceptionReceived;
            Client.Disconnected += Client_Disconnected;
            Client.MessageReceived += Client_MessageReceived;
            Connected = true;
            Client.Start();
            if (Connected)
                Client.Send(new PingRequest());
        }

        private void StartLogin()
        {
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.MessageReceived -= Client_MessageReceived;
            Hide();
            Login login = new Login(Client);
            login.ShowDialog();
            Show();
        }

        private void Client_MessageReceived(object sender, IMessage e)
        {
            if (e.MessageType == MessageTypes.PingResponse)
            {
                BeginInvoke(new Action(StartLogin));
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            if (IsClosing) return;
            BeginInvoke(new Action(() => MessageBox.Show("Disconnected From Server")));
            Connected = false;
        }

        private void Client_ExceptionReceived(object sender, Exception e)
        {
            if (IsClosing) return;
            BeginInvoke(new Action(() => MessageBox.Show("An error occurred: " + e.Message)));
            Connected = false;
        }

        private void ServerConnect_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsClosing = true;
            Client?.Stop();
        }
    }
}
