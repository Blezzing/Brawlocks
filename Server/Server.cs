using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Net;
using CommonLibrary;
using CommonLibrary.Debug;
using Server.Logic;

namespace Server
{
    public static class Server
    {
        #region Fields
        //Threads
        private static Thread listenerThread;
        private static Thread gameManagerThread;
        private static Dictionary<Game, Thread> gameLoopThreads = new Dictionary<Game, Thread>();

        //Public constants
        public const String ID = "server";
        public const int CLIENTS_PER_GAME = 2; 

        //Connection field
        private static Socket listenerSocket;

        //Public storage fields
        public static List<Client> clients = new List<Client>();
        public static List<Game> games = new List<Game>();

        //Public storage fields locks
        public static object clientsLock = new object();
        public static object gamesLock = new object();

        //Debug handler
        public static ConsoleHandler Informer = new ConsoleHandler();

        #endregion

        #region Main Logic
        /// <summary>
        /// The main logic of the server.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            //Prepare data
            SetupInformer();
            SetupConnection();

            //Start performing logic
            StartListenerThread();
            StartGameManagerThread();
        }

        #endregion

        #region Internal Methods
        private static void SetupInformer()
        {
            Informer.AddBasicInformation("Server running at: ", HelperFunctions.GetIP4Address);
            Informer.AddBasicInformation("Number of clients connected: ", () => { return clients.Count; });
            Informer.AddBasicInformation("Numbers of games running: ", () => { return games.Count; });
        }

        private static void SetupConnection()
        {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(HelperFunctions.GetIP4Address()), 4852);
            listenerSocket.Bind(ipe);
            listenerSocket.Listen(5);
        }

        private static void StartListenerThread()
        {
            listenerThread = new Thread(ListenerTask);
            listenerThread.Start();
        }

        private static void StartGameManagerThread()
        {
            gameManagerThread = new Thread(GameManagerTask);
            gameManagerThread.Start();
        }

        private static void StartGameLoopThread(Game game)
        {
            Thread newThread = new Thread(game.GameLoopTask);

            lock(gameLoopThreads)
            {
                gameLoopThreads.Add(game, newThread);
            }

            newThread.Start();
        }
        #endregion

        #region Tasks

        /// <summary>
        /// Listens on the listenerSocket for new connections, and constructs new clientData when found.
        /// </summary>
        private static void ListenerTask()
        {
            while (true)
            {
                //Prepare clientdata and set it to next Accepted connection (Blocks thread until one appears)
                Client newClient = new Client(listenerSocket.Accept());

                //Sends registration packet
                Packet registrationPacket = new Packet(PacketType.Registration, ID);
                registrationPacket.stringData.Add(newClient.id);
                newClient.clientSocket.Send(registrationPacket.ToBytes());

                //Adds the client on the server
                lock(clientsLock)
                {
                    clients.Add(newClient);
                }
                
                //Debug
                Informer.AddEventInformation("A new client was added! Registration packet size: " + registrationPacket.ToBytes().Length);
            }
        }

        /// <summary>
        /// Manages games, and assigns clients to them.
        /// </summary>
        private static void GameManagerTask()
        {
            List<Client> assignedClients = new List<Client>();
            List<Client> unassignedClients = new List<Client>();

            while (true)
            {
                //Update list of unassigned clients
                lock (clientsLock)
                {
                    foreach(Client client in clients)
                    {
                        if (!assignedClients.Contains(client) && !unassignedClients.Contains(client))
                        {
                            unassignedClients.Add(client);
                        }
                    }
                }

                //Handles unassigned clients
                if (unassignedClients.Count >= CLIENTS_PER_GAME)
                {
                    //Takes the first clients and puts them in a list to prepare them for a game
                    List<Client> clientsToAdd = new List<Client>();
                    for (int i = 0; i < CLIENTS_PER_GAME; i++)
                    {
                        clientsToAdd.Add(unassignedClients[i]);
                    }

                    //Moves the clients to assigned clients
                    foreach(Client client in clientsToAdd)
                    {
                        assignedClients.Add(client);
                        unassignedClients.Remove(client);
                    }

                    //Starts a game with the clients
                    Game game = new Game(clientsToAdd);
                    lock(gamesLock)
                    { 
                        games.Add(game);
                    }
                    StartGameLoopThread(game);
                }
            }
        }
        #endregion
    }
}
