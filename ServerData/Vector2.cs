using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Vector2
    {
        public float x, y;

        public Vector2()
        {
            this.x = 0f;
            this.y = 0f;
        }

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(Vector2 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
        }

        public void Normalize()
        {
            float l = (float)Math.Sqrt(((x * x) + (y * y)));
            x /= l;
            y /= l;
        }
    }
}
