using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Client
{
    public static class GraphicsTools
    {
        public static Texture2D LoadTexture(String path)
        {
            int id = GL.GenTexture(); //get id
            GL.BindTexture(TextureTarget.Texture2D, id);

            Bitmap bmp = new Bitmap("Content/Textures/" + path);
            BitmapData data = bmp.LockBits(
                new Rectangle(0,0,bmp.Width,bmp.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return new Texture2D(id, bmp.Width, bmp.Height);
        }

        public static Texture2D GenerateTextureFromText(string txt)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 10);
            return GenerateTextureFromText(txt, font);
        }

        public static Texture2D GenerateTextureFromText(string txt, Font font)
        {
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            Image img = DrawTextTexture(txt, font, Color.Blue, Color.Empty);
            Bitmap textPic = new Bitmap(img, img.Width, img.Height);
            BitmapData data = textPic.LockBits(
                new Rectangle(0, 0, textPic.Width, textPic.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte, data.Scan0);
            textPic.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


            return new Texture2D(id, textPic.Width, textPic.Height);
        }

        //Used by GenerateTextureFromText()
        private static Image DrawTextTexture(String text, Font font, Color textColor, Color backColor)
        {
            //Dummy bitmap
            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //Mesure string size
            SizeF textSize = drawing.MeasureString(text, font);

            //Free dummy
            img.Dispose();
            drawing.Dispose();

            //Create new imgage with the right size
            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

            drawing = Graphics.FromImage(img);

            //Background color
            drawing.Clear(backColor);

            //Brush for the text
            Brush textBrush = new SolidBrush(textColor);

            drawing.DrawString(text, font, textBrush, 0, 0);

            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            return img;
        }

    }
}
