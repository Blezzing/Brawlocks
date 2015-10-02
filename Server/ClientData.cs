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
        //Communication fields
        public readonly Socket clientSocket;
        public readonly Thread clientThread;
        public readonly String id;

        //Recieved stuff
        public Input InputData;
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

            this.InputData = new Input();

            this.clientThread.Start(new Tuple<Socket,Input>(clientSocket,InputData));
        }
        #endregion

        #region CollectionClass
        public class Input
        {
            public Vector2 InputDirection = new Vector2();
            public Vector2 AimDirection = new Vector2();
        }
        #endregion

        #region Connection Layer
        private static void IncomingDataTask(object tuple)
        {
            Socket cSocket  = ((Tuple<Socket, Input>)tuple).Item1;
            Input input = ((Tuple<Socket, Input>)tuple).Item2;

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
                        DataManager(packet, input);
                    }
                }
            }
            catch (SocketException)
            {
                Server.Informer.AddEventInformation("A client disconnected");
            }
        }

        private static void DataManager(Packet packet, Input input)
        {
            switch (packet.packetType)
            {
                case (PacketType.Registration):
                    //should never happen.
                    break;
                case (PacketType.Movement):
                    input.InputDirection.x = float.Parse(packet.stringData[0]);
                    input.InputDirection.y = float.Parse(packet.stringData[1]);
                    Server.Informer.AddEventInformation("Movement pakke modtaget!",2);
                    break;
            }
        }
        #endregion

        #region Surface Layer
        //empty so far.
        #endregion
    }
}
