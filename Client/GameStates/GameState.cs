using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CommonLibrary;
using CommonLibrary.Representation;

namespace Client
{
    class GameState : IState
    {
        Game game;

        CommonLibrary.Vector2 oldInputDirection = new CommonLibrary.Vector2();
        CommonLibrary.Vector2 newInputDirection = new CommonLibrary.Vector2();

        public GameState(Game owner)
        {
            game = owner;
			Client.ConnectToServer (HelperFunctions.GetIP4Address());
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

			lock (game.LocalPlayerObjectsLock) 
			{
				foreach (PlayerObject po in game.LocalPlayerObjects) 
				{
					GraphicsTemplates.RenderPlayer(po.Position, game.Textures["Player"]);
				}
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
