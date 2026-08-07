using engineConfigNameSpace;
using System.Runtime.InteropServices;
using SDL3;
using System.Data.SqlTypes;

namespace textureManagerNamespace{
public class TextureManager : IDisposable
{
    //a dictionary of all the textures currently loaded
    private readonly Dictionary<string, IntPtr> textures = new();

    //loads a PNG texture, sets its name.
    public bool LoadPNG(string Name, string Path, nint renderer)
    {
        if (textures.ContainsKey(Name))
            return true; //texture already exists

        nint io = SDL.IOFromFile(Path, "rb");
        if (io == nint.Zero)
            return false;

        nint surface = SDL3.Image.LoadPNGIO(io); // or Image.LoadPNGIO(io), depending on your binding
        SDL.CloseIO(io);

        if (surface == nint.Zero)
            return false;

        nint texture = SDL.CreateTextureFromSurface(renderer, surface);
        SDL.DestroySurface(surface);

        if (texture == nint.Zero)
            return false;

        textures[Name] = texture;
        return true;
    }

    //loads a JPEG texture, sets its name.
    public bool LoadJPG(string name, string path, nint renderer)
    {
        if (textures.ContainsKey(name))
            return true;

        nint io = SDL.IOFromFile(path, "rb");
        if (io == nint.Zero)
            return false;

        nint surface = SDL3.Image.LoadJPGIO(io);
        SDL.CloseIO(io);

        if (surface == nint.Zero)
            return false;

        nint texture = SDL.CreateTextureFromSurface(renderer, surface);
        SDL.DestroySurface(surface);

        if (texture == nint.Zero)
            return false;

        textures[name] = texture;
        return true;
    }

    //gets a texture by name
    public nint Get(string Name)
    {
        if(!textures.TryGetValue(Name,out var texture)) throw new KeyNotFoundException($"Texture '{Name}' isn't loaded");
        return texture;
    }
    
    //unloads a texture by name
    public void Unload(string Name)
    {
        if(textures.TryGetValue(Name, out var texture)){
            SDL.DestroyTexture(texture);
            textures.Remove(Name);
        }
    }

    //dispose of class
    public void Dispose()
    {
        foreach(var texture in textures.Values)
        {
            SDL.DestroyTexture(texture);
        }

        textures.Clear();
    }

}
}