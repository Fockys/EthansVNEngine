


class Character
{

    public string characterName;
    SpriteRenderer spriteRenderer;
    Sprite currentSprite = null!;
    Dictionary<string,Sprite> characterSprites = new();
    public int characterX = 0;
    public int characterY = 0;
    public float spriteScale;

    public int drawLayer = 5;



    public Character(SpriteRenderer spriteRendererParam, string characterNameParam){
        spriteRenderer = spriteRendererParam;
        characterName = characterNameParam;
    }

    public void render()
    {
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

    void removeSprite(string spriteName)
    {
        if (!characterSprites.ContainsKey(spriteName)) return; //sprite doesnt exist
        characterSprites.Remove(spriteName);
    }

}