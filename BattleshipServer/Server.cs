using BattleshipMessages;
using System;
using System.Net;
using System.Net.Sockets;

namespace BattleshipServer
{
    public class Server
    {
        public event EventHandler<(IMessage, TcpClient)> MessageReceived;
        public event EventHandler<TcpClient> Disconnected;
        public event EventHandler<(Exception, TcpClient)> ExceptionReceived;
        TcpListener TcpListener;
        readonly int Port;

        public Server(int Port = 1234)
        {
            this.Port = Port;
        }

        public void Start()
        {
            try
            {
                TcpListener = new TcpListener(IPAddress.Any, Port);
                TcpListener.Start();
                TcpListener.BeginAcceptTcpClient(OnAccept, null);
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, (e, null));
            }
        }

        public void Stop()
        {
            TcpListener?.Stop();
        }

        public void OnAccept(IAsyncResult ar)
        {
            TcpClient client = TcpListener.EndAcceptTcpClient(ar);
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[4];
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), (stream, buffer, client));
                TcpListener.BeginAcceptTcpClient(OnAccept, null);
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, (e, client));
            }
        }

        public void OnRead(IAsyncResult ar)
        {
            (NetworkStream stream, byte[] buffer, TcpClient client) = ((NetworkStream, byte[], TcpClient))ar.AsyncState;
            try
            {
                int bytesRead = stream.EndRead(ar);
                if (bytesRead == 0)
                {
                    Disconnected?.Invoke(this, client);
                    client.Close();
                }
                else
                {
                    int length = BitConverter.ToInt32(buffer, 0);
                    if (length > 1024)
                    {
                        throw new Exception("Message from client too long");
                    }
                    byte[] message = new byte[length];
                    stream.Read(message, 0, message.Length);
                    IMessage msg = MessageFactory.FromBytes(message);
                    MessageReceived?.Invoke(this, (msg, client));
                    stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), (stream, buffer, client));
                }
            }
            catch (Exception e)
            {
                ExceptionReceived?.Invoke(this, (e, client));
            }
        }

        public void Send(IMessage message, TcpClient client)
        {
            byte[] buffer = message.ToBytes();
            client.GetStream().Write(BitConverter.GetBytes(buffer.Length), 0, 4);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}
