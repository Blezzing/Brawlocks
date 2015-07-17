using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Client
{
    class GameState : IState
    {
        Game game;

        //clean this.
        double playerXPos = 0;
        double playerYPos = 0;
        double playerSpeed = 1;

        public GameState(Game owner)
        {
            game = owner;
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            //clean this.
            //Player controls
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[game.playerControls.MoveUp])
            {
                playerYPos += playerSpeed * e.Time;
            }
            if (keyboardState[game.playerControls.MoveDown])
            {
                playerYPos -= playerSpeed * e.Time;
            }
            if (keyboardState[game.playerControls.MoveLeft])
            {
                playerXPos -= playerSpeed * e.Time;
            }
            if (keyboardState[game.playerControls.MoveRight])
            {
                playerXPos += playerSpeed * e.Time;
            }   
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            //Tegn efter GAME_VIEW
            game.CurrentView = game.GAME_VIEW;

            //Tegner textures
            GraphicsTemplates.RenderBackground(game.Textures["ArenaBackground"]);
            GraphicsTemplates.RenderArena(0, 0, 1.5, game.Textures["ArenaFloor"]);

            //Render Player
            GraphicsTemplates.RenderPlayer(playerXPos, playerYPos, game.Textures["Player"]);

            //Tegn efter GUI_VIEW
            game.CurrentView = game.GUI_VIEW;

            //MAKE GUI xD
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            
        }

        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            
        }

        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.DeltaPrecise > 0)
                game.GAME_VIEW.zoom += 0.05;
            else if (e.DeltaPrecise < 0)
                game.GAME_VIEW.zoom -= 0.05;
        }
    }
}
