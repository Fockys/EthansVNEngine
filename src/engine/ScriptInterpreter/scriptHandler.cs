

using System.Diagnostics;
using System.Globalization;
using System.Text;

class ScriptHandler
{

    Stream currentScript = Stream.Null;
    int currentLine = 0;
    
    String[] scriptLines= [""];
    int scriptLinesAmount;
    DialogueManager dialogueManager;
    CharacterManager characterManager;

    string currentCharacter = "";

    public ScriptHandler(DialogueManager dialogueManagerParam, CharacterManager characterManagerParam)
    {
        dialogueManager = dialogueManagerParam;
        characterManager = characterManagerParam;
    }


    public bool loadScript(string Path)
    {
        if (!File.Exists(Path))
        {
            Console.Error.WriteLine($"file '{Path}' could not be found");
           return false; 
        } 

        scriptLines = File.ReadAllLines(Path);
        scriptLinesAmount = scriptLines.GetLength(0);
        return true;
    }    



    //goes to the next line in script
    public void scriptStep()
    {

        //ensures keepts in bounds
        if (scriptLinesAmount <= currentLine)
        {
            //end of script
            Console.WriteLine("end of script");
            
            return;
        }

        

        //check if line is a command
        if (scriptLines[currentLine].StartsWith("!"))
        {
            string[] tokens = scriptLines[currentLine].Split(" "); //split command into tokens

            tokens[0] = tokens[0].Substring(1); //remote leading !

            switch (tokens[0])
            {
                case "currentChar":
                {
                    currentCharacter = tokens[1];
                    characterManager.setFocusedCharacter(currentCharacter);
                    break;
                }
                case "loadChar":
                    {
                        characterManager.loadCharacter(tokens[1]);
                        break;
                    }
                case "charScale":
                    {
   
                        characterManager.setCharacterScale(currentCharacter,(float)Convert.ToDouble(tokens[1]));
                        characterManager.setFocusedCharacter(currentCharacter); //set focused again to recalc focused scale
                        break;
                    }
                case "changeSprite":
                    {
                        characterManager.setCharacterCurrentSprite(currentCharacter, tokens[1]);
                        break;
                    }
                    //change pos of currently selected character
                case "changePos":
                    {
                        characterManager.changeCharacterPosByCoords(currentCharacter,Convert.ToInt32(tokens[1]),Convert.ToInt32(tokens[2]));
                        break;
                    }
                default:
                    break;
            }

            //go to next line
            currentLine++;
            scriptStep(); 
            return;
        }

        dialogueManager.characterSay(currentCharacter,scriptLines[currentLine]);
        currentLine++;
    }



}