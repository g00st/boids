using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace App.Engine.Template;

public class TexturedRectangle : DrawObject
{
public DrawInfo drawInfo { get; }

    public TexturedRectangle(Vector2 positon, Vector2 size, Texture texture, string name = "TexturedRectangle")
    {
        this.drawInfo = new DrawInfo(new Vector2(), new Vector2(), 0, null, name);
        this.drawInfo.Position = positon;
        this.drawInfo.Size = size;
        this.drawInfo.Rotation = 0;
        

        Bufferlayout bufferlayout = new Bufferlayout();
        bufferlayout.count = 2;
        bufferlayout.normalized = false;
        bufferlayout.offset = 0;
        bufferlayout.type = VertexAttribType.Float;
        bufferlayout.typesize = sizeof(float);

        this.drawInfo.mesh = new Mesh();
        this.drawInfo.mesh.Texture = texture;
        this.drawInfo.mesh.AddAtribute(bufferlayout, new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f });
        this.drawInfo.mesh.AddAtribute(bufferlayout, new float[]  { 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f });
        this.drawInfo.mesh.AddIndecies(new uint[] { 0, 1, 2, 2, 3, 0 });
        this.drawInfo.mesh.Shader = new Shader("resources/Template/simple_texture.vert",
            "resources/Template/simple_texture.frag");
    }

    public TexturedRectangle(Texture texture) : this(new Vector2(0, 0), new Vector2(texture.Width, texture.Height), texture)
    {
        
    }
    
    public TexturedRectangle(Vector2 positon, Vector2 size, Texture texture, Shader shader) : this(positon, size, texture)
    {
        this.drawInfo.mesh.Shader = shader;
    }

}