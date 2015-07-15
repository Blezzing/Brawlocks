using System;
using System.Drawing;
using OpenTK;
using OpenTK.Input;


namespace Client
{
    class Game : GameWindow
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Game()
        {

        }
        
        /// <summary>
        /// Kaldes når vinduet er startet
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
            base.OnKeyDown(e);
        }


        
        //TODO add game here
    }
}
