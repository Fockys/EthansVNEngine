
using SDL3;

namespace engineConfigNameSpace{
public static class EngineConfig
{
    //the location of the textures in the file system
    public static string texturesRootPath = "assets/";
    public static SDL.PixelFormat globalPixelFormat = SDL.PixelFormat.RGBA8888;
    public static string windowName = "Ethans VN Engine";
    public static int windowWidth = 1920;
    public static int windowHeight = 1080;
}
}