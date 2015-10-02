using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CommonLibrary;
using CommonLibrary.Representation;

namespace Server.Logic
{
    public class Game
    {
        //Objects to manage
        private GameStatusObject gameStatusObject = new GameStatusObject();
        private List<PlayerObject> playerObjects = new List<PlayerObject>();
        private List<StaticObject> staticObjects = new List<StaticObject>();
        private List<DynamicObject> dynamicObjects = new List<DynamicObject>();

        //Sources of information
        private Dictionary<PlayerObject, ClientData> playerConnections = new Dictionary<PlayerObject, ClientData>();

        //Variables for game logic
        private Stopwatch currentElapsedTime = new Stopwatch();
        private Timer updateClock;
        private Boolean paused = false;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="players"></param>
        public Game(List<ClientData> players)
        {
            //Link players to their connections
            foreach(ClientData player in players)
            { 
                PlayerObject po = new PlayerObject();
                playerConnections.Add(po,player);
                playerObjects.Add(po);
            }

            updateClock = new Timer(50);
            updateClock.Elapsed += updateClock_Elapsed;
        }

        /// <summary>
        /// Eventhandler sending the updated gamestate to all clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void updateClock_Elapsed(object sender, ElapsedEventArgs e)
        {
            //Build packet representing state of game.
            Packet p = new Packet(PacketType.GameState, Server.ID);

            string sGameStatusObject = "";
            sGameStatusObject = gameStatusObject.ToString();

            string sPlayerObjects = "";
            foreach (PlayerObject o in playerObjects)
                sPlayerObjects += o.ToString();
            
            string sStaticObjects = "";
            foreach (StaticObject o in staticObjects)
                sStaticObjects += o.ToString();

            string sDynamicObjects = "";
            foreach (DynamicObject o in dynamicObjects)
                sDynamicObjects += o.ToString();

            p.stringData.Add(sGameStatusObject);
            p.stringData.Add(sPlayerObjects);
            p.stringData.Add(sStaticObjects);
            p.stringData.Add(sDynamicObjects);  

            //Send packet to all clients.
            foreach (KeyValuePair<PlayerObject,ClientData> kvp in playerConnections)
            {
                kvp.Value.clientSocket.Send(p.ToBytes());
            }
            Console.WriteLine("Statepakker sendt! Size: " + p.ToBytes().Length);
        }
        
        /// <summary>
        /// DOES NEVER END! Enter when everything is ready.
        /// </summary>
        public void GameLoop()
        {
            currentElapsedTime.Start();
            updateClock.Start();
            
            while(true)
            {
                while (currentElapsedTime.ElapsedMilliseconds < 10) { /*Do nothing*/ }
                foreach(PlayerObject po in playerObjects)
                {
                    po.Position += playerConnections[po].InputData.InputDirection * 0.0001f * currentElapsedTime.ElapsedMilliseconds;
                }

                currentElapsedTime.Restart();
            }
        }
    }
}
