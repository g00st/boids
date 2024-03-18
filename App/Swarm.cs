using System.Data;
using App.Engine;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace App;

public class Swarm: DrawObject
{
    private int _count;
    private ComputeShader _computeShader;
    public float alignmentWeight = 1.0f;
    public float cohesionWeight = 1.0f;
    public float separationWeight = 1.0f;
    public Vector4 Bounds;
    float[] positions;
    float offset = 0.0f;



    public Swarm(int count, Vector2 positon, Vector2 size, string name = "Swarm")
    {
        Bounds = new Vector4(-200, -200, 200, 200);
        _count = count;
        InstancedMesh mesh = new InstancedMesh(count);

        //rectangles vertex positions
        Bufferlayout vertexbufferlayout = new Bufferlayout();
        vertexbufferlayout.count = 2;
        vertexbufferlayout.normalized = false;
        vertexbufferlayout.offset = 0;
        vertexbufferlayout.type = VertexAttribType.Float;
        vertexbufferlayout.typesize = sizeof(float);
        mesh.AddAtribute(vertexbufferlayout, new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f });    
        
        //rectangles indices
        mesh.AddIndecies(new uint[] { 0, 1, 2, 2, 3, 0 });
        
        //positions an rotation of the rectangles
        Bufferlayout posslayout = new Bufferlayout();
        
        posslayout.count = 4;
        posslayout.normalized = false;
        posslayout.offset = 0;
        posslayout.type = VertexAttribType.Float;
        posslayout.typesize = sizeof(float);

        Random rand = new Random();
        
         positions = new float[count*4];

        int test = (int)Math.Sqrt(count);
        Console.WriteLine("test "+ test);
        for (int i = 0; i < count; i++)
        {
            positions[i*4] = 0 ;
            positions[i*4+1]= 0 ;
            positions[i*4+2] = MathF.PI*2/count * i;
            //Console.WriteLine("f "+ positions[i*3] + "  " + positions[i*3+1] + "  " + positions[i*3+2]);
        }

        mesh.AddInstancedAtribute(posslayout, positions, "positions");
        this.drawInfo = new DrawInfo(positon, size, 0, mesh, name);
        drawInfo.mesh.Texture = new Texture("Resources/swarm/spider.png");
        
        //shaders and uniforms int
        drawInfo.mesh.Shader = new Shader("Resources/swarm/swarm.vert", "Resources/swarm/swarm.frag");
        
        _computeShader = new ComputeShader("Resources/swarm/swarm.comp");
        Console.WriteLine("buffer 0 " + mesh.getBuffer(1));
        Console.WriteLine("buffer poss " + mesh.getInstancedAtribute("positions"));

        _computeShader.AddBuffer( (int)mesh.getInstancedAtribute("positions"),0);
        
        //velocety buffer
        int vels;
        GL.CreateBuffers( 1, out vels);
        float[] velos = new float[count*2];
        for (int i = 0; i < count; i++)
        {
            velos[i*2] = (float)rand.NextDouble()*2 -1;
            velos[i*2+1] = (float)rand.NextDouble()*2-1;
        }
        GL.NamedBufferData(vels, count* sizeof(float)*2,velos ,BufferUsageHint.DynamicDraw );
        _computeShader.AddBuffer(vels,1);
        
        _computeShader.setUniform1v("u_AlignmentWeight", alignmentWeight);
        _computeShader.setUniform1v("u_CohesionWeight", cohesionWeight);
        _computeShader.setUniform1v("u_SeparationWeight", separationWeight);
        _computeShader.setUniform4v("u_Bounds", Bounds.X, Bounds.Y, Bounds.Z, Bounds.W);
        
    }

    public DrawInfo drawInfo { get; }
    public void UpdateSwarm()
    {
        _computeShader.Bind();
        _computeShader.Dispatch(_count, 1, 1);
        _computeShader.wait();
      /*  Bufferlayout posslayout = new Bufferlayout();
        
        posslayout.count = 3;
        posslayout.normalized = false;
        posslayout.offset = 0;
        posslayout.type = VertexAttribType.Float;
        posslayout.typesize = sizeof(float);
        int test = (int)Math.Sqrt(_count);
        for (int i = 0; i < _count; i++)
        {
            positions[i*3] = i* 10 ;
            positions[i*3+1]= (float) (i % test) * 10+offset ;
            positions[i*3+2] = MathF.PI*2/_count * i ;
        }
        offset += 0.1f;
        ((InstancedMesh) this.drawInfo.mesh).ChangeInstancedAtributeData(posslayout,positions, "positions");
    */
    }
    
    
    public void ImguiControls()
    {
        ImGui.Begin("Swarm");
        ImGui.SliderFloat("Alignment Weight", ref alignmentWeight, 0.0f, 10.0f);
        ImGui.SliderFloat("Cohesion Weight", ref cohesionWeight, 0.0f, 10.0f);
        ImGui.SliderFloat("Separation Weight", ref separationWeight, 0.0f, 10.0f);
        System.Numerics.Vector4 bounds = new System.Numerics.Vector4(Bounds.X, Bounds.Y, Bounds.Z, Bounds.W);
        ImGui.SliderFloat4("Bounds", ref bounds, 0f, 1000.0f);
        Bounds = new Vector4(bounds.X, bounds.Y, bounds.Z, bounds.W);
        
        

            ImGui.End();
            
            //updat uniform if changed
            
            _computeShader.setUniform1v("u_AlignmentWeight", alignmentWeight);
            _computeShader.setUniform1v("u_CohesionWeight", cohesionWeight);
            _computeShader.setUniform1v("u_SeparationWeight", separationWeight);
            _computeShader.setUniform4v("u_Bounds", Bounds.X, Bounds.Y, Bounds.Z, Bounds.W);
    }
}