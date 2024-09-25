using BattleshipMessages;
using BattleshipMessages.Messages;
using BattleshipObjects;
using System;
using System.Windows.Forms;

namespace BattleshipClient
{
    public partial class Register : Form
    {
        readonly Client Client;
        public string[] Cities;
        public Register(Client Client, CitiesResponse response)
        {
            InitializeComponent();
            this.Client = Client;
            Client.ExceptionReceived += Client_ExceptionReceived;
            Client.Disconnected += Client_Disconnected;
            Client.MessageReceived += Client_MessageReceived;
            FormClosing += Register_FormClosing;
            Cities = response.Cities;
            CityBox.Items.AddRange(Cities);
            CityBox.SelectedIndex = 0;
            GenderBox.Items.AddRange(Enum.GetNames(typeof(Genders)));
            GenderBox.SelectedIndex = 0;
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameBox.Text) || string.IsNullOrEmpty(PasswordBox.Text) || string.IsNullOrEmpty(FirstNameBox.Text) || string.IsNullOrEmpty(LastNameBox.Text) || string.IsNullOrEmpty(EmailBox.Text))
            {
                MessageBox.Show("Please fill out all fields");
                return;
            }
            if (CityBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a city");
                return;
            }
            if (GenderBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a gender");
            }

            Client.Send(new RegisterRequest(new User(UsernameBox.Text, EmailBox.Text, FirstNameBox.Text, LastNameBox.Text, Cities[CityBox.SelectedIndex], (Genders)GenderBox.SelectedIndex), PasswordBox.Text));
        }

        private void Client_MessageReceived(object sender, IMessage e)
        {
            if (e.MessageType == MessageTypes.RegisterResponse)
            {
                RegisterResponse response = (RegisterResponse)e;
                if (response.Success)
                {
                    BeginInvoke(new Action(() => MessageBox.Show("Registration successful")));
                    BeginInvoke(new Action(Close));
                }
                else
                {
                    BeginInvoke(new Action(() => MessageBox.Show("Registration failed: " + response.ErrorMessage)));
                }
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {
            BeginInvoke(new Action(() => MessageBox.Show("Disconnected from server")));
            BeginInvoke(new Action(Close));
        }

        private void Client_ExceptionReceived(object sender, Exception e)
        {
            BeginInvoke(new Action(() => MessageBox.Show("An error occurred: " + e.Message)));
            BeginInvoke(new Action(Close));
        }

        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client.ExceptionReceived -= Client_ExceptionReceived;
            Client.Disconnected -= Client_Disconnected;
            Client.MessageReceived -= Client_MessageReceived;
        }
    }
}
