

using System.Diagnostics;
using System.Text;

class ScriptHandler
{

    Stream currentScript = Stream.Null;
    int currentLine = 0;
    
    String[] scriptLines= [""];
    int scriptLinesAmount;
    DialogueManager dialogueManager;

    string currentCharacter = "";

    public ScriptHandler(DialogueManager dialogueManagerParam)
    {
        dialogueManager = dialogueManagerParam;
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

        string[] tokens = scriptLines[currentLine].Split(" ");

        //in the case of change character dialogue
        if (tokens[0].StartsWith("!"))
        {
            currentCharacter = tokens[0].Substring(1);
            currentLine++;
            scriptStep(); //go to next line
            return;
        }
        Console.Error.WriteLine($"{scriptLinesAmount}  {currentLine}");

        dialogueManager.characterSay(currentCharacter,scriptLines[currentLine]);
        currentLine++;
    }



}