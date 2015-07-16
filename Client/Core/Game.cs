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

        public Dictionary<String, Texture2D> Textures = new Dictionary<String,Texture2D>();
        public Dictionary<String, Object> Sounds = new Dictionary<String,Object>(); //Change this to a sound-type when created.
        public Stack<IState> States = new Stack<IState>();

        public double aspectRatio;
        public View view; //Bør denne ikke være per state?


        double mouseXPos = 0;
        double mouseYPos = 0;

        //Player character stuff
        double playerXPos = 0;
        double playerYPos = 0;
        double playerSpeed = 1;
        PlayerControls playerControls = new PlayerControls("Userprefs.txt");

        #endregion
        
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {
            GL.Enable(EnableCap.Texture2D);                                                 //enable textures
            GL.Enable(EnableCap.Blend);                                                     //enable transparency
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);  //sets up alpha scaling

            States.Push(new MainMenuState(this));

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

            UpdateAspectRatio();

            LoadTextures();
            LoadSounds();
        }

        /// <summary>
        /// Kaldes når vindues resizes (Ændre relative koordinater)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            UpdateAspectRatio();

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

            //Mouse location relative to screen
            mouseXPos = Mouse.X;
            mouseYPos = Mouse.Y;
            mouseXPos = ((mouseXPos / Width) * 2 * aspectRatio - aspectRatio);
            mouseYPos = ((mouseYPos / Height) *2 - 1) * view.zoom;
            
            //Player controls
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[playerControls.MoveUp])
            {
                playerYPos += playerSpeed * e.Time;
            }
            if (keyboardState[playerControls.MoveDown])
            {
                playerYPos -= playerSpeed * e.Time;
            }
            if (keyboardState[playerControls.MoveLeft])
            {
                playerXPos -= playerSpeed * e.Time;
            }
            if (keyboardState[playerControls.MoveRight])
            {
                playerXPos += playerSpeed * e.Time;
            }

            States.Peek().OnUpdateFrame(e);
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
            GraphicsTemplates.RenderBackground(Textures["ArenaBackground"]);
            GraphicsTemplates.RenderArena(0, 0, 1.5, Textures["ArenaFloor"]);

            //Render Player
            GraphicsTemplates.RenderPlayer(playerXPos, playerYPos, Textures["Player"]);

            //Render custom mouse cursor
            GraphicsTemplates.RenderMouse(mouseXPos, -mouseYPos, Textures["Cursor"]);

            //Swapper buffers
            this.SwapBuffers();

            States.Peek().OnRenderFrame(e);
        }

        /// <summary>
        /// Kaldes når der klikkes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            States.Peek().OnMouseDown(e);
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

            States.Peek().OnMouseWheel(e);
        }

        /// <summary>
        /// Kaldes når en tast trykkes (som ikke har med gamelogic at gøre)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            //Skift mellem fullscreen og window mode
            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;

            States.Peek().OnKeyDown(e);
        }
        #endregion
        private void LoadTextures()
        {
            Textures.Add("ArenaFloor",GraphicsTools.LoadTexture("StoneArena.png"));
            Textures.Add("ArenaBackground",GraphicsTools.LoadTexture("LavaBackground.png"));
            Textures.Add("Cursor",GraphicsTools.LoadTexture("Cursor.png"));
            Textures.Add("Player",GraphicsTools.LoadTexture("PlayerTemplate.png"));
        }

        private void LoadSounds()
        {

        }

        private void UpdateAspectRatio()
        {
            aspectRatio = ((double)Width / (double)Height);
        }
    }
}
