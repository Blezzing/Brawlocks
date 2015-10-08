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
    class GuiButton : IGuiElement
    {
        RectangleF rectangle;
        string txt = "Template";
        //fnt = new Font(FontFamily.GenericSansSerif, 10);
        float x;
        float y;
        Texture2D tex;
        Color colorInactive = Color.LightGray;
        Color colorHover = Color.Yellow;
        Color colorOnRender;
        Action onClickLol;
        bool wasMousePressed = false;

        public GuiButton(string text, float xPos, float yPos, Action onClickAction)
        {
            onClickLol = onClickAction;
            colorOnRender = colorInactive;
            txt = text;
            tex = GraphicsTools.GenerateTextureFromText(txt);
            x = xPos;
            y = yPos;
            rectangle = new RectangleF(x, y, tex.Width / 100, tex.Height / 100);
        }

        public GuiButton(string text, Font font, float xPos, float yPos)
        {

        }

        public void UpdateFunc() { }
        public void UpdateFunc(ref Vector2 mousePos)
        {
            if ((mousePos.X > x && mousePos.X < x + (tex.Width / 100f)) &&
                (mousePos.Y < y && mousePos.Y > y - (tex.Height / 100f)))
            {
                colorOnRender = colorHover;

                if (Mouse.GetState()[MouseButton.Left])
                {
                    wasMousePressed = true;
                }
            }
            else
            {
                colorOnRender = colorInactive;
                wasMousePressed = false;
            }

            if (wasMousePressed && !Mouse.GetState()[MouseButton.Left])
            {
                onClick();
                wasMousePressed = false;
            }
        }

        public void RenderFunc()
        {
            GraphicsTemplates.RenderButton(x, y, tex, colorOnRender);
        }

        public void onClick()
        {
            onClickLol();
        }
    }
}
