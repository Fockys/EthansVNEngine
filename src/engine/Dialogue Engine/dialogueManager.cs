




public class DialogueManager
{
    
    string currrentDialogue = "";
    string currentCharacter = "";
    TextRenderer textRenderer;
    Dictionary<string,bool> CharactersShown = new();

    public DialogueManager(TextRenderer textRendererParam)
    {
        textRenderer = textRendererParam;
    }


    //adds character dialogue box
    private void addCharacterDialogueBox(string name, string text)
    {
        CharactersShown[name] = true;
        textRenderer.addText(name,$"{name}: {text}",100,900);
        textRenderer.setFont(name,Path.Combine(AppContext.BaseDirectory,"arial.ttf"),60);
        textRenderer.updateTexture(name);
        currentCharacter = name;
    }

    void removeCharacterDialogueBox(string name)
    {
        CharactersShown.Remove(name);
        textRenderer.removeText(name);
    }

    private void setCharacterText(string name, string text)
    {
        textRenderer.replaceText(name,$"{name}: {text}");
        textRenderer.updateTexture(name);
    }

    void toggleCharacterDialogue(string name)
    {
        CharactersShown[name] = !CharactersShown[name];
    }

    void disableCharacterDialogue(string name)
    {   
        textRenderer.removeText(name);
        CharactersShown.Remove(name);
    }

    void enableCharacterDialogue(string name)
    {
        CharactersShown[name] = true;
    }

    

    public void characterSay(string characterName, string dialogue)
    {

        //first character adding
        if(currentCharacter == "")
        {
            addCharacterDialogueBox(characterName,dialogue);
        }

        //character not currently in isCharacterShown
        if (!CharactersShown.ContainsKey(characterName))
        {
            disableCharacterDialogue(currentCharacter);
            addCharacterDialogueBox(characterName,dialogue);
            return;
        }

        //update character that is in isCharacterShown
        setCharacterText(characterName,dialogue);
        
    }

}