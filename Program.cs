using System;
using System.IO;
using System.Security.Cryptography;

class Program
{
    static string[] lines;

    static void Main()
    {
        string filePath = "input.csv";
        lines = File.ReadAllLines(filePath);

        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display Characters");
            Console.WriteLine("2. Add Character");
            Console.WriteLine("3. Level Up Character");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayAllCharacters(lines);
                    break;
                case "2":
                    AddCharacter(ref lines, filePath);
                    lines = File.ReadAllLines(filePath);  //had help from copilot. unable to figure out why new charcters did not sisplay without this line
                    break;
                case "3":
                    LevelUpCharacter(lines, filePath); 
                    lines = File.ReadAllLines(filePath);
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void DisplayAllCharacters(string[] lines)
    {
        // Skip the header row
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string name;
            string characterClass;
            int level;
            int hitPoints;
            string[] equipment;

            int startIdx;

            // Check if the name is quoted
            if (line.StartsWith("\""))
            {
                int endQuoteIdx = line.IndexOf('"', 1);
                name = line.Substring(1, endQuoteIdx - 1);
                // The next comma after the closing quote
                startIdx = endQuoteIdx + 2; // skip quote and comma
            }
            else
            {
                int commaIdx = line.IndexOf(',');
                name = line.Substring(0, commaIdx);
                startIdx = commaIdx + 1;
            }

            // Now parse the rest of the fields
            string rest = line.Substring(startIdx);
            string[] fields = rest.Split(',');

            characterClass = fields[0];
            level = int.Parse(fields[1]);
            hitPoints = int.Parse(fields[2]);
            equipment = fields[3].Split('|');

            // Display character information
            Console.WriteLine($"Name: {name}, Class: {characterClass}, Level: {level}, HP: {hitPoints}, Equipment: {string.Join(", ", equipment)}");
        }
    }

    static void AddCharacter(ref string[] lines, string filePath)
    {
        // TODO: Implement logic to add a new character

        // Prompt for character details (name, class, level, hit points, equipment)
        Console.WriteLine("Enter character name: ");
        string name = Console.ReadLine();
        Console.WriteLine("Enter class:");
        string characterClass = Console.ReadLine();
        Console.WriteLine("Enter level:");
        int level = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter hit points:");
        int hitPoints = int.Parse(Console.ReadLine());
        Console.WriteLine("Enter equipment items separated by commas:");
        string[] equipmentArray = Console.ReadLine().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        string equipmentInput = string.Join("|", equipmentArray);

        // DO NOT just ask the user to enter a new line of CSV data or enter the pipe-separated equipment string
        string csvName = name.Contains(",") ? $"\"{name}\"" : name; // Handle names with commas
       
        // Append the new character to the lines array
        string newLine = $"{csvName},{characterClass},{level},{hitPoints},{equipmentInput}";
        try
        {
            // Print the full file path for debugging
            Console.WriteLine($"Writing to: {Path.GetFullPath(filePath)}");

            // Append the new character to the file
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(newLine);
            }

            // Refresh lines array
            lines = File.ReadAllLines(filePath);

            Console.WriteLine("Character added successfully!");
            // Optionally, display all characters immediately:
            DisplayAllCharacters(lines);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }

    }
    static void LevelUpCharacter(string[] lines, string filePath)
    {
        Console.Write("Enter the name of the character to level up: ");
        string nameToLevelUp = Console.ReadLine();

        bool found = false;

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            string name;
            int startIdx;

            if (line.StartsWith("\""))
            {
                int endQuoteIdx = line.IndexOf('"', 1);
                name = line.Substring(1, endQuoteIdx - 1);
                startIdx = endQuoteIdx + 2;
            }
            else
            {
                int commaIdx = line.IndexOf(',');
                name = line.Substring(0, commaIdx);
                startIdx = commaIdx + 1;
            }

            if (name.Equals(nameToLevelUp, StringComparison.OrdinalIgnoreCase))
            {
                string rest = line.Substring(startIdx);
                string[] fields = rest.Split(',');

                string characterClass = fields[0];
                int level = int.Parse(fields[1]);
                int hitPoints = int.Parse(fields[2]);
                string equipment = fields[3];

                level++;
                Console.WriteLine($"Character {name} leveled up to level {level}!");

                string csvName = line.StartsWith("\"") ? $"\"{name}\"" : name;
                string newLine = $"{csvName},{characterClass},{level},{hitPoints},{equipment}";
                lines[i] = newLine;
                found = true;
                break;
            }
        }

        if (found)
        {
            File.WriteAllLines(filePath, lines);
        }
        else
        {
            Console.WriteLine("Character not found.");
        }
    }




}