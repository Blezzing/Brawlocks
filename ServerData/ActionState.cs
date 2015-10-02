using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonLibrary
{
    public class ActionState
    {
        Vector2 position;
        Vector2 direction;
        int type;
        int senderID;

        public ActionState(int senderID, int type)
        {
            this.position = new Vector2(0, 0);
            this.direction = new Vector2(0, 0);
            this.type = type;
            this.senderID = senderID;
        }
    }
}
