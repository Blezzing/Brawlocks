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
        List<GuiButton> buttons = new List<GuiButton>();

        public MainMenuState(Game owner)
        {
            game = owner;

            buttons.Add(new GuiButton("Join Game", -1f, 0.3f, delegate () { game.States.Push(new GameState(game)); }));
            buttons.Add(new GuiButton("Host Game", -1f, 0f, delegate () { game.States.Push(new GameState(game)); }));
            buttons.Add(new GuiButton("Settings", -1f, -0.3f, delegate () { game.States.Push(new MainSettingsState(game)); }));
            buttons.Add(new GuiButton("Exit Game", -1f, -0.6f, game.Close));

        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            foreach (GuiButton butt in buttons)
            {
                butt.UpdateFunc(ref game.mousePos);

            }
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            // Render background 
            GraphicsTemplates.RenderBackground(game.Textures["ArenaBackground"]); // Render lava
            GraphicsTemplates.RenderMainMenuAlphaBox(); // Render "alpha box", so text is readable


            //Old method - rendering the title
            Font font = new Font(FontFamily.GenericSansSerif, 20);
            Texture2D texTitle = GraphicsTools.GenerateTextureFromText("-BRAWLOCKS-", font);
            GraphicsTemplates.RenderText(-1.26, 0.9f, texTitle);
            
            //Render each button
            foreach (GuiButton butt in buttons)
            {
                butt.RenderFunc();
            }
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
        }

        public void OnMouseWheel(MouseWheelEventArgs e)
        {

        }
    }
}
