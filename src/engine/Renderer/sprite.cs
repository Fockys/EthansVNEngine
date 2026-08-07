using SDL3;
using textureManagerNamespace;

//sprite object
public class Sprite
{
    IntPtr texture;

    public int x {get;set;}
    public int y {get;set;}
    public int height {get;private set;}
    public int width {get;private set;}
    public int layer {get;set;}


    public Sprite(int xParam, int yParam){
        x = xParam;
        y = yParam;
    }


    public void setTexture(TextureManager textureManager, string textureName)
    {
        texture = textureManager.Get(textureName);
        float w;
        float h;

        SDL.GetTextureSize(texture, out w, out h);
        width = (int)w;
        height = (int)h;

        SDL.SetTextureBlendMode(texture,SDL.BlendMode.Blend);

    }
    public nint GetTexture()
    {
        return texture;
    }


    
}