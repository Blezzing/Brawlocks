using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Representation
{
    public abstract class AbstractPlayerObject
    {
        public string ID = "tag";

        public Vector2 Position = new Vector2();
        public Vector2 Velocity = new Vector2();

        public AbstractPlayerObject()
        {
            ID = Guid.NewGuid().ToString();
        }

        public AbstractPlayerObject(String source)
        {
            string[] input = source.Split(' ');
            Position = new Vector2(float.Parse(input[2]), float.Parse(input[1]));
            ID = input[0];
        }

        public override string ToString()
        {
            return ID + " " + Position.x.ToString() + " " + Position.y.ToString() + "|";
        }
    }
}
