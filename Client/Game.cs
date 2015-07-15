using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;


namespace Client
{
    class Game : GameWindow
    {
        #region Fields
        Texture2D arenaTexture;
        Texture2D backgroundTexture;
        Texture2D cursorTexture;
        double mouseXPos = 0;
        double mouseYPos = 0;

        View view;

        double aspectRatio;
        #endregion
        
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
            GL.Enable(EnableCap.Texture2D);

            view = new View(Vector2.Zero, 1.0, 0.0);

            this.Run(60);
        }
        #endregion

        #region Overrides fra GameWindow
        /// <summary>
        /// Kaldes når vinduet er startet (Load textures, sounds etc.)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Calculate variable values
            aspectRatio = ((double)Width / (double)Height);

            //Load textures
            arenaTexture = GraphicsTools.LoadTexture("StoneArena.png");
            backgroundTexture = GraphicsTools.LoadTexture("LavaBackground.png");
            cursorTexture = GraphicsTools.LoadTexture("Cursor.png");
        }

        /// <summary>
        /// Kaldes når vindues resizes (Ændre relative koordinater)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            //Recalculate aspect ratio for use in on render frame
            aspectRatio = ((double)Width / (double)Height);

            //Ensure fitting viewport
            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
        }

        /// <summary>
        /// Kaldes når en frame dannes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
<<<<<<< HEAD

            mouseXPos = Mouse.X;
            mouseYPos = Mouse.Y;

            mouseXPos = ((mouseXPos / Width) * 2 * aspectRatio - aspectRatio);
            mouseYPos = ((mouseYPos / Height) *2 - 1) * view.zoom;

            Console.WriteLine("X: " + mouseXPos + " Y: " + mouseYPos);
        }
=======
>>>>>>> origin/master

            //Gamelogic
        }
        
        /// <summary>
        /// Kaldes når mussehjulet drejes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            //Changes the view for next OnRenderFrame
            if (e.DeltaPrecise > 0)
                view.zoom += 0.05;
            else if (e.DeltaPrecise < 0)
                view.zoom -= 0.05;
        }

        /// <summary>
        /// Kaldes når en frame skal rendes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            //Tømmer buffer
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Opsætter grafisk rum
            GL.LoadIdentity();
            GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 1.0);
            view.ApplyTransform();
            
            //Tegner textures
            GraphicsTemplates.RenderBackground(backgroundTexture);
            GraphicsTemplates.RenderArena(0, 0, 1.5, arenaTexture);

<<<<<<< HEAD
            GraphicsTemplates.RenderMouse(mouseXPos, -mouseYPos, cursorTexture);

=======
            //Swapper buffers
>>>>>>> origin/master
            this.SwapBuffers();
        }

        /// <summary>
        /// Kaldes når der klikkes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Kaldes når en tast trykkes (som ikke har med gamelogic at gøre)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //Afslut gamewindow
            if (e.Key == Key.Escape)
                this.Exit();

            //Skift mellem fullscreen og window mode
            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
        }
        #endregion
    }
}
