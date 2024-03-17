using App.Engine;
using ImGuiNET;
using OpenTK.Mathematics;

namespace App.Engine;

public class DrawInfo
{
    static List<DrawInfo> _drawInfos = new List<DrawInfo>();
    public Vector2 Position;
    public Vector2 Size;
    public float Rotation;
    public Mesh mesh;
    public string Name;
    
    public DrawInfo(Vector2 position, Vector2 size, float rotation, Mesh mesh, string name )
    {
        this.Position = position;
        this.Size = size;
        this.Rotation = rotation;
        this.mesh = mesh;
        _drawInfos.Add(this);
        this.Name = name;
    }
    
    ~DrawInfo()
    {
        _drawInfos.Remove(this);
    }
    
   public static void darwImguiDebugWindow()
{
    ImGui.Begin("Debug Window");
    ImGui.BeginChild("scrolling", new System.Numerics.Vector2(0, 0), ImGuiChildFlags.Border, ImGuiWindowFlags.HorizontalScrollbar);

    foreach (var drawInfo in _drawInfos)
    {
        if (ImGui.CollapsingHeader(drawInfo.Name))
        {
            
            ImGui.PushID(drawInfo.GetHashCode());
            System.Numerics.Vector2 position = new System.Numerics.Vector2(drawInfo.Position.X, drawInfo.Position.Y);
            if (ImGui.SliderFloat2("Position", ref position, -1000.0f, 1000.0f))
            {
                drawInfo.Position = new Vector2(position.X, position.Y);
            }

            System.Numerics.Vector2 size = new System.Numerics.Vector2(drawInfo.Size.X, drawInfo.Size.Y);
            if (ImGui.SliderFloat2("Size", ref size, 0.1f, 1000.0f))
            {
                drawInfo.Size = new Vector2(size.X, size.Y);
            }

            float rotation = drawInfo.Rotation;
            if (ImGui.SliderFloat("Rotation", ref rotation, -360.0f, 360.0f))
            {
                drawInfo.Rotation = MathHelper.DegreesToRadians(rotation);
            }
            ImGui.PopID(); 
        }
    }

    ImGui.EndChild();
    ImGui.End();
}
    
    
    
    
    
}