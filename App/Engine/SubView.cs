using App.Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace App.Engine;

public class SubView
{



    private List<DrawObject> drawObjects;
    public int  Width, Height;
    public Vector2 vpossition;
    public Vector2 vsize;
    public float rotation;
    public VBO _rendertarget;
    public Color4 ClearColor;
   
    
   
    

    public void Resize(int width, int height )
    {
        Width = width;
        Height = height;
       
        GL.Viewport(0, 0, Width, Height);
    }
    
    public void addObject(DrawObject obj){
        drawObjects.Add(obj);
    }
    public void draw()
    {
        _rendertarget.Bind();
        Matrix4 camera =  calcCameraProjection();
        GL.ClearColor(ClearColor);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        
        //statt liste an drawobjects dann eine liste an renderables
        foreach (var drawObject in drawObjects)
        {
            DrawInfo obj = drawObject.drawInfo;
       
            Matrix4 ObjectScalematrix = Matrix4.CreateScale(obj.Size.X,obj.Size.Y, 1.0f);
            Matrix4 ObjectRotaionmatrix = Matrix4.CreateRotationZ(obj.Rotation);
            Matrix4 ObjectTranslationmatrix = Matrix4.CreateTranslation(obj.Position.X,obj.Position.Y,0);

            Matrix4 objectransform = Matrix4.Identity * ObjectScalematrix;
            objectransform *= ObjectRotaionmatrix;
            objectransform *= ObjectTranslationmatrix;
            
            
          Vector3 cameraRotationAxis = new Vector3(0, 0, 1);
          Matrix4 cameraRotationMatrix = Matrix4.CreateFromAxisAngle(cameraRotationAxis, MathHelper.DegreesToRadians(rotation));
          Matrix4 comb =   (objectransform* Matrix4.CreateTranslation(-vpossition.X,-vpossition.Y,0) * cameraRotationMatrix *Matrix4.CreateTranslation(vpossition.X,vpossition.Y,0) )*camera  ;
            //prüfe was gamestate
            
            
                
            Matrix4 translateToOrigin = Matrix4.CreateTranslation(-vpossition.X, -vpossition.Y, 0);
            Matrix4 rotate = Matrix4.CreateRotationZ(rotation);
            Matrix4 translateBack = Matrix4.CreateTranslation(vpossition.X, vpossition.Y, 0);
            Matrix4 view = translateToOrigin * rotate * translateBack;

            Matrix4 projection = calcCameraProjection();

            obj.mesh.Draw(comb, obj,view,projection);
        }
        
        
    }

    

    private Matrix4 calcCameraProjection()
    {

     
        float left = vpossition.X - vsize.X / 2.0f;
        float right = vpossition.X + vsize.X / 2.0f;
        float bottom = vpossition.Y - vsize.Y/ 2.0f;
        float top = vpossition.Y + vsize.Y / 2.0f;
        return  Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1.0f, 1.0f);

        
    }
    public SubView( VBO rendertarget)
    {
        ClearColor = Color4.Black;
        _rendertarget = rendertarget;
        vsize = new Vector2(rendertarget.Widht(), rendertarget.Height());
        Width = rendertarget.Widht();
        Height = rendertarget.Height();
        vpossition = new Vector2(rendertarget.Widht()/2, rendertarget.Height()/2);
        drawObjects = new List<DrawObject>();
        rotation = 0;
    }
    
    //remove object?
    
}
