

using System.Text;

class ScriptHandler()
{


    public bool loadScript(string Path)
    {
        if (!File.Exists(Path))
        {
            Console.Error.WriteLine($"file '{Path}' could not be found");
           return false; 
        } 

        using (FileStream fs = File.Open(Path, FileMode.Open, FileAccess.Read, FileShare.None))
        {
            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);
            
            while (fs.Read(b,0,b.Length) > 0)
            {
                Console.WriteLine(temp.GetString(b));
            }


        }
        


        return true;
    }    



}