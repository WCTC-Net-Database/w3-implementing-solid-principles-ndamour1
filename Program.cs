namespace CharacterConsole;

class Program
{
    static void Main()
    {
        var input = new ConsoleInput();
        var output = new ConsoleOutput();

        CharacterManager manager = new CharacterManager(input, output);
        manager.Run();
    }
}

class ConsoleInput : IInput
{
    public string ReadLine()
    {
        return Console.ReadLine();
    }
}

class ConsoleOutput : IOutput
{
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public void Write(string message)
    {
        Console.Write(message);
    }
}
