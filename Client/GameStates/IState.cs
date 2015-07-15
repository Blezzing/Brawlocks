using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Client
{
    interface IState
    {
        //Have to render stuff
        void OnUpdateFrame(FrameEventArgs e);
        void OnRenderFrame(FrameEventArgs e);

        //Have to handle input
        void OnMouseDown(MouseButtonEventArgs e);
        void OnKeyDown(KeyboardKeyEventArgs e);
        void OnMouseWheel(MouseWheelEventArgs e);
    }
}
