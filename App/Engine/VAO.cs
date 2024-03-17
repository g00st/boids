using OpenTK.Graphics.OpenGL4;
using GL = OpenTK.Graphics.OpenGL4.GL;
using VertexAttribType = OpenTK.Graphics.OpenGL4.VertexAttribType;

namespace App.Engine;
//vertex array object = VAO
public class VAO
{
    private uint _handle;
    private uint counter = 0;
    private Dictionary<uint,uint> buffers = new Dictionary<uint, uint>();
    public VAO()
    {
        GL.CreateVertexArrays(1, out _handle);
        //handle= GL.GenVertexArray();
        ErrorChecker.CheckForGLErrors("A");
    }

    public uint   LinkAtribute(float[] bufferData,Bufferlayout layout,uint divisor = 0)
    {
        uint vbo  = 0;
        float[] a = bufferData.ToArray();
        GL.CreateBuffers(1, out  vbo);
        buffers.Add(counter,vbo);
        ErrorChecker.CheckForGLErrors("b1");
        GL.NamedBufferData(vbo, bufferData.Length*layout.typesize,bufferData ,BufferUsageHint.DynamicDraw );
        ErrorChecker.CheckForGLErrors("b2");
        GL.EnableVertexArrayAttrib(_handle, counter);
        ErrorChecker.CheckForGLErrors("b3");
        GL.VertexArrayAttribFormat(_handle, counter, layout.count,layout.type , layout.normalized, 0);
        ErrorChecker.CheckForGLErrors("b4");
        GL.VertexArrayVertexBuffer(_handle, counter, vbo, (IntPtr) layout.offset,layout.count*layout.typesize );
        ErrorChecker.CheckForGLErrors("b5");
        GL.VertexArrayAttribBinding(_handle,counter,counter);
        ErrorChecker.CheckForGLErrors("b6");
        GL.VertexArrayBindingDivisor(_handle, counter, divisor);
        ErrorChecker.CheckForGLErrors("b7");
        return counter++;

    }
    public void UpdateAttributeData(float[] bufferData, Bufferlayout layout, uint location)
    {
        if (location >= counter)
        {
            throw new ArgumentException("Location is not valid");
        }

        // Check if the buffer associated with the location exists
        if (!buffers.ContainsKey(location))
        {
            throw new ArgumentException("No buffer exists for the specified location");
        }

        uint vbo = buffers[location];

        // Update buffer data
        GL.NamedBufferSubData(vbo, (IntPtr)layout.offset, bufferData.Length * layout.typesize, bufferData);
    }

    public void LinkElements(uint[]bufferData)
    {
        uint vbe  = 0;
        ErrorChecker.CheckForGLErrors("chh");
        GL.CreateBuffers(1, out  vbe);
        Console.WriteLine(bufferData.Length);
        ErrorChecker.CheckForGLErrors("c0");
        GL.NamedBufferData(vbe, bufferData.Length*sizeof(uint),bufferData ,BufferUsageHint.StaticDraw );
        ErrorChecker.CheckForGLErrors("c1");
        GL.VertexArrayElementBuffer(_handle, vbe);
        ErrorChecker.CheckForGLErrors("c");
    }

    public void Bind()
    {
        GL.BindVertexArray(_handle);
        ErrorChecker.CheckForGLErrors("Bind VAO");
    }
    
    public uint GetBuffer(uint location)
    {
        return buffers[location];
    }
   
    
}


public struct Bufferlayout
{
    public int count;
    public VertexAttribType type;
    public int offset;
    public bool normalized;
    public int typesize;
}