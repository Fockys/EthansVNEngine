

using SDL3;

class InputHandler
{

    ScriptHandler scriptHandler;

    public InputHandler(ScriptHandler scriptHandlerParam)
    {
        scriptHandler = scriptHandlerParam;
    }


    public int handleInput(SDL.Event e)
    {
        
        switch (e.Key.Key)
        {
            case SDL.Keycode.Escape:
                return 0;


            case SDL.Keycode.Space:
                scriptHandler.scriptStep();
                break;

            default:
                break;
        }
        return 1;
    }
}