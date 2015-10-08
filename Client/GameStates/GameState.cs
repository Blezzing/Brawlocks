using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CommonLibrary;
using Client.GameObjects;
using Client.Core;

namespace Client
{
    class GameState : IState
    {
        private Game game;

        private CommonLibrary.Vector2 oldInputDirection = new CommonLibrary.Vector2();
        private CommonLibrary.Vector2 newInputDirection = new CommonLibrary.Vector2();

        private ServerState checkState = new ServerState();

        public GameState(Game owner)
        {
            game = owner;
            Client.ConnectToServer(HelperFunctions.GetIP4Address());
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            HandleControls();
            HandleExtrapolation();
        }

        public void OnRenderFrame(FrameEventArgs e)
        {

            RenderBackground();
            RenderObjects();
            RenderGUI();
        }

        #region Update Methods
        private void HandleControls()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            newInputDirection = new CommonLibrary.Vector2();
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

        private void HandleExtrapolation()
        {
            //Validate current idea
            if (checkState != game.OldServerState)
            {
                lock(game.ServerStateLock)
                {
                    //Updates checkvalue
                    checkState = game.OldServerState;

                    //
                }
            }
        }
        #endregion

        #region Render Methods
        private void RenderBackground()
        {
            game.CurrentView = game.GAME_VIEW;
            GraphicsTemplates.RenderBackground(game.Textures["ArenaBackground"]);
            GraphicsTemplates.RenderArena(0, 0, 1.5, game.Textures["ArenaFloor"]);
        }

        private void RenderObjects()
        {
            game.CurrentView = game.GAME_VIEW;
            lock (game.ServerStateLock)
            {
                foreach (PlayerObject po in game.NewServerState.PlayerObjects)
                {
                    GraphicsTemplates.RenderPlayer(po.Position, game.Textures["Player"]);
                }
            }
        }

        private void RenderGUI()
        {
            game.CurrentView = game.GUI_VIEW;
        }
        #endregion

        #region OnXXX Methods
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
        #endregion

    }
}
