using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using CommonLibrary.Representation;


namespace Client
{
    public class Game : GameWindow
    {
        #region Fields

        //Shared content
        public readonly Dictionary<String, Texture2D> Textures = new Dictionary<String,Texture2D>();
        public readonly Dictionary<String, Object> Sounds = new Dictionary<String, Object>(); //Change this to a sound-type when created.
        public PlayerControls playerControls = new PlayerControls("Userprefs.txt");

        //Game-Relevant Data
        public GameStatusObject LocalGameStatusObject = new GameStatusObject();
        public List<PlayerObject> LocalPlayerObjects = new List<PlayerObject>();
        public List<StaticObject> LocalStaticObjects = new List<StaticObject>();
        public List<DynamicObject> LocalDynamicObjects = new List<DynamicObject>();

        //constants
        public readonly View GAME_VIEW = new View(Vector2.Zero, 1.0, 0.0);
        public readonly View GUI_VIEW = new View(Vector2.Zero, 1.0, 0.0);
        
        //State-stack
        public Stack<IState> States = new Stack<IState>();

        //Window related properties
        public float aspectRatio;
        private View currentView;
        public View CurrentView
        {
            get { return currentView; }
            set
            {
                GL.LoadIdentity();
                GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 1.0);
                currentView = value;
                currentView.ApplyTransform();
            }
        }

        public Vector2 mousePos = new Vector2(0,0);

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
            this.CursorVisible = false;                                                     //Fanger musen og gør den usynlig

            GraphicsTemplates.currentGame = this;
            this.States.Push(new MainMenuState(this));
            this.currentView = this.GUI_VIEW;
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
            GL.Viewport(0, 0, Width, Height);
        }

        /// <summary>
        /// Kaldes når en frame dannes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (this.Focused) // Only do logic when the game is actually in focus
            {
                base.OnUpdateFrame(e);

                UpdateMouse();

                States.Peek().OnUpdateFrame(e);
            }
        }

        /// <summary>
        /// Kaldes når en frame skal rendes
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            /////////////////////////////////////////////////////////////
            // Fremgangsmåden her er følgende:                         //
            // #1. Clear buffer                                        //
            // #2. Sæt CurrentView                                     //
            // #3. Tegn en mængde af ting, og gå til #2 eller #4.      //
            // #4. Swap buffers                                        //
            // #?. Profit.                                             //
            /////////////////////////////////////////////////////////////

            GL.LoadIdentity();
            GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 1.0);
            //Tømmer buffer
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Lad statet tegne
            this.States.Peek().OnRenderFrame(e);

            //Tegn mus
            GL.LoadIdentity();
            GL.Ortho(-aspectRatio, aspectRatio, -1.0, 1.0, 0.0, 1.0);
            this.CurrentView = this.GUI_VIEW;
            GraphicsTemplates.RenderMouse(mousePos, Textures["Cursor"]);

            //Swapper buffers
            this.SwapBuffers();
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

            States.Peek().OnMouseWheel(e);
        }

        /// <summary>
        /// Kaldes når en tast trykkes (som ikke har med gamelogic at gøre)
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            //Skift mellem fullscreen og window mode
            if (e.Key == Key.F11)
                if (this.WindowState == WindowState.Fullscreen)
                    this.WindowState = WindowState.Normal;
                else
                    this.WindowState = WindowState.Fullscreen;

            if (e.Key == Key.Escape)
            {
                States.Pop();

                if (States.Count <= 0)
                {
                    this.Close();
                    Environment.Exit(1);
                }
            }

            States.Peek().OnKeyDown(e);
        }
        #endregion

        #region Private methods
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
            aspectRatio = Width / (float)Height;
        }

        private void UpdateMouse()
        {
            mousePos.X = ((Mouse.X / (float)Width) * 2 * (float)aspectRatio) - (float)aspectRatio;
            mousePos.Y = -(((Mouse.Y / (float)Height) * 2) - 1);
        }
        #endregion
    }
}
