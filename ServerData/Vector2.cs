using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

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

        public Vector2 Normalize()
        {
            if (x != 0 || y != 0)
            {
                float l = (float)Math.Sqrt(((x * x) + (y * y)));
                x /= l;
                y /= l;

                return new Vector2(x, y);
            }
            else
            {
                return new Vector2();
            }
        }

        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            return ((v1.x == v2.x) && (v1.y == v2.y));
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !((v1.x == v2.x) && (v1.y == v2.y));
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2 operator *(Vector2 v1, long v2)
        {
            return new Vector2(v1.x * v2, v1.y * v2);
        }

        public static Vector2 operator *(Vector2 v1, float v2)
        {
            return new Vector2(v1.x * v2, v1.y * v2);
        }
    }
}
