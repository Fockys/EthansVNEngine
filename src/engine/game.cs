using System.IO;
using engineConfigNameSpace;
using SDL3;
using textureManagerNamespace;

class Game
{
    private bool gameRunning;
    private nint renderer;
    private nint window;
    private SpriteRenderer spriteRenderer = null!;
    private InputHandler inputHandler = null!;
    
    private readonly TextureManager textureManager = new();
    private ScriptHandler scriptHandler = new();
    private readonly Sprite testSprite = new(100,100);
    private readonly Sprite testBack = new(0,0);


    public void Run()
    {
        if (!init())
        {
            Console.Error.WriteLine("Engine init failed");
            return;
        }
        testing();
        mainLoop();
        terminateSDL(renderer, window);
    }


    private bool init()
    {
        
        if (!initSDL()) return false;
        
        if (!SDL.CreateWindowAndRenderer(EngineConfig.windowName, 1920, 1080, 0, out window, out renderer))
        {
            Console.WriteLine(SDL.GetError());
            return false;
        }

        spriteRenderer = new SpriteRenderer(renderer);
        inputHandler = new InputHandler();
        
        SDL.ShowWindow(window);
        gameRunning = true;
        return true;


    }

    void mainLoop()
    {
        while (gameRunning)
        {
            while (SDL.PollEvent(out var e))
            {
                if ((SDL.EventType)e.Type == SDL.EventType.Quit) gameRunning = false;
                else if((SDL.EventType)e.Type == SDL.EventType.KeyUp)
                {
                    if(inputHandler.handleInput(e) == 0) gameRunning = false;
                }
            }

            

            spriteRenderer.drawSprite(testBack);
            spriteRenderer.drawSprite(testSprite,5);

            SDL.RenderPresent(renderer);
            SDL.Delay(16);
        }
    }

    void testing()
    {
        
        string imagePath = Path.Combine(AppContext.BaseDirectory, "test.png");
        textureManager.LoadPNG("testTexture", imagePath, renderer);
        imagePath = Path.Combine(AppContext.BaseDirectory, "background.jpg");
        textureManager.LoadJPG("testBackground",imagePath,renderer);
        
        testSprite.setTexture(textureManager, "testTexture");
        testBack.setTexture(textureManager, "testBackground");

        scriptHandler.loadScript(Path.Combine(AppContext.BaseDirectory,"test.txt"));
        
    }

   

    private bool initSDL()
    {
        if (!SDL.Init(SDL.InitFlags.Video))
        {
            Console.WriteLine(SDL.GetError());
            return false;
        }

        return true;
    }

    private void terminateSDL(nint renderer, nint window)
    {
        SDL.DestroyRenderer(renderer);
        SDL.DestroyWindow(window);
        SDL.Quit();
    }
}