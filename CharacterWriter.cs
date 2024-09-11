using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterConsole;

public class CharacterWriter
{
    private string _filePath = "input.csv";

    public CharacterWriter(string filePath)
    {
        _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath)); // Ensure file path is not null
    }

    public void WriteCharactersToFile(List<Character> characters)
    {
        if (characters == null) throw new ArgumentNullException(nameof(characters)); // Ensure character list is not null

        var lines = new List<string> { "Name,Class,Level,HP,Equipment" }; // Fix header spacing
        lines.AddRange(characters.Select(c =>
        {
            // Handle names with commas by re-quoting them
            string formattedName = c.name.Contains(",") ? $"\"{c.name}\"" : c.name;

            // Join equipment with '|' and ensure it handles null value
            string formattedEquipment = string.Join("|", c.equipment ?? new string[0]);

            // Return properly formatted line
            return $"{formattedName},{c.characterClass},{c.level},{c.hitPoints},{formattedEquipment}";
        }));
        try
        {
            File.WriteAllLines(_filePath, lines);
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
        }
    }
}