using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Representation
{
    public class PlayerObject
    {
        public Vector2 Position = new Vector2();
        public Vector2 Velocity;

        public PlayerObject()
        {
            Position = new Vector2();
        }

        public PlayerObject(String source)
        {
            string[] input = source.Split(' ');
            Position = new Vector2(float.Parse(input[1]), float.Parse(input[0]));
        }

        public override string ToString()
        {
            return Position.x.ToString() + " " + Position.y.ToString() + "|";
        }
    }
}
