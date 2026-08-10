using System.Net.ServerSentEvents;
using System.Text.Json;
using System.Text.Json.Serialization;
using engineConfigNameSpace;
using textureManagerNamespace;


public class CharacterData
{
    public required string characterName { get; set; }
    public required Dictionary<string,string> characterSprites {get;set;}
    public required string currentSprite { get; set; }
}


class CharacterManager
{
    
    Dictionary<string,Character> loadedCharacters = new();
    SpriteRenderer spriteRenderer;
    TextureManager textureManager;
    string focusedCharacter = "";

    public CharacterManager(SpriteRenderer spritRendererParam, TextureManager textureManagerParam)
    {
        spriteRenderer = spritRendererParam;
        textureManager = textureManagerParam;
    }

    public void loadCharacter(string characterDataPath)
    {
        string json;
        using (StreamReader r = new StreamReader(Path.Combine(EngineConfig.gameDataPath, characterDataPath))) json = r.ReadToEnd();
        CharacterData characterData = JsonSerializer.Deserialize<CharacterData>(json)!;
        if (loadedCharacters.ContainsKey(characterData.characterName)) return; //character already exists in loaded

        Character character = new Character(spriteRenderer, characterData.characterName);

        foreach (KeyValuePair<string, string> sprite in characterData.characterSprites)
        {
            if (!textureManager.LoadPNG(sprite.Key, sprite.Value))
            {
                Console.Error.WriteLine($"Failed to load sprite '{sprite.Key}' from '{sprite.Value}'");
                continue;
            }

            Sprite temp = new Sprite();
            temp.setTexture(textureManager, sprite.Key);
            character.addSprite(sprite.Key, temp);
        }

        if (!string.IsNullOrWhiteSpace(characterData.currentSprite)) character.setSprite(characterData.currentSprite);
        loadedCharacters[characterData.characterName] = character;
    }

    public void renderCharacters(){
        foreach(KeyValuePair<string,Character> character in loadedCharacters){
            character.Value.render();
        }
    }

    public void setCharacterScale(string characterName, float characterScale)
    {
        if(!checkCharacterExists(characterName)) return;

        Character character = loadedCharacters[characterName];
        character.baseScale = characterScale;

        float targetScale = characterName == focusedCharacter
            ? characterScale * EngineConfig.focusedCharacterScale
            : characterScale;

        character.scaleSpriteKeepCenter(targetScale);
    }

    public void setCharacterCurrentSprite(string characterName, string spriteName)
    {
        if(!checkCharacterExists(characterName)) return;
        loadedCharacters[characterName].setSprite(spriteName);
    }

    public void removeCharacterSprite(string characterName, string spriteName){
        if(!checkCharacterExists(characterName)) return;
        loadedCharacters[characterName].removeSprite(textureManager,spriteName);
    }

    public void changeCharacterPosByCoords(string characterName, int x, int y)
    {
        if(!checkCharacterExists(characterName)) return;
        loadedCharacters[characterName].setPos(x,y);
    }

    public void setFocusedCharacter(string characterName)
    {
        if(!checkCharacterExists(characterName)) return;

        if (!string.IsNullOrEmpty(focusedCharacter) && checkCharacterExists(focusedCharacter))
        {
            loadedCharacters[focusedCharacter].scaleSpriteKeepCenter(loadedCharacters[focusedCharacter].baseScale);
        }

        focusedCharacter = characterName;

        float focusedScale = loadedCharacters[characterName].baseScale * EngineConfig.focusedCharacterScale;
        loadedCharacters[characterName].focusScale = focusedScale;
        loadedCharacters[characterName].scaleSpriteKeepCenter(focusedScale);
    }

    public void debugPrintCharScales(string characterName){

        if(!checkCharacterExists(characterName)){
            Console.Error.WriteLine($"character {characterName} does not exist");
            return;
        }

        Console.Out.WriteLine($"{loadedCharacters[characterName].spriteScale} | {loadedCharacters[characterName].baseScale} | {loadedCharacters[characterName].focusScale}");
        return;
    }

    private bool checkCharacterExists(string characterName){

        if (!loadedCharacters.ContainsKey(characterName)) //character doesnt exist
        {
            Console.Error.WriteLine($"character {characterName} does not exist"); 
            return false; 
        }

        return true;
    }
    

}