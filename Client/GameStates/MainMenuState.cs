using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Client
{
    class MainMenuState : IState
    {
        Game game;

        public MainMenuState(Game owner)
        {
            game = owner;

        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            Font font = new Font(FontFamily.GenericSansSerif, 20);
            Texture2D texTitle = GraphicsTools.GenerateTextureFromText("-BRAWLOCKS-", font);
            GraphicsTemplates.RenderText(-1.2, 0.5, texTitle);
            Texture2D texJoin = GraphicsTools.GenerateTextureFromText("Join Game");
            GraphicsTemplates.RenderText(-1, 0, texJoin);
            Texture2D texHost = GraphicsTools.GenerateTextureFromText("Host Game");
            GraphicsTemplates.RenderText(-1, -0.2, texHost);
            Texture2D texSettings = GraphicsTools.GenerateTextureFromText("Settings");
            GraphicsTemplates.RenderText(-1, -0.4, texSettings);
            Texture2D texExit = GraphicsTools.GenerateTextureFromText("Exit");
            GraphicsTemplates.RenderText(-1, -0.6, texExit);
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            
        }

        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                game.States.Push(new GameState(game));
            }
            if (e.Key == Key.Escape)
            {
                game.Close();
            }
        }

        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            
        }
    }
}
