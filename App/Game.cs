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
    private Swarm swarm;


    private const int TargetFPS = 60; // Set your target FPS here
    private DateTime _lastFrameTime;


    public Game(int width, int height, string title) : base(GameWindowSettings.Default,
        new NativeWindowSettings() { ClientSize = (width, height), Title = "hi", Profile = ContextProfile.Core })
    {
        ErrorChecker.InitializeGLDebugCallback();
        _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
        Width = width;
        Height = height;
        swarm = new Swarm(1000, new Vector2(width / 2, height / 2), new Vector2(10, 10));
        Main.addObject(new ColoredRectangle(new Vector2(0), new Vector2(width, height), Color4.DarkBlue));
        Main.addObject(swarm);


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

        swarm.UpdateSwarm();


        var elapsed = DateTime.Now - _lastFrameTime;
        var millisecondsPerFrame = 1000 / TargetFPS;
        if (elapsed.TotalMilliseconds < millisecondsPerFrame)
        {
            var delay = (int)(millisecondsPerFrame - elapsed.TotalMilliseconds);
            System.Threading.Thread.Sleep(delay);
        }

        _lastFrameTime = DateTime.Now;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        _controller.Update(this, (float)args.Time);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
        Main.draw();

        DrawInfo.darwImguiDebugWindow();
        swarm.ImguiControls();
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
        Console.WriteLine(e.Offset);
        Main.vsize = Vector2.Clamp( new Vector2(Main.vsize.X + e.Offset.Y * 100, Main.vsize.Y + e.Offset.Y * 100), new Vector2(0.1f), new Vector2(2000));
        Console.WriteLine(Main.vsize);
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);

        if (MouseState.IsButtonDown(MouseButton.Right))
        {
            Vector2 normalizedDelta = new Vector2(e.Delta.X / Width, e.Delta.Y / Height);

            normalizedDelta.Y *= -1;
            Main.vpossition -= normalizedDelta * Main.vsize;
        }
        
          
        
    }
}