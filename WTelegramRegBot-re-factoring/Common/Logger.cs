namespace Common;

public class Logger
{
    public void Info(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
    public void Error(string msg, Exception? ex = null)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(msg);
        if (ex != null)
            Console.WriteLine(ex.GetBaseException().Message);
        Console.ResetColor();
    }
}
