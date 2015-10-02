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

namespace Server
{
    public static class Server
    {
        #region Fields
        public const String ID = "server";
        private static Socket listenerSocket;
        public static List<ClientData> clients;

        public static List<Logic.Game> games = new List<Logic.Game>();

        #endregion

        #region Main Logic
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting server on ip: " + HelperFunctions.GetIP4Address());

            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<ClientData>();

            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(HelperFunctions.GetIP4Address()),4852);
            listenerSocket.Bind(ipe);

            Thread listenerThread = new Thread(ListenerTask);
            listenerThread.Start();

            //jesus fuck, fix this.
            while (true)
            {
                if (clients.Count >= 1)
                {
                    games.Add(new Logic.Game(clients));
                    games[0].GameLoop();
                }
            }
        }
        #endregion
        
        #region Connection Layer
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
                
                Console.WriteLine("A new client was added! Registration packet size: " + registrationPacket.ToBytes().Length);
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
