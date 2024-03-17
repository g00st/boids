using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace App.Engine;

struct UniformData
{
    public ActiveUniformType Type;
     public int Location;
}

public class Shader
{
    private int vertexHandle;
    private int fragmentHandle;
    private int _Handle;
    private Dictionary<string, UniformData> uniformLocations;
    public Shader(string vertex, string fragment)
    {
        uniformLocations = new Dictionary<string, UniformData>();
        string vertexData = Loader.LoadVertexShader(vertex);
        string fragementData = Loader.LoadFragmentShader(fragment);
        
         vertexHandle= GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexHandle, vertexData);
        GL.CompileShader(vertexHandle);
        
        fragmentHandle=  GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentHandle, fragementData);
        GL.CompileShader(fragmentHandle);
        

        int res =0;
        GL.GetShader(vertexHandle,ShaderParameter.CompileStatus, out  res);
        if (1 == res) { Console.WriteLine("vertex shader compiled: "+ vertex); }
        else
        {
            Console.WriteLine( " shader compilation error: "+ vertex + "--------------------------------------\n"+  GL.GetShaderInfoLog(vertexHandle) + "\n--------------------------------------");
            throw new  Exception("shader compilation error");
        }
        GL.GetShader(fragmentHandle,ShaderParameter.CompileStatus, out  res);
        if (1 == res) { Console.WriteLine("frag shader compiled: " +fragment); }
        else
        {
            Console.WriteLine(" shader compilation error: "+fragment  + "--------------------------------------\n"+ GL.GetShaderInfoLog(fragmentHandle)+ "\n--------------------------------------");
            throw new  Exception("shader compilation error");
        }

        _Handle = GL.CreateProgram();
        GL.AttachShader(_Handle,vertexHandle);
        GL.AttachShader(_Handle,fragmentHandle);
        GL.LinkProgram(_Handle);
        GL.ValidateProgram(_Handle);
        GenerateUniforms();
    }

    private void GenerateUniforms()
    {
        // Get the number of active uniforms in the shader program
        GL.GetProgram(_Handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

        // Query and store uniform information
        for (int i = 0; i < uniformCount; i++)
        {
            string uniformName = GL.GetActiveUniform(_Handle, i, out _, out ActiveUniformType uniformType);
            Console.WriteLine(uniformName +"  "+ uniformType);
            
            int location = GL.GetUniformLocation(_Handle, uniformName);
            UniformData t;
            t.Type = uniformType;
            t.Location = location;
            uniformLocations.Add(uniformName, t);

            // You can store the uniform type if needed
            // For example: uniformTypes.Add(uniformName, uniformType);
        }
    }
    public void Bind(){ GL.UseProgram(_Handle);}
    public void Unbind (){GL.UseProgram(0);}

    
   
    public void setUniform1i(string name,int v1)
    {
        GL.Uniform1( GL.GetUniformLocation(_Handle, name),v1);
    }
    public void setUniform1v(string name,float v1)
    {
        GL.Uniform1( GL.GetUniformLocation(_Handle, name),v1);
    }

    public void setUniformV2f(string name, Vector2 v2)
    {
        GL.Uniform2( GL.GetUniformLocation(_Handle, name),v2);
    }   
    
    public void setUniformM4(string name,Matrix4 v1)
    {
      
        GL.UniformMatrix4( GL.GetUniformLocation(_Handle, name),false,ref v1);
    }   
    
    public void setUniform4v(string name,float v1,float v2,float v3,float v4)
    {
        GL.Uniform4( GL.GetUniformLocation(_Handle, name),v1,v2,v3,v4);
    }   
    
}