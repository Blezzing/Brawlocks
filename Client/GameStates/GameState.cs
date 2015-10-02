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

        CommonLibrary.Vector2 oldInputDirection;
        CommonLibrary.Vector2 newInputDirection;

        public GameState(Game owner)
        {
            game = owner;
            oldInputDirection = new CommonLibrary.Vector2(0, 0);
            newInputDirection = new CommonLibrary.Vector2(0, 0);
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            //Player controls
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            newInputDirection = new CommonLibrary.Vector2(0, 0);
            if (keyboardState[game.playerControls.MoveUp])
            {
                newInputDirection.x += 1;
            }
            if (keyboardState[game.playerControls.MoveDown])
            {
                newInputDirection.x -= 1;
            }
            if (keyboardState[game.playerControls.MoveLeft])
            {
                newInputDirection.y -= 1;
            }
            if (keyboardState[game.playerControls.MoveRight])
            {
                newInputDirection.y += 1;
            }   

            if (newInputDirection != oldInputDirection)
            {
                Client.SendMovementToServer(newInputDirection);
                oldInputDirection = newInputDirection;
            }
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            //Tegn efter GAME_VIEW
            game.CurrentView = game.GAME_VIEW;

            //Tegner textures
            GraphicsTemplates.RenderBackground(game.Textures["ArenaBackground"]);
            GraphicsTemplates.RenderArena(0, 0, 1.5, game.Textures["ArenaFloor"]);

            //Render Player, fix if.
            if (game.LocalPlayerObjects.Count >= 1)
            {
                Console.WriteLine(game.LocalPlayerObjects[0].Position.x + ", " + game.LocalPlayerObjects[0].Position.y);
                GraphicsTemplates.RenderPlayer(game.LocalPlayerObjects[0].Position, game.Textures["Player"]);
            }
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
