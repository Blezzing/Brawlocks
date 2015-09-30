using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
    class GUICheckBox : IGuiElement
    {
        public void RenderFunc()
        {
            float x = 0, y = 0;
            float h = 0.1f, w = 0.1f;
            Texture2D tex = new Texture2D();
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.LineLoop);
            GL.TexCoord2(0f, 1f); GL.Vertex2(x, y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(x + w, y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(x + w, y - h);
            GL.TexCoord2(0f, 0f); GL.Vertex2(x, y - h);
            GL.End();

            GL.Begin(PrimitiveType.LineStrip);
            GL.TexCoord2(0f, 1f); GL.Vertex2(x, y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(x + w, y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(x + w, y - h);
            GL.TexCoord2(0f, 0f); GL.Vertex2(x, y - h);
            GL.End();
        }

        public void UpdateFunc()
        {
            //
        }

        public void UpdateFunc(ref Vector2 mouse)
        {
            //
        }
    }
}
