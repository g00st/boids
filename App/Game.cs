using App.Engine.ImGuisStuff;
using App.Engine.Template;
using ImGuiNET;

namespace App;

using OpenTK.Graphics.OpenGL4;
using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using System.Drawing;
using System.Net.Mail;
using OpenTK.Mathematics;
using Engine;




   
public class Game : GameWindow
{
        ImGuiController _controller;
        View Main = new View();
        private int Width;
        private int Height;
        private ColoredRectpParticles par;
        private ColoredRectpParticles par2;
        private ColoredRectpParticles par3;
    
        private const int TargetFPS = 60; // Set your target FPS here
        private DateTime _lastFrameTime;

   

        public Game(int width, int height, string title) : base(GameWindowSettings.Default,
            new NativeWindowSettings() { ClientSize = (1920, 1080), Title = "hi", Profile = ContextProfile.Core })
        {
            ErrorChecker.InitializeGLDebugCallback(); 
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            Width = width;
            Height = height;
            par = new ColoredRectpParticles(new Vector2(1920/2, 1080/2), new Vector2(5,5), Color4.Green,100000,"par");
            par2 = new ColoredRectpParticles(new Vector2(1920/2, 1080/2), new Vector2(5,5), Color4.Red,100000,"par");
            par3 = new ColoredRectpParticles(new Vector2(1920/2, 1080/2), new Vector2(5,5), Color4.Blue,100000,"par");
           Main.addObject(par);
           Main.addObject(par2);
           Main.addObject(par3);
           this.Resize += e => Main.Resize(e.Width, e.Height);
            this.Resize += e => this.resize(e.Width, e.Height);
            this.KeyDown += e => Update(e);
        }

        void resize(int width, int height)
        {
            _controller.WindowResized(ClientSize.X, ClientSize.Y);
            this.Width = 1920;
            this.Height = 1080;
        }


        //void update();

        void Update(KeyboardKeyEventArgs e)
        {
           
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
          
            var elapsed = DateTime.Now - _lastFrameTime;
            var millisecondsPerFrame = 1000 / TargetFPS;
            if (elapsed.TotalMilliseconds < millisecondsPerFrame)
            {
                var delay = (int)(millisecondsPerFrame - elapsed.TotalMilliseconds);
                System.Threading.Thread.Sleep(delay);
                
            }
            par.Update();
            par2.Update();
            par3.Update();
            _lastFrameTime = DateTime.Now;
            if (MouseState.IsButtonDown(MouseButton.Right))
            {
               par.setwWind(new Vector2(MouseState.X, Height- MouseState.Y));
               
            }else
            {
                par.setwWind(par.drawInfo.Position);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            _controller.Update(this, (float)args.Time);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            Main.draw();
            
              DrawInfo.darwImguiDebugWindow();
              par.DrawImguiDebug();
                par2.DrawImguiDebug();
                par3.DrawImguiDebug();
          //   ImGui.ShowIDStackToolWindow();
              ImGuiController.CheckGLError("End of frame");
              
              _controller.Render();
            this.SwapBuffers();

        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            _controller.MouseScroll(e.Offset);
        }
    }
