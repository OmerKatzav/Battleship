using BattleshipMessages;
using System;
using System.Net.Sockets;
using System.Windows.Forms;

namespace BattleshipClient
{
    public class Client
    {
        public event EventHandler<IMessage> MessageReceived;
        public event EventHandler Disconnected;
        public event EventHandler<Exception> ExceptionReceived;
        TcpClient TcpClient;
        readonly string Hostname;
        readonly int Port;

        public Client(string Hostname = "localhost", int Port = 1234)
        {
            this.Hostname = Hostname;
            this.Port = Port;
        }

        public void Start()
        {
            try
            {
                TcpClient = new TcpClient(Hostname, Port);
                NetworkStream stream = TcpClient.GetStream();
                byte[] buffer = new byte[4];
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), (stream, buffer));
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
            }
        }

        public void Stop()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
            TcpClient?.Close();
        }

        public void OnRead(IAsyncResult ar)
        {
            try
            {
                (NetworkStream stream, byte[] buffer) = ((NetworkStream, byte[]))ar.AsyncState;
                int bytesRead = stream.EndRead(ar);
                if (bytesRead == 0)
                {
                    Stop();
                    Disconnected?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    int length = BitConverter.ToInt32(buffer, 0);
                    if (length > 1024)
                    {
                        throw new Exception("Message from server too long");
                    }
                    byte[] messageBuffer = new byte[length];
                    stream.Read(messageBuffer, 0, messageBuffer.Length);
                    IMessage message = MessageFactory.FromBytes(messageBuffer);
                    MessageReceived?.Invoke(this, message);
                    stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), (stream, buffer));
                }
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, e);
            }
        }

        public void Send(IMessage message)
        {
            byte[] buffer = message.ToBytes();
            TcpClient.GetStream().Write(BitConverter.GetBytes(buffer.Length), 0, 4);
            TcpClient.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
