using engineConfigNameSpace;
using System.Runtime.InteropServices;
using SDL3;
using System.Data.SqlTypes;

namespace textureManagerNamespace{
public class TextureManager : IDisposable
{
    //a dictionary of all the textures currently loaded
    private readonly Dictionary<string, IntPtr> textures = new();

    nint renderer;

    public TextureManager(nint rendereParam)
        {
            renderer = rendereParam;
        }

    private static string ResolveAssetPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Texture path cannot be empty.", nameof(path));

        return Path.IsPathRooted(path)
            ? path
            : Path.Combine(EngineConfig.gameDataPath, path);
    }

    //loads a PNG texture, sets its name.
    public bool LoadPNG(string Name, string Path)
    {
        if (textures.ContainsKey(Name))
            return true; //texture already exists

        string resolvedPath = ResolveAssetPath(Path);

        nint io = SDL.IOFromFile(resolvedPath, "rb");
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
    public bool LoadJPG(string name, string path)
    {
        if (textures.ContainsKey(name))
            return true;

        string resolvedPath = ResolveAssetPath(path);

        nint io = SDL.IOFromFile(resolvedPath, "rb");
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