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
        Texture2D cursorTexture;
        double mouseXPos = 0;
        double mouseYPos = 0;

        View view;

        double aspectRatio;

        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
            GL.Enable(EnableCap.Texture2D);

            view = new View(Vector2.Zero, 1.0, 0.0);

            this.Run(60);
        }
        
        /// <summary>
        /// Kaldes når vinduet er startet (Load textures, sounds etc.)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            aspectRatio = ((double)Width / (double)Height);

            arenaTexture = GraphicsTools.LoadTexture("StoneArena.png");
            backgroundTexture = GraphicsTools.LoadTexture("LavaBackground.png");
            cursorTexture = GraphicsTools.LoadTexture("Cursor.png");
        }

        /// <summary>
        /// Kaldes når en frame dannes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            mouseXPos = Mouse.X;
            mouseYPos = Mouse.Y;

            mouseXPos = ((mouseXPos / Width) * 2 * aspectRatio - aspectRatio);
            mouseYPos = ((mouseYPos / Height) *2 - 1) * view.zoom;

            Console.WriteLine("X: " + mouseXPos + " Y: " + mouseYPos);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

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

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.LoadIdentity();
            GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 1.0);
            view.ApplyTransform();
            
            GraphicsTemplates.RenderBackground(backgroundTexture);
            GraphicsTemplates.RenderArena(0, 0, 1.5, arenaTexture);

            GraphicsTemplates.RenderMouse(mouseXPos, -mouseYPos, cursorTexture);

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
            aspectRatio = ((double)Width / (double)Height);

            GL.Viewport(0, 0, Width, Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 1.0);

            base.OnResize(e);
        }
    }
}
