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
    private DialogueManager dialogueManager = null!;
    private CharacterManager characterManager = null!;
    
    private TextureManager textureManager = null!;
    
    private ScriptHandler scriptHandler = null!;
    private readonly Sprite testSprite = new(100,100);
    private readonly Sprite testBack = new(0,0);
    private Sprite testTextBox = new(0,800);


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
        
        if (!SDL.CreateWindowAndRenderer(EngineConfig.windowName, EngineConfig.windowWidth, EngineConfig.windowHeight, 0, out window, out renderer))
        {
            Console.WriteLine(SDL.GetError());
            return false;
        }

        
        spriteRenderer = new SpriteRenderer(renderer);
        textRenderer = new TextRenderer(renderer);
        dialogueManager = new DialogueManager(textRenderer);
        textureManager = new TextureManager(renderer);
        characterManager = new CharacterManager(spriteRenderer, textureManager);
        scriptHandler = new ScriptHandler(dialogueManager, characterManager);
        inputHandler = new InputHandler(scriptHandler);
        
        
        
        
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
            characterManager.renderCharacters();
            spriteRenderer.drawSprite(testTextBox);
            
            
            textRenderer.renderAllText();


            

            SDL.RenderPresent(renderer);
            SDL.Delay(16);
        }
    }

    void testing()
    {
        
        
        string imagePath = Path.Combine(EngineConfig.gameDataPath, "textures/pinkCry.png");
        textureManager.LoadPNG("testTexture", imagePath);
        imagePath = Path.Combine(EngineConfig.gameDataPath, "textures/background.jpg");
        textureManager.LoadJPG("testBackground",imagePath);
        
        testSprite.setTexture(textureManager, "testTexture");
        testBack.setTexture(textureManager, "testBackground");
        SDL.Color tempColor = new() {R=0,G=0,B=0,A=255};
        testTextBox.setRectangle(renderer,1920,280,tempColor);

/*
        characterManager.loadCharacter("characters/pink.json");
        characterManager.setCharacterScale("Pink",5);
        characterManager.setCharacterCurrentSprite("Pink","pinkCry");
*/

        scriptHandler.loadScript(Path.Combine(EngineConfig.gameDataPath,"scripts/testScript.txt"));


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