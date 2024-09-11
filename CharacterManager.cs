namespace CharacterConsole;

public class CharacterManager
{
    private readonly IInput _input;
    private readonly IOutput _output;
    private readonly string _filePath = "input.csv";

    private string[] lines;

    public CharacterManager(IInput input, IOutput output)
    {
        _input = input;
        _output = output;
    }

    public void Run()
    {
        _output.WriteLine("Welcome to Character Management");

        lines = File.ReadAllLines(_filePath);

        while (true)
        {
            // Main menu
            _output.Write("\nSelect what you want to do.\n1. Display Characters\n2. Find Character\n3. Add Character\n4. Level Up Character\n5. Exit\n");

            if (!int.TryParse(_input.ReadLine(), out int choice))
            {
                _output.Write("Invalid input. Please enter a number. ");
                continue;
            }

            switch (choice)
            {
                case 1:
                    DisplayCharacters();
                    break;
                case 2:
                    FindCharacter();
                    break;
                case 3:
                    AddCharacter();
                    break;
                case 4:
                    LevelUpCharacter();
                    break;
                case 5:
                    _output.Write("See you again!");
                    return;
                default:
                    _output.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            // Pause to allow the user to see the result before the menu is shown again
            _output.Write("Press any key to continue...");
            _input.ReadLine();
        }
    }

    // Method to display characters
    public void DisplayCharacters()
    {
        CharacterReader characterList = new CharacterReader(_filePath);
        var characters = characterList.ReadCharactersFromFile();
        if (characters.Count == 0)
        {
            Console.WriteLine("No characters found.");
        }
        else
        {
            foreach (var character in characters)
            {
                Console.WriteLine(character);
            }
        }
    }

    // Method to find character
    public void FindCharacter()
    {
        CharacterReader findCharacters = new CharacterReader(_filePath);
        Console.Write("Enter the character's name: ");
        string name = Console.ReadLine();
        findCharacters.FindCharactersByName(name);
    }

    // Method to add characters
    public void AddCharacter()
    {
        // variable to break while loop below
        bool notZero = false;

        // Input for character's name
        Console.Write("Enter your character's first name: ");
        string name = _input.ReadLine();
        if (name.StartsWith("\"") && name.EndsWith("\""))
        {
            name = name.Substring(1, name.Length - 1);
        }

        // Input for character's class
        Console.Write("Enter your character's class: ");
        string characterClass = Console.ReadLine();

        // While loop to make sure level is greater than 0
        int level = 0;
        while (!notZero)
        {
            Console.Write("Enter your character's level. It must be 1 or higher. ");
            level = int.Parse(Console.ReadLine());

            if (level <= 0)
            {
                Console.Write("The number you entered is less than 1. Try again. ");
                level = int.Parse(Console.ReadLine());
            }
            else
            {
                notZero = true;
            }
        }

        // Calculation for hit points
        int hitPoints = level * 6;

        // Input for character's equipment
        Console.Write("Enter your character's equipment (separate items with a '|'): ");
        string[] equipment = _input.ReadLine().Split('|');

        // Displays the user's input for the character
        // CharacterReader call
        CharacterReader characterList = new CharacterReader(_filePath);
        var characters = characterList.ReadCharactersFromFile();
        characters.Add(new Character { name = name, characterClass = characterClass, level = level, hitPoints = hitPoints, equipment = equipment });
        
        // CharacterWriter call
        CharacterWriter newList = new CharacterWriter(_filePath);
        newList.WriteCharactersToFile(characters);
        Console.WriteLine($"Welcome, {name} the {characterClass}! You are level {level} with {hitPoints} HP and your equipment includes: {string.Join(", ", equipment)}.");
    }

    // Method for leveling up character
    public void LevelUpCharacter()
    {
        Console.Write("Enter the number indexed to the character you want to level up.\n");

        CharacterReader characterList = new CharacterReader(_filePath);
        var characters = characterList.ReadCharactersFromFile();
        for (int i = 0; i < characters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {characters[i].name} the {characters[i].characterClass}, Level {characters[i].level}");
        }
        int listNumber = int.Parse(Console.ReadLine()) - 1;
        Character chosen = characters[listNumber];
        int currLevel = chosen.level;
        int newLevel = 0;

        // Loop to make sure user inputs a number greater than chosen character's current level
        while (newLevel <= currLevel)
        {
            Console.Write($"You have chosen {chosen.name}.\nEnter your character's new level. It must be higher than their current level. ");
            newLevel = int.Parse(Console.ReadLine());

            if (newLevel > currLevel)
            {
                Console.Write($"{chosen.name} is now level {newLevel} with {newLevel * 6} HP.\n");
                characters[listNumber].level = newLevel;
                characters[listNumber].hitPoints = newLevel * 6;
                CharacterWriter newList = new CharacterWriter(_filePath);
                newList.WriteCharactersToFile(characters);
            }
            else if (newLevel < chosen.level)
            {
                Console.Write($"The number you typed is less than {currLevel}. Try again. ");
                newLevel = int.Parse(Console.ReadLine());
            }
            else if (newLevel == chosen.level)
            {
                Console.Write($"{newLevel} is {chosen.name}'s current level. Try again. ");
                newLevel = int.Parse(Console.ReadLine());
            }
        }
    }
}
