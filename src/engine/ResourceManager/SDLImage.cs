using System.Runtime.InteropServices;


//SDL image wrapper
public static class SDLIMAGE
    {
    [DllImport("SDL3_image", CallingConvention = CallingConvention.Cdecl)]
    public static extern nint IMG_LoadTexture(
        nint renderer,
        [MarshalAs(UnmanagedType.LPUTF8Str)] string file);
    }