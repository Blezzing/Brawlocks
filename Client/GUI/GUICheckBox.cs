using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Client
{
    class GUICheckBox : IGuiElement
    {
        float x, y;
        float h = 0.1f, w = 0.1f;

        Texture2D tex = new Texture2D(); // Dummy texture

        public bool checkedStatus = false;
        bool wasMousePressed = false;


        public GUICheckBox(float xPos, float yPos) 
            : this (xPos, yPos, false) { }
        public GUICheckBox(float xPos, float yPos, bool startCheckedStatus)
        {
            x = xPos;
            y = yPos - 0.07f; // 0.07 = offset to match text - fix

            checkedStatus = startCheckedStatus;
        }

        public void RenderFunc()
        {
            drawBox();

            if (checkedStatus)
            {
                drawCheckMark();
            }
        }

        private void drawBox()
        {
            // Box
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Color3(Color.Gray);
            GL.Begin(PrimitiveType.LineLoop);
            GL.TexCoord2(0f, 1f); GL.Vertex2(x, y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(x + w, y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(x + w, y - h);
            GL.TexCoord2(0f, 0f); GL.Vertex2(x, y - h);
            GL.End();
        }

         private void drawCheckMark()
        {
            // Flueben
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Color3(Color.Gray);
            GL.Begin(PrimitiveType.LineStrip);
            GL.TexCoord2(0f, 1f); GL.Vertex2(x, y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(x + w - 0.05f, y - h);
            GL.TexCoord2(1f, 0f); GL.Vertex2(x + w + 0.05f, y + 0.05f);
            GL.End();
        }

        public void UpdateFunc()
        {
            //
        }

        public void UpdateFunc(ref Vector2 mousePos)
        {
            if ((mousePos.X > x && mousePos.X < x + w) &&
                (mousePos.Y < y && mousePos.Y > y - h))
            {

                if (Mouse.GetState()[MouseButton.Left])
                {
                    wasMousePressed = true;
                }
            }
            else
            {
                wasMousePressed = false;
            }

            if (wasMousePressed && !Mouse.GetState()[MouseButton.Left])
            {
                onClick();
                wasMousePressed = false;
            }
        }

        public void onClick()
        {
            checkedStatus = !checkedStatus;
        }
    }
}
