using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
    public static class GraphicsTemplates
    {
        static double var = 0, difX, difY;
        public static void RenderBackground(Texture2D texture)
        {
            if (var >= Math.PI*2) var = 0;
            else var += 0.01;

            difX = Math.Cos(var)/100;
            difY = Math.Sin(var)/100;

            GL.Color3(1-difX*30, 1-difX*30, 1);

            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0 - difY, 4 - difY);
            GL.Vertex2(-8, 8);
            GL.TexCoord2(4 - difY, 4 - difY);
            GL.Vertex2(8, 8);
            GL.TexCoord2(4 - difX, 0 - difX);
            GL.Vertex2(8, -8);
            GL.TexCoord2(0 - difX, 0 - difX);
            GL.Vertex2(-8, -8);
            GL.End(); 
        }

        //TODO Optimize this
        public static void RenderArena(double centerX, double centerY, double radius, Texture2D texture)
        {
            GL.BindTexture(TextureTarget.Texture2D, texture.ID);
            GL.Color3(Color.BlanchedAlmond);
            GL.Begin(PrimitiveType.Polygon);

            int n = 256;
            double textureRadius = 0.25;

            for (int i = 0; i < n; i++)
            {
                GL.TexCoord2((centerX + 0.5 + textureRadius * Math.Cos(2 * Math.PI * i / n)), -(centerY - 0.5 + textureRadius * Math.Sin(2 * Math.PI * i / n)));
                GL.Vertex2(centerX + radius * Math.Cos(2 * Math.PI * i / n), centerY + radius * Math.Sin(2 * Math.PI * i / n));
            }
            GL.End();
        }

        public static void RenderMouse(double x, double y, Texture2D tex)
        {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            double off = 0.2;
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0);     GL.Vertex2(x, y);
            GL.TexCoord2(1, 0);     GL.Vertex2(x + off, y);
            GL.TexCoord2(1, 1);     GL.Vertex2(x + off, y - off);
            GL.TexCoord2(0, 1);     GL.Vertex2(x, y - off);

            GL.End();
        }

        public static void RenderPlayer(double x, double y, Texture2D tex)
        {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            double off = 0.2;
            GL.Begin(PrimitiveType.Quads);

            GL.TexCoord2(0, 0); GL.Vertex2(x, y);
            GL.TexCoord2(1, 0); GL.Vertex2(x + off, y);
            GL.TexCoord2(1, 1); GL.Vertex2(x + off, y - off);
            GL.TexCoord2(0, 1); GL.Vertex2(x, y - off);

            GL.End();
        }
    }
}
