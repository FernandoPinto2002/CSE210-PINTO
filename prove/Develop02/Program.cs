using System;
using System.Collections.Generic;
using System.IO;

public class Entry
{
    public DateTime Date { get; } 
    public string Prompt { get; } 
    public string Response { get; } 

    public Entry(string prompt, string response)
    {
        Date = DateTime.Now;
        Prompt = prompt;
        Response = response;
    }

    public Entry(string prompt, string response, DateTime date)
    {
        Date = date;
        Prompt = prompt;
        Response = response;
    }

    public string GetFormattedEntry()
    {
        return $"Date: {Date.ToString("g")}\nPrompt: {Prompt}\nResponse: {Response}";
    }
}

public class Journal
{
    private List<Entry> _entries = new List<Entry>(); // Stores journal entries

   
    public void AddEntry(string prompt, string response)
    {
        Entry newEntry = new Entry(prompt, response);
        _entries.Add(newEntry);
    }

    public void DisplayJournal()
    {
        if (_entries.Count == 0)
        {
            Console.WriteLine("No entries in the journal.");
        }
        else
        {
            Console.WriteLine("\nJournal Entries:");
            foreach (Entry entry in _entries)
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine(entry.GetFormattedEntry());
                Console.WriteLine("----------------------------\n");
            }
        }
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (Entry entry in _entries)
            {
                writer.WriteLine($"{entry.Date}|{entry.Prompt}|{entry.Response}");
            }
        }
        Console.WriteLine("Journal saved to file.");
    }

    public void LoadFromFile(string filename)
    {
        if (File.Exists(filename))
        {
            _entries.Clear();
            string[] lines = File.ReadAllLines(filename);

            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                DateTime date = DateTime.Parse(parts[0]);
                string prompt = parts[1];
                string response = parts[2];
                _entries.Add(new Entry(prompt, response, date));
            }
            Console.WriteLine("Journal loaded from file.");
        }
        else
        {
            Console.WriteLine("File not found.");
        }
    }
}

public class Program
{
    static Journal myJournal = new Journal();
    static List<string> prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    static void Main(string[] args)
    {
        string userInput;
        do
        {
            DisplayMenu(); 
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    WriteNewEntry();
                    break;
                case "2":
                    myJournal.DisplayJournal();
                    break;
                case "3":
                    SaveJournal();
                    break;
                case "4":
                    LoadJournal();
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        } while (userInput != "5");
    }
    static void DisplayMenu()
    {
        Console.WriteLine("Please choose one of the following options:");
        Console.WriteLine("1. Write a new journal entry");
        Console.WriteLine("2. Display journal entries");
        Console.WriteLine("3. Save the journal to a file");
        Console.WriteLine("4. Load the journal from a file");
        Console.WriteLine("5. Quit");
        Console.Write("Your choice: ");
    }

    static void WriteNewEntry()
    {
        Random random = new Random();
        int index = random.Next(prompts.Count);
        string prompt = prompts[index];
        Console.WriteLine(prompt);

        Console.Write("Your response: ");
        string response = Console.ReadLine();

        myJournal.AddEntry(prompt, response);
        Console.WriteLine("Journal entry added.");
    }
    static void SaveJournal()
    {
        Console.Write("Enter filename to save the journal: ");
        string filename = Console.ReadLine();
        myJournal.SaveToFile(filename);
    }
    static void LoadJournal()
    {
        Console.Write("Enter filename to load the journal: ");
        string filename = Console.ReadLine();
        myJournal.LoadFromFile(filename);
    }
}