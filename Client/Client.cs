using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public static class Client
    {
        #region Private Fields
        private static Socket serverSocket;
        private static String id;
        private static Thread incomingDataThread;
        private static Game game;
        #endregion

        #region Main Logic
        public static void Main(string[] args)
        {
            //TODO  make this better.
            Console.WriteLine("Enter server IP: ");
            
            //TODO Verify this shit.
            String ip = Console.ReadLine();

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), 4852);

            //TODO improve information
            while (!serverSocket.Connected)
            {
                try
                {
                    serverSocket.Connect(ipe);
                }
                catch (Exception ex)
                {
                }
            }

            incomingDataThread = new Thread(IncomingDataTask);
            incomingDataThread.Start(serverSocket);

            game = new Game();
        }
        #endregion

        #region Connection Layer
        private static void IncomingDataTask(object serverSocket)
        {
            Socket sSocket = (Socket)serverSocket;

            byte[] buffer;
            int readBytes;

            try
            {    
                while (true)
                {                   
                    buffer = new byte[sSocket.SendBufferSize];
                    readBytes = sSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        DataManager(new Packet(buffer));
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Disconnected from server.");
            }            
        }
        private static void DataManager(Packet packet)
        {
            switch(packet.packetType)
            {
                case (PacketType.Registration):
                    id = packet.stringData[0];
                    break;
            }
        }
        #endregion

        #region Surface Layer
        public static void SendActionToServer(int actionNumber, Vector2 position)
        {
            Packet p = new Packet(PacketType.Action, id);
            p.stringData.Add(actionNumber.ToString());
            p.stringData.Add(position.x.ToString());
            p.stringData.Add(position.y.ToString());
            serverSocket.Send(p.ToBytes());
        }

        public static void SendMovementToServer(Vector2 direction)
        {
            Packet p = new Packet(PacketType.Action, id);
            p.stringData.Add(direction.x.ToString());
            p.stringData.Add(direction.y.ToString());
            serverSocket.Send(p.ToBytes());
        }

        public static void SendMessageToServer(String message)
        {
            Packet p = new Packet(PacketType.Message, id);
            p.stringData.Add(message);
            serverSocket.Send(p.ToBytes());
        }

        public static int PingServer()
        {
            //Stopwatch ligcszqwer plox
            Packet p = new Packet(PacketType.Ping, id);
            serverSocket.Send(p.ToBytes());
            return 0;
        }
        #endregion
    }
}
