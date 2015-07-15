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
        Texture2D arenaTexture;
        Texture2D backgroundTexture;

        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
            GL.Enable(EnableCap.Texture2D);
            this.Run(60);
        }
        
        /// <summary>
        /// Kaldes når vinduet er startet (Load textures, sounds etc.)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            arenaTexture = GraphicsTools.LoadTexture("LavaDirtArena.png");
            backgroundTexture = GraphicsTools.LoadTexture("LavaBackground.png");
        }

        /// <summary>
        /// Kaldes når en frame dannes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        /// <summary>
        /// Kaldes når en frame skal rendes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GraphicsTemplates.RenderBackground(backgroundTexture);
            GraphicsTemplates.RenderArena(0, 0, 1, arenaTexture);

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
        /// Kaldes når en tast trykkes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Exit();

            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;
        }

        /// <summary>
        /// Kaldes når vindues resizes (Ændre relative koordinater)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            double ratio = ((double)Width / (double)Height);

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-ratio, ratio, -1.0, 1.0, 0.0, 1.0);

            base.OnResize(e);
        }
    }
}
