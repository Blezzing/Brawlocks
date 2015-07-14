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
    public class ClientData
    {
        #region Public Fields
        public readonly Socket clientSocket;
        public readonly Thread clientThread;
        public readonly String id;
        #endregion

        #region Constructors
        public ClientData()
        {
            this.clientThread = new Thread(IncomingDataTask);
            this.id = Guid.NewGuid().ToString();

            this.clientThread.Start(clientSocket);
        }
        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            this.clientThread = new Thread(IncomingDataTask);
            this.id = Guid.NewGuid().ToString();

            this.clientThread.Start(clientSocket);
        }
        #endregion

        #region Connection Layer
        private static void IncomingDataTask(object clientSocket)
        {
            Socket cSocket = (Socket)clientSocket;

            byte[] buffer;
            int readBytes;

            try
            {
                while (true)
                {
                    buffer = new byte[cSocket.SendBufferSize];
                    readBytes = cSocket.Receive(buffer);

                    if (readBytes > 0)
                    {
                        Packet packet = new Packet(buffer);
                        DataManager(packet);
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("A client disconnected");
            }
        }

        private static void DataManager(Packet packet)
        {
            switch (packet.packetType)
            {
                case (PacketType.Registration):
                    //should never happen.
                    break;
            }
        }
        #endregion

        #region Surface Layer
        //empty so far.
        #endregion
    }
}
