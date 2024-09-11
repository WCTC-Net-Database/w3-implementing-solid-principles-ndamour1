using CharacterConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CharacterConsole;

public class CharacterReader
{
    private string _filePath = "input.csv";

    public CharacterReader(string filePath)
    {
        _filePath = filePath;
    }

    // Method to read characters from file
    public List<Character> ReadCharactersFromFile()
    {
        var characters = new List<Character>();

        if (!File.Exists(_filePath))
        {
            Console.Write("File not found.");
            return characters;
        }

        var lines = File.ReadAllLines(_filePath);
        foreach (var line in lines.Skip(1)) // Skip header
        {
            var parts = ParseCsvLine(line);
            if (parts.Length == 5)
            {
                characters.Add(new Character
                {
                    name = parts[0],
                    characterClass = parts[1],
                    level = int.Parse(parts[2]),
                    hitPoints = int.Parse(parts[3]),
                    equipment = parts[4].Split('|')
                });
            }
        }
        return characters;
    }

    // Method to parse CSV lines
    public string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        bool inQuotes = false;
        var currentPart = new System.Text.StringBuilder();

        foreach (char c in line)
        {
            if (c == '\"')
            {
                inQuotes = !inQuotes; // Toggle whether we are inside quotes
            }
            else if (c == ',' && !inQuotes)
            {
                // If we hit a comma outside of quotes, it's a delimiter
                result.Add(currentPart.ToString().Trim());
                currentPart.Clear();
            }
            else
            {
                // Append to the current field (inside quotes or not)
                currentPart.Append(c);
            }
        }

        // Add the final part
        result.Add(currentPart.ToString().Trim());
        return result.ToArray();
    }

    // Method to find character by name using LINQ
    public Character FindCharactersByName(string name)
    {
        var characters = ReadCharactersFromFile();
        var chosen = characters.FirstOrDefault<Character>(c => c.name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (chosen != null)
        { 
            return chosen; 
        }
        else
        {
            Console.WriteLine("Character not found.\n");
            return null;
        }
    }
}