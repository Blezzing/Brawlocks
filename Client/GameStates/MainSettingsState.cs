﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Client
{
    class MainSettingsState : IState
    {
        Game game;

        // So we know which menu the player is in
        enum settingStates { topLayer, generalLayer, audioLayer,
                            videoLayer, controlsLayer };
        int currentLayer = (int)settingStates.topLayer;

        // The titles for each layer
        Texture2D texSettingsTitle;
        Texture2D texGeneralTitle;
        Texture2D texAudioTitle;
        Texture2D texVideoTitle;
        Texture2D texControlsTitle;

        //Lists of gui elements
        List<GuiButton> topLayerButtons    = new List<GuiButton>();
        List<IGuiElement> generalLayerGUI  = new List<IGuiElement>();
        List<IGuiElement> audioLayerGUI    = new List<IGuiElement>();
        List<IGuiElement> videoLayerGUI    = new List<IGuiElement>();
        List<IGuiElement> controlsLayerGUI = new List<IGuiElement>();

        public MainSettingsState(Game owner)
        {
            game = owner;

            //Create and fill gui lists here

            // The titles for each layer
            Font font = new Font(FontFamily.GenericSansSerif, 20);
            texSettingsTitle = GraphicsTools.GenerateTextureFromText("-SETTINGS-", font);
            texGeneralTitle  = GraphicsTools.GenerateTextureFromText("-GENERAL-",  font);
            texAudioTitle    = GraphicsTools.GenerateTextureFromText("-AUDIO-",    font);
            texVideoTitle    = GraphicsTools.GenerateTextureFromText("-VIDEO-",    font);
            texControlsTitle = GraphicsTools.GenerateTextureFromText("-CONTROLS-", font);

                    /* Add the GUI elements to each layer and delegate functionality */
            //The top layer of the options menu:
            topLayerButtons.Add(new GuiButton("General",  - 1f,  0.3f, delegate () { currentLayer = (int)settingStates.generalLayer; }));
            topLayerButtons.Add(new GuiButton("Audio",    - 1f,    0f, delegate () { currentLayer = (int)settingStates.audioLayer; }));
            topLayerButtons.Add(new GuiButton("Video",    - 1f, -0.3f, delegate () { currentLayer = (int)settingStates.videoLayer; }));
            topLayerButtons.Add(new GuiButton("Controls", - 1f, -0.6f, delegate () { currentLayer = (int)settingStates.controlsLayer; }));
            topLayerButtons.Add(new GuiButton("Return",   0.5f, -0.6f, delegate () { game.States.Pop(); }));

            generalLayerGUI.Add(new GUICheckBox());
            generalLayerGUI.Add (new GuiButton("Return", 0.5f, -0.6f, delegate () { currentLayer = (int)settingStates.topLayer; }));

            audioLayerGUI.Add   (new GuiButton("Return", 0.5f, -0.6f, delegate () { currentLayer = (int)settingStates.topLayer; }));

            videoLayerGUI.Add   (new GuiButton("Return", 0.5f, -0.6f, delegate () { currentLayer = (int)settingStates.topLayer; }));

            controlsLayerGUI.Add(new GuiButton("Return", 0.5f, -0.6f, delegate () { currentLayer = (int)settingStates.topLayer; }));
        }

        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            
        }

        public void OnMouseDown(MouseButtonEventArgs e)
        {
            
        }

        public void OnMouseWheel(MouseWheelEventArgs e)
        {
            
        }

        public void OnRenderFrame(FrameEventArgs e)
        {
            //Render each element based the current layer
            switch (currentLayer)
            {
                case (int)settingStates.topLayer:
                    GraphicsTemplates.RenderText(-1f, 0.9f, texSettingsTitle);
                    foreach (GuiButton butt in topLayerButtons)
                    {
                        butt.RenderFunc();
                    }
                    break;

                case (int)settingStates.generalLayer:
                    GraphicsTemplates.RenderText(-0.95f, 0.9f, texGeneralTitle);
                    foreach (IGuiElement elem in generalLayerGUI)
                    {
                        elem.RenderFunc();
                    }
                    break;

                case (int)settingStates.audioLayer:
                    GraphicsTemplates.RenderText(-0.7f, 0.9f, texAudioTitle);
                    foreach (IGuiElement elem in audioLayerGUI)
                    {
                        elem.RenderFunc();
                    }
                    break;

                case (int)settingStates.videoLayer:
                    GraphicsTemplates.RenderText(-0.7f, 0.9f, texVideoTitle);
                    foreach (IGuiElement elem in videoLayerGUI)
                    {
                        elem.RenderFunc();
                    }
                    break;

                case (int)settingStates.controlsLayer:
                    GraphicsTemplates.RenderText(-1.1f, 0.9f, texControlsTitle);
                    foreach (IGuiElement elem in controlsLayerGUI)
                    {
                        elem.RenderFunc();
                    }
                    break;
            }
        }

        public void OnUpdateFrame(FrameEventArgs e)
        {
            switch (currentLayer)
            {
                case (int)settingStates.topLayer:
                    foreach (GuiButton butt in topLayerButtons)
                    {
                        butt.UpdateFunc(ref game.mousePos);
                    }
                    break;

                case (int)settingStates.generalLayer:
                    foreach (IGuiElement elem in generalLayerGUI)
                    {
                        elem.UpdateFunc(ref game.mousePos);
                    }
                    break;

                case (int)settingStates.audioLayer:
                    foreach (IGuiElement elem in audioLayerGUI)
                    {
                        elem.UpdateFunc(ref game.mousePos);
                    }
                    break;

                case (int)settingStates.videoLayer:
                    foreach (IGuiElement elem in videoLayerGUI)
                    {
                        elem.UpdateFunc(ref game.mousePos);
                    }
                    break;

                case (int)settingStates.controlsLayer:
                    foreach (IGuiElement elem in controlsLayerGUI)
                    {
                        elem.UpdateFunc(ref game.mousePos);
                    }
                    break;
            }
        }
    }
}