using CommonLibrary;
using CommonLibrary.Representation;
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
        private static Thread gameLoopThread;
        private static Game game;
        #endregion

        #region Main Logic
        public static void Main(string[] args)
        {
            //Forbered socket
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //Autoconnect to own ip, in case a server is hosted.
            IPEndPoint clientEndpoint = new IPEndPoint(IPAddress.Parse(HelperFunctions.GetIP4Address()), 4852);
            
            try
            {
                serverSocket.Connect(clientEndpoint);
                Console.WriteLine("Connected to: " + clientEndpoint.Address.ToString());
            }
            catch (Exception)
            {
            }

            //Connect to another ip. (Needs sanitizing)
            while (!serverSocket.Connected)
            {
                Console.WriteLine("Enter server IP: ");
                String ip = Console.ReadLine();

                IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(ip), 4852);

                try
                {
                    serverSocket.Connect(serverEndpoint);
                    Console.WriteLine("Connected to: " + serverEndpoint.Address.ToString());
                    break;
                }
                catch (Exception)
                {
                }
                Console.WriteLine("Failed to connect to server, try again.");
            }
            
            game = new Game();

            incomingDataThread = new Thread(IncomingDataTask);
            incomingDataThread.Start(serverSocket);

            game.Run(60);

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
            catch (SocketException)
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
                case (PacketType.GameState):
                    if (game != null)
                    {
                        game.LocalGameStatusObject = new GameStatusObject(packet.stringData[0]);

                        List<string> sPlayerObjects = packet.stringData[1].Split('|').ToList();
                        game.LocalPlayerObjects = new List<PlayerObject>();
                        foreach (string s in sPlayerObjects)
                            if (s.Length > 0)
                                game.LocalPlayerObjects.Add(new PlayerObject(s));

                        List<string> sStaticObjects = packet.stringData[2].Split('|').ToList();
                        game.LocalStaticObjects = new List<StaticObject>();
                        foreach (string s in sStaticObjects)
                            if (s.Length > 0)
                                game.LocalStaticObjects.Add(new StaticObject(s));

                        List<string> sDynamicObjects = packet.stringData[3].Split('|').ToList();
                        game.LocalDynamicObjects = new List<DynamicObject>();
                        foreach (string s in sDynamicObjects)
                            if (s.Length > 0)
                                game.LocalDynamicObjects.Add(new DynamicObject(s));

                        Console.WriteLine("Gamestate pakke håndteret!");
                    }
                    Console.WriteLine("Gamestate pakke modtaget!");
                    break;
                default:
                    Console.WriteLine("wtf pakke modtaget!");
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
            Packet p = new Packet(PacketType.Movement, id);
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
            return 0;//to be ping.
        }
        #endregion
    }
}
