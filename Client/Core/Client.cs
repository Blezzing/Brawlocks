using CommonLibrary;
using CommonLibrary.Debug;
using Client.GameObjects;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Client.Core;

namespace Client
{
    public static class Client
    {
        #region Fields
        //Threads
        private static Thread incomingDataThread;
        private static Thread gameWindowThread;

        //Connection fields
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static String id = "";

        //Game field
        private static Game game;
        public static Stopwatch GlobalTimer = new Stopwatch();
        
        //Debug handler
        public static ConsoleHandler Informer = new ConsoleHandler();

        #endregion

        #region Main Logic
        public static void Main(string[] args)
        {
            //Prepare data
            SetupInformer();
            GlobalTimer.Start();

            //Start performing logic
            StartIncomingDataThread();
            StartGameWindowThread();
        }
        #endregion

        #region Internal Methods
        private static void SetupInformer()
        {
            Informer.AddBasicInformation("Connection to server etablished: ", () => { return serverSocket.Connected;});
            Informer.AddBasicInformation("Connected to server at: ", () => { return serverSocket.RemoteEndPoint; });
            Informer.AddBasicInformation("Client ID: ", () => { return id; });
        }

        private static void StartIncomingDataThread()
        {
            if (incomingDataThread == null)
            {
                incomingDataThread = new Thread(IncomingDataTask);
                incomingDataThread.Start(serverSocket);
            }
        }

        private static void StartGameWindowThread()
        {
            gameWindowThread = new Thread(GameWindowTask);
            gameWindowThread.Start();
        }

        private static void DataUnpacker(Packet packet)
        {
            switch (packet.packetType)
            {
                case (PacketType.Registration):
                    id = packet.stringData[0];
                    break;

                case (PacketType.GameState):
                    if (game != null)
                    {
                        //Makes a new serverState
                        ServerState tempState = new ServerState();

                        //Initializes data in the state
                        tempState.GameStatusObject = new GameStatusObject(packet.stringData[0]);

                        tempState.PlayerObjects = new List<PlayerObject>();
                        foreach(string s in packet.stringData[1].Split('|'))
                        {
                            if (s.Length > 0)
                            {
                                tempState.PlayerObjects.Add(new PlayerObject(s));
                            }
                        }

                        tempState.StaticObjects = new List<StaticObject>();
                        foreach (string s in packet.stringData[2].Split('|'))
                        {
                            if (s.Length > 0)
                            {
                                tempState.StaticObjects.Add(new StaticObject(s));
                            }
                        }

                        tempState.DynamicObjects = new List<DynamicObject>();
                        foreach (string s in packet.stringData[3].Split('|'))
                        {
                            if (s.Length > 0)
                            {
                                tempState.DynamicObjects.Add(new DynamicObject(s));
                            }
                        }

                        tempState.ContructionTime = GlobalTimer.Elapsed;

                        //exchange data
                        lock (game.ServerStateLock)
                        {
                            game.OldServerState = game.MidServerState;
                            game.MidServerState = game.NewServerState;
                            game.NewServerState = tempState;
                        }
                    }
                    break;

                default:
                    Informer.AddEventInformation("wtf pakke modtaget!");
                    break;

            }
        }
        #endregion

        #region Tasks
        private static void GameWindowTask()
        {
            game = new Game();
            game.Run(60);
        }

        private static void IncomingDataTask(object serverSocket)
        {
            Socket socket = (Socket)serverSocket;

            byte[] buffer;
            int readBytes;

            while (true)
            {
                while (!socket.Connected)
                {
                    /*wait for connection*/
                }

                try
                {
                    while (true)
                    {
                        buffer = new byte[socket.SendBufferSize];
                        readBytes = socket.Receive(buffer);

                        if (readBytes > 0)
                        {
                            DataUnpacker(new Packet(buffer));
                        }
                    }
                }
                catch (SocketException)
                {
                    Informer.AddEventInformation("Disconnected from server.");
                }
            }
        }
        #endregion

        #region Public Methods

        public static void ConnectToServer(string ip)
        {
            IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(ip), 4852);

            try
            {
                serverSocket.Connect(serverEndpoint);
                Informer.AddEventInformation("Connected to: " + serverEndpoint.Address.ToString());
            }
            catch (Exception)
            {
                Informer.AddEventInformation("Failed to connect to server, try again.");
            }
        }
        #endregion

        #region Send Methods
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
        #endregion
    }
}
