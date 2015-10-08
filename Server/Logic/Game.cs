using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using CommonLibrary;
using Server.Logic.GameObjects;

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
        private Dictionary<PlayerObject, Client> playerConnections = new Dictionary<PlayerObject, Client>();

        //Variables for game logic
		private Stopwatch currentElapsedTime = new Stopwatch();
		private Timer updateClock;
		private Timer publishClock;

		//Clients associated with this game
		public readonly List<Client> Clients;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="players"></param>
        public Game(List<Client> players)
        {
			Clients = players;
            //Link players to their connections
            foreach(Client player in Clients)
            { 
                PlayerObject po = new PlayerObject();
                playerConnections.Add(po,player);
                playerObjects.Add(po);
            }

			updateClock = new Timer(50);
			updateClock.Elapsed += updateClock_Elapsed;

			publishClock = new Timer(20);
			publishClock.Elapsed += publishClock_Elapsed;
        }

        /// <summary>
        /// Eventhandler sending the updated gamestate to all clients.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void updateClock_Elapsed(object sender, ElapsedEventArgs e)
		{
			foreach(PlayerObject po in playerObjects)
			{
				po.UpdatePosition(playerConnections[po].InputData,currentElapsedTime);
			}

			//Reset time
			currentElapsedTime.Restart();
        }

		void publishClock_Elapsed(object sender, ElapsedEventArgs e)
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
			foreach (KeyValuePair<PlayerObject,Client> kvp in playerConnections)
			{
				try
				{
					kvp.Value.clientSocket.Send(p.ToBytes());
				}
				catch (System.Net.Sockets.SocketException)
				{
					//Handle dc'ed client here.
				}
			}
		}
        
        /// <summary>
        /// To be run from another thread when object is constructed.
        /// </summary>
        public void GameLoopTask()
        {
			//Start timers
            currentElapsedTime.Start();
            updateClock.Start();
			publishClock.Start();

			while (true)
			{
				/*skal ændres til noget i stil med, så længe spillet ikke er ovre(?)*/
			}

			//Stop timers
			currentElapsedTime.Start();
			updateClock.Stop();
			publishClock.Stop();

			//Inform server about game being over
			Server.EndGame(this);
        }
    }
}
