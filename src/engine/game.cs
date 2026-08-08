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
    private TextRenderer textRenderer = null!;
    
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
        terminate();
        
    }


    private bool init()
    {
        
        if (!initSDL()) return false;
        if(!initTTF()) return false;
        
        if (!SDL.CreateWindowAndRenderer(EngineConfig.windowName, 1920, 1080, 0, out window, out renderer))
        {
            Console.WriteLine(SDL.GetError());
            return false;
        }

        spriteRenderer = new SpriteRenderer(renderer);
        inputHandler = new InputHandler();
        textRenderer = new TextRenderer(renderer);
        
        
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
            textRenderer.renderAllText();


            

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


        textRenderer.addText("text1","Test text",1000,300);
        textRenderer.setFont("text1",Path.Combine(AppContext.BaseDirectory,"arial.ttf"),52);
        textRenderer.replaceText("text1","replacement text");
        textRenderer.updateTexture("text1");

        textRenderer.addText("text2","hello :P",1000,900);
        textRenderer.setFont("text2",Path.Combine(AppContext.BaseDirectory,"arial.ttf"),80);
        textRenderer.updateTexture("text2");


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

    private bool initTTF()
    {
        if(!TTF.Init())
        {
            Console.WriteLine("TTF failed to initialise");
        }
        return true;
    }

    private void terminate(){
        terminateSDL();
        textRenderer.TextRendererTerminate();
    }

    private void terminateSDL()
    {
        SDL.DestroyRenderer(renderer);
        SDL.DestroyWindow(window);
        SDL.Quit();
    }



}