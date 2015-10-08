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
    class GUISlider : IGuiElement
    {
        // General slider values
        float x, y;
        float h = 0.1f, w = 0.5f;
        float sliderValue = 100f;

        // SliderPointer values
        float pointerX;
        //float pointerW = 0.08f;
        float pointerH = 0.1f;
        float pointerHalfW = 0.04f;
        
        bool isDragging      = false;

        Texture2D tex = new Texture2D(); // Dummy texture


        public GUISlider(float xPos, float yPos) 
            : this(xPos, yPos, 100) { }

        public GUISlider(float xPos, float yPos, float setValue) 
        {
            x = xPos;
            y = yPos - 0.07f; // 0.07 = offset to match text - fix

            // Ensuring the desired value is inbound
            if (setValue < 0)
                setValue = 0;
            else if (setValue > 100)
                setValue = 100;
            sliderValue = setValue;

            updatePointerX();
        }

        private void updatePointerX()
        {
            pointerX = x + (w / 100f * sliderValue); // Scales the x coordinate down
            Console.WriteLine("pointerX: " + x + ", "+ pointerX);
        }

        public void RenderFunc()
        {
            renderSliderBar();

            renderSliderPointer();
        }

        private void renderSliderBar()
        {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Color3(Color.Gray);
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(0f, 1f); GL.Vertex2(x, y - h);
            GL.TexCoord2(1f, 1f); GL.Vertex2(x + w, y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(x + w, y - h);
            GL.End();
        }

        private void renderSliderPointer()
        {
            GL.BindTexture(TextureTarget.Texture2D, tex.ID);
            GL.Color3(Color.DarkGoldenrod);
            GL.Begin(PrimitiveType.Triangles);
            GL.TexCoord2(0f, 1f); GL.Vertex2(pointerX - pointerHalfW, y);
            GL.TexCoord2(1f, 1f); GL.Vertex2(pointerX + pointerHalfW, y);
            GL.TexCoord2(1f, 0f); GL.Vertex2(pointerX, y - pointerH);
            GL.End();
        }

        public void UpdateFunc() { }
        public void UpdateFunc(ref Vector2 mousePos)
        {
            if ((mousePos.X > x && mousePos.X < x + w) &&
                (mousePos.Y < y && mousePos.Y > y - h))
            {

                if (Mouse.GetState()[MouseButton.Left])
                {
                    isDragging = true;
                }
            }
            if (isDragging)
            {
                updateSliderValue(mousePos.X);
            }
        }
        private void updateSliderValue(float xValue)
        {
            
            if (xValue < x)
            {
                pointerX = x;
            }
            else if (xValue > x + w)
            {
                pointerX = x + w;
            }
            else
            {
                pointerX = xValue;
            }

            // Pointer stops when mouse.left is released
            if (!Mouse.GetState()[MouseButton.Left])
            {
                isDragging = false;
            }
        }
    }
}
