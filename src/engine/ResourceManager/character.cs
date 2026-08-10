using textureManagerNamespace;

class Character
{

    public string characterName;
    SpriteRenderer spriteRenderer;
    Sprite currentSprite = null!;
    Dictionary<string,Sprite> characterSprites = new();
    public int characterX = 0;
    public int characterY = 0;
    public float baseScale = 1;
    public float spriteScale = 1;
    public float focusScale = 1;
    public int drawLayer = 5;



    public Character(SpriteRenderer spriteRendererParam, string characterNameParam){
        spriteRenderer = spriteRendererParam;
        characterName = characterNameParam;
    }

    public void render()
    {
        if (currentSprite == null)
        {
            Console.Error.WriteLine("currentSprite is null");
            return;
        }
            

        spriteRenderer.drawSpriteWithPos(currentSprite, characterX, characterY, scale:spriteScale);
    }

    

    public void setSprite(string spriteName)
    {
        if (!characterSprites.ContainsKey(spriteName)) return; //sprite doesnt exist
        currentSprite = characterSprites[spriteName];
        
    }

    public void addSprite(string spriteName, Sprite newSprite)
    {
        if(characterSprites.ContainsKey(spriteName)) return; //sprite already exists
        characterSprites[spriteName] = newSprite;
    }

    public void removeSprite(TextureManager textureManager, string spriteName)
    {
        if (!characterSprites.ContainsKey(spriteName)) return; //sprite doesnt exist
        characterSprites[characterName].terminate(textureManager);
        characterSprites.Remove(spriteName);
    }

    public void setPos(int xParam, int yParam)
    {
        characterX = xParam;
        characterY = yParam;
    }

    //scales the character but changes its coords so the center of the texture is in the same place
    public void scaleSpriteKeepCenter(float scale)
    {
        if (currentSprite == null) return;

        float previousScale = spriteScale;
        float oldWidth = currentSprite.width * previousScale;
        float oldHeight = currentSprite.height * previousScale;

        float centerX = characterX + oldWidth / 2f;
        float centerY = characterY + oldHeight / 2f;

        spriteScale = scale;

        float newWidth = currentSprite.width * spriteScale;
        float newHeight = currentSprite.height * spriteScale;

        characterX = (int)MathF.Round(centerX - newWidth / 2f);
        characterY = (int)MathF.Round(centerY - newHeight / 2f);
    }

}