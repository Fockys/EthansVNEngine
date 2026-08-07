

using SDL3;

class InputHandler()
{


    public int handleInput(SDL.Event e)
    {
        
        switch (e.Key.Key)
        {
            case SDL.Keycode.Escape:
                return 0;


            default:
                break;
        }
        return 1;
    }
}