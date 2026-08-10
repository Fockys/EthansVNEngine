using SDL3;
using textureManagerNamespace;
using engineConfigNameSpace;

//sprite object
public class Sprite
{
    IntPtr texture;
    string textureName = null!;

    public int x {get;set;}
    public int y {get;set;}
    public int height {get;private set;}
    public int width {get;private set;}
    public int layer {get;set;}


    public Sprite(int xParam = 0, int yParam = 0){
        x = xParam;
        y = yParam;
    }


    public void setTexture(TextureManager textureManager, string textureNameParam)
    {
        textureName = textureNameParam;
        texture = textureManager.Get(textureName);
        float w;
        float h;

        SDL.GetTextureSize(texture, out w, out h);
        width = (int)w;
        height = (int)h;

        SDL.SetTextureBlendMode(texture,SDL.BlendMode.Blend);

    }

    public void setRectangle(nint renderer, int w, int h, SDL.Color colorParam)
    {
        nint surface = SDL.CreateSurface(w, h, EngineConfig.globalPixelFormat);
        if(surface == nint.Zero)
        {
            Console.Error.WriteLine($"CreateSurface failed: {SDL.GetError()}");
            return;
        }
        //make surface filled
        uint color = SDL.MapSurfaceRGBA(surface, colorParam.R,colorParam.B,colorParam.G,colorParam.A);
        SDL.FillSurfaceRect(surface,nint.Zero, color); // null rect fills whole surface
        
        //make texture from surface and set dimentions
        texture = SDL.CreateTextureFromSurface(renderer,surface);
        if(texture == IntPtr.Zero)
        {
            Console.Error.WriteLine("failed to make rectangle");
        }
        width = w;
        height = h;


        //cleanup
        SDL.DestroySurface(surface);

    }
    public nint GetTexture()
    {
        return texture;
    }


    public void terminate(TextureManager textureManager)
    {
        if(textureName == null) return; //texture doesnt exist
        textureManager.Unload(textureName);
    }



    
}