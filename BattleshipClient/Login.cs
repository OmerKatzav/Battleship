using BattleshipMessages;
using BattleshipMessages.Messages;
using System;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class Login : Form
    {
        readonly Client Client;

        public Login(Client Client)
        {
            InitializeComponent();
            this.Client = Client;
            Client.MessageReceived += Client_MessageReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            FormClosing += Login_FormClosing;
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameBox.Text) || string.IsNullOrEmpty(PasswordBox.Text))
            {
                MessageBox.Show("Please enter a username and password");
                return;
            }
            Client.Send(new LoginRequest(UsernameBox.Text, PasswordBox.Text));
        }

        private void RegisterLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Client.Send(new CitiesRequest());
        }

        private void StartHome()
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected += AlternateClient_Disconnected;
            Client.ExceptionReceived += AlternateClient_ExceptionReceived;
            Homepage home = new Homepage(Client, UsernameBox.Text);
            Hide();
            home.ShowDialog();
            Show();
        }

        private void StartRegister(CitiesResponse response)
        {
            Client.MessageReceived -= Client_MessageReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected += AlternateClient_Disconnected;
            Client.ExceptionReceived += AlternateClient_ExceptionReceived;
            Register register = new Register(Client, response);
            Hide();
            register.ShowDialog();
            Show();
            Client.MessageReceived += Client_MessageReceived;
            Client.Disconnected += Client_Disconnected;
            Client.ExceptionReceived += Client_ExceptionReceived;
            Client.Disconnected -= AlternateClient_Disconnected;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
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
            if (e.MessageType == MessageTypes.LoginResponse)
            {
                LoginResponse response = (LoginResponse)e;
                if (response.Success)
                {
                    BeginInvoke(new Action(StartHome));
                }
                else
                {
                    BeginInvoke(new Action(() => MessageBox.Show("Login failed: " + response.ErrorMessage)));
                }
            }
            else if (e.MessageType == MessageTypes.CitiesResponse)
            {
                BeginInvoke(new Action(() => StartRegister((CitiesResponse)e)));
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.Disconnected -= Client_Disconnected;
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.MessageReceived -= Client_MessageReceived;
            Client.ExceptionReceived -= AlternateClient_ExceptionReceived;
            Client.Disconnected -= AlternateClient_Disconnected;
        }
    }
}
