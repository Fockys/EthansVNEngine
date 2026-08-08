using SDL3;

/*
textRenderer holds a dictionary of all currently show text textures
gameText holds one piece of text including its texture

to create a new text 
textRenderer.addText() then textRenderer.setFont() then textRenderer.updateFont();
this feels a bit complicated rn so will refine later

*/

class gameText
{
    nint font;
    public string text;
    public nint textTexture;
    float textureW;
    float textureH;
    int textX;
    int textY;

    public SDL.FRect dst;
    SDL.Color color = new() { R = 255, G = 255, B = 255, A = 255 };

    //constructor
    public gameText(string textParam, int xParam, int yParam) 
    {
        text = textParam;
        textX = xParam;
        textY = yParam;
    }

    //opens a font and sets current object font to it
    public bool openFont(string path, float ptSize)
    {
        if (font != nint.Zero)
        {
            TTF.CloseFont(font);
            font = nint.Zero;
        }

        font = TTF.OpenFont(path, ptSize);
        if(font == nint.Zero)
        {
            Console.Error.WriteLine($"Font '{path}' failed to open: {SDL.GetError()}");
            return false;
        }
        return true;
    }

    //when the gameText is changed in anyway the texture should be updated
    public unsafe void updateTexture(nint renderer)
    {
        

        nint textSurface = TTF.RenderTextSolid(font, text,0, color);
        if (textSurface == nint.Zero)
        {
            Console.Error.WriteLine($"textSurface failed: {SDL.GetError()}");
            return;
        }

        textTexture = SDL.CreateTextureFromSurface(renderer, textSurface);
        if (textTexture == nint.Zero)
        {
            SDL.DestroySurface(textSurface);
            Console.Error.WriteLine($"CreateTextureFromSurface failed: {SDL.GetError()}");
            return;
        }

        SDL.GetTextureSize(textTexture, out textureW, out textureH);
        calculateFrect();

        //cleanup
        SDL.DestroySurface(textSurface);
    }

    //calculates the location and size of the rect to be rendered
    void calculateFrect()
    {
        dst = new() {X=textX,Y=textY,W=textureW,H=textureH};
    }


}

public class TextRenderer
{
    nint font;
    nint renderer;
    Dictionary<string,gameText> gameTexts = new();

    public TextRenderer(nint rendererParam)
    {
        renderer = rendererParam;
    }

    //add a new gameText class to the dictionary of current
    public void addText(string name, string text, int textX, int textY)
    {
        if (gameTexts.ContainsKey(name)) return;

        gameTexts[name] = new gameText(text, textX, textY);
    }

    //set the font of one gameText object
    public void setFont(string textName, string fontpath, int fontSize){
        if(!checkTextExists(textName)) return;
        gameTexts[textName].openFont(fontpath,fontSize);
    }

    //update the texture of one gameText object
    public void updateTexture(string textName){
        if(!checkTextExists(textName)) return;
        gameTexts[textName].updateTexture(renderer);
    }

    //remove a new gameText object from the dictionary of current texts
    public void removeText(string name){
        if (!gameTexts.ContainsKey(name)) return;
        gameTexts.Remove(name);
    }

    //check if the textName is in dictionary
    private bool checkTextExists(string textName){
        if(!gameTexts.ContainsKey(textName)){
            Console.Error.WriteLine($"textName {textName} does not exist");
            return false;
        }
        return true;

    }

    //set a new text on a gameText object
    public void replaceText(string textName, string newText){
        if(!checkTextExists(textName)) return;
        gameTexts[textName].text = newText;
    }

    unsafe public void renderAllText()
    {
        foreach(var text in gameTexts){
            gameText t = text.Value;
            fixed(SDL.FRect* dstPtr = &t.dst)
            {
                SDL.RenderTexture(renderer,text.Value.textTexture, nint.Zero, (nint)dstPtr);
            }
           
        }
    }

    
    public void TextRendererTerminate()
    {
        if (font != nint.Zero)
        {
            TTF.CloseFont(font);
            font = nint.Zero;
        }

        TTF.Quit();
    }
}