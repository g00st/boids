using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace App.Engine;

public class View
{
    private List<DrawObject> drawObjects;
    public int  Width, Height;
    public Vector2 vpossition;
    public Vector2 vsize;
    public float rotation;
    public VBO _rendertarget;
   
    
   
    

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
        GL.Viewport(0, 0, Width, Height);
        _rendertarget.Bind();
        Matrix4 camera =  calcCameraProjection();
        GL.Enable(EnableCap.Blend);

        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
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
            
            //Console.Write(objectransform.ToString() + "\n" +" \n");
            
               
            Matrix4 translateToOrigin = Matrix4.CreateTranslation(-vpossition.X, -vpossition.Y, 0);
            Matrix4 rotate = Matrix4.CreateRotationZ(rotation);
            Matrix4 translateBack = Matrix4.CreateTranslation(vpossition.X, vpossition.Y, 0);
            Matrix4 view = translateToOrigin * rotate * translateBack;

            Matrix4 projection = calcCameraProjection();
            
            
          Vector3 cameraRotationAxis = new Vector3(0, 0, 1);
          Matrix4 cameraRotationMatrix = Matrix4.CreateFromAxisAngle(cameraRotationAxis, MathHelper.DegreesToRadians(rotation));
          Matrix4 comb =   objectransform* Matrix4.CreateTranslation(-vpossition.X,-vpossition.Y,0) * cameraRotationMatrix *Matrix4.CreateTranslation(vpossition.X,vpossition.Y,0)  * camera;
            //prüfe was gamestate
          
            obj.mesh.Draw(comb, obj,view,projection);
            
        }
        
        
    }

    

    private Matrix4 calcCameraProjection()
    {

        //Matrix4.CreateOrthographic(1920, 1080, -1.0f, 1.0f)* Matrix4.CreateTranslation(-1,-1,0);
        
        float left = vpossition.X - vsize.X / 2.0f;
        float right = vpossition.X + vsize.X / 2.0f;
        float bottom = vpossition.Y -  ((vsize.X/Width)*Height)/ 2.0f;
        float top = vpossition.Y +  ((vsize.X/Width)*Height)/ 2.0f;
        return  Matrix4.CreateOrthographicOffCenter(left, right, bottom, top, -1.0f, 1.0f);

        
    }
    public View()
    {
        
        _rendertarget = VBO.VBO_0();
        vsize = new Vector2(1920 ,1080);
        vpossition = new Vector2(1920/2, 1080/2);
        drawObjects = new List<DrawObject>();
        rotation = 0;
    }
    

    
}