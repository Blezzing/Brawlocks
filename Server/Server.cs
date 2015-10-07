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

namespace Server
{
    public static class Server
    {
        #region Fields
        public const String ID = "server";
        private static Socket listenerSocket;
        private static IPEndPoint ipe;

        public static List<ClientData> clients = new List<ClientData>();
        public static List<Logic.Game> games = new List<Logic.Game>();

        public static ConsoleHandler Informer = new ConsoleHandler(null);

        private static Thread listenerThread;
        private static Thread gameManagerThread;
        #endregion

        #region Main Logic
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
            Informer.AddBasicInformation("Server running on ip: ", HelperFunctions.GetIP4Address);
            Informer.AddBasicInformation("Number of clients connected: ", () => { return clients.Count; });
            Informer.AddBasicInformation("Numbers of games running: ", () => { return games.Count; });
        }

        private static void SetupConnection()
        {
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ipe = new IPEndPoint(IPAddress.Parse(HelperFunctions.GetIP4Address()), 4852);
            listenerSocket.Bind(ipe);
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
        #endregion

        #region Tasks
        private static void ListenerTask()
        {
            while (true)
            {
                listenerSocket.Listen(5); //experimental as 5.
                ClientData newClient = new ClientData(listenerSocket.Accept());
                clients.Add(newClient);

                Packet registrationPacket = new Packet(PacketType.Registration, ID);
                registrationPacket.stringData.Add(newClient.id);
                newClient.clientSocket.Send(registrationPacket.ToBytes());
                
                Informer.AddEventInformation("A new client was added! Registration packet size: " + registrationPacket.ToBytes().Length);
            }
        }

        private static void GameManagerTask()
        {
            while (true)
            {
                //fuck this
                if (clients.Count >= 1)
                {
                    games.Add(new Logic.Game(clients));
                    games[0].GameLoop();
                }
            }
        }
        #endregion

        #region Surface Layer
        public static void SendMessageToClients(String Message)
        {

        }

        public static void SendMessageToClient(String Message, String recieverID)
        {

        }
        #endregion
    }

}
