using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Client
{
    public class View
    {
        public Vector2 position;
        /// <summary>
        /// Radians, + = clockwise
        /// </summary>
        public double rotation;
        /// <summary>
        /// multiplier
        /// </summary>
        public double zoom;

        public View(Vector2 startPosition, double startZoom, double startRotation)
        {
            this.position = startPosition;
            this.zoom = startZoom;
            this.rotation = startRotation;
        }

        public void Update()
        {
            //Lad objektet selv finde nyt data fra andre objekter her (som fx. player position)
        }

        public void ApplyTransform()
        {
            Matrix4 transform = Matrix4.Identity;

            transform = Matrix4.Mult(transform, Matrix4.CreateTranslation(-position.X, -position.Y, 0));
            transform = Matrix4.Mult(transform, Matrix4.CreateRotationZ(-(float)rotation));
            transform = Matrix4.Mult(transform, Matrix4.CreateScale((float)zoom, (float)zoom, 1.0f));

            GL.MultMatrix(ref transform);
        }
    }
}
