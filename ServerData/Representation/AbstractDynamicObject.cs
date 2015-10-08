using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Representation
{
    public abstract class AbstractDynamicObject
    {
        public Vector2 Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public AbstractDynamicObject()
        {
            Position = new Vector2();
        }

        public AbstractDynamicObject(String source)
        {
            string[] input = source.Split(' ');
            Position = new Vector2(float.Parse(input[0]), float.Parse(input[0]));
        }

        public override string ToString()
        {
            return Position.x.ToString() + " " + Position.y.ToString() + "|";
        }
    } 
}
