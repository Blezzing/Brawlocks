using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.GameObjects;

namespace Client.Core
{
    public class ServerState
    {
        public GameStatusObject GameStatusObject = new GameStatusObject();
        public List<PlayerObject> PlayerObjects = new List<PlayerObject>();
        public List<DynamicObject> DynamicObjects = new List<DynamicObject>();
        public List<StaticObject> StaticObjects = new List<StaticObject>();

        public ServerState()
        {

        }
    }
}
