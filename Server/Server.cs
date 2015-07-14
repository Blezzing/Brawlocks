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
        private static Socket listenerSocket;
        private static List<ClientData> clients;
        private const String id = "server";
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
        }
        #endregion
        
        #region Connection Layer
        private static void ListenerTask()
        {
            while (true)
            {
                listenerSocket.Listen(5); //experimental as fuck.
                ClientData newClient = new ClientData(listenerSocket.Accept());
                clients.Add(newClient);

                Packet registrationPacket = new Packet(PacketType.Registration, id);
                registrationPacket.stringData.Add(newClient.id);
                newClient.clientSocket.Send(registrationPacket.ToBytes());

                Console.WriteLine("A new client was added");
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

        public static void SendGameStateToClients(List<GameObject> gameObjects)
        {

        }
        #endregion
    }

}
