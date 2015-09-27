﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Client
{
    interface IGuiElement
    {
        void UpdateFunc();
        void RenderFunc();
    }
}
