using System; // For console input/output
using System.IO; // For file handling
using System.Collections.Generic; // For lists and collections

// Base class for Goals
abstract class Goal
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int Points { get; set; }

    // Constructor
    protected Goal(string name, string description, int points)
    {
        Name = name;
        Description = description;
        Points = points;
    }

    // Abstract methods for subclasses to implement
    public abstract string SaveData();
    public abstract void Display();
    public abstract int RecordEvent();
}

// SimpleGoal class
class SimpleGoal : Goal
{
    private bool Completed { get; set; } // Encapsulated property

    public SimpleGoal(string name, string description, int points)
        : base(name, description, points)
    {
        Completed = false;
    }

    public void SetCompleted(bool completed)
    {
        Completed = completed;
    }

    public override string SaveData()
    {
        return $"Simple,{Name},{Description},{Points},{Completed}";
    }

    public override void Display()
    {
        string status = Completed ? "[X]" : "[ ]";
        Console.WriteLine($"{status} {Name}: {Description} ({Points} pts)");
    }

    public override int RecordEvent()
    {
        if (!Completed)
        {
            Completed = true;
            return Points;
        }
        return 0;
    }
}

// EternalGoal class
class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points)
        : base(name, description, points) { }

    public override string SaveData()
    {
        return $"Eternal,{Name},{Description},{Points}";
    }

    public override void Display()
    {
        Console.WriteLine($"[*] {Name}: {Description} ({Points} pts per event)");
    }

    public override int RecordEvent()
    {
        return Points;
    }
}

// ChecklistGoal class
class ChecklistGoal : Goal
{
    private int CurrentCount { get; set; } // Encapsulated property
    public int TargetCount { get; private set; }
    public int BonusPoints { get; private set; }

    public ChecklistGoal(string name, string description, int points, int targetCount, int bonusPoints)
        : base(name, description, points)
    {
        TargetCount = targetCount;
        CurrentCount = 0;
        BonusPoints = bonusPoints;
    }

    public void SetCurrentCount(int count)
    {
        CurrentCount = count;
    }

    public override string SaveData()
    {
        return $"Checklist,{Name},{Description},{Points},{TargetCount},{CurrentCount},{BonusPoints}";
    }

    public override void Display()
    {
        Console.WriteLine($"[ ] {Name}: {Description} ({Points} pts each, {BonusPoints} bonus for {TargetCount} completions) [{CurrentCount}/{TargetCount}]");
    }

    public override int RecordEvent()
    {
        CurrentCount++;
        if (CurrentCount >= TargetCount)
        {
            CurrentCount = TargetCount; // Cap the count
            return Points + BonusPoints;
        }
        return Points;
    }
}

// Main Program
class EternalQuestProgram
{
    private static List<Goal> goals = new List<Goal>();
    private static int totalPoints = 0;

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine($"\nTotal Points: {totalPoints}");
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. List Goals");
            Console.WriteLine("3. Record Event");
            Console.WriteLine("4. Save Goals");
            Console.WriteLine("5. Load Goals");
            Console.WriteLine("6. Quit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateNewGoal();
                    break;
                case "2":
                    ListGoals();
                    break;
                case "3":
                    RecordEvent();
                    break;
                case "4":
                    SaveGoals();
                    break;
                case "5":
                    LoadGoals();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void CreateNewGoal()
    {
        Console.WriteLine("Select Goal Type:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        string choice = Console.ReadLine();

        Console.Write("Enter name: ");
        string name = Console.ReadLine();
        Console.Write("Enter description: ");
        string description = Console.ReadLine();
        Console.Write("Enter points: ");
        int points = int.Parse(Console.ReadLine());

        if (choice == "1")
        {
            goals.Add(new SimpleGoal(name, description, points));
        }
        else if (choice == "2")
        {
            goals.Add(new EternalGoal(name, description, points));
        }
        else if (choice == "3")
        {
            Console.Write("Enter target count: ");
            int targetCount = int.Parse(Console.ReadLine());
            Console.Write("Enter bonus points: ");
            int bonusPoints = int.Parse(Console.ReadLine());
            goals.Add(new ChecklistGoal(name, description, points, targetCount, bonusPoints));
        }
        else
        {
            Console.WriteLine("Invalid choice.");
        }
    }

    static void ListGoals()
    {
        if (goals.Count == 0)
        {
            Console.WriteLine("No goals created.");
            return;
        }

        for (int i = 0; i < goals.Count; i++)
        {
            Console.Write($"{i + 1}. ");
            goals[i].Display();
        }
    }

    static void RecordEvent()
    {
        ListGoals();
        Console.Write("Select a goal to record: ");
        int index = int.Parse(Console.ReadLine()) - 1;

        if (index >= 0 && index < goals.Count)
        {
            int pointsEarned = goals[index].RecordEvent();
            totalPoints += pointsEarned;
            Console.WriteLine($"You earned {pointsEarned} points!");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

    static void SaveGoals()
    {
        Console.Write("Enter filename to save: ");
        string filename = Console.ReadLine();

        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine(totalPoints);
            foreach (Goal goal in goals)
            {
                writer.WriteLine(goal.SaveData());
            }
        }

        Console.WriteLine("Goals saved successfully.");
    }

    static void LoadGoals()
    {
        Console.Write("Enter filename to load: ");
        string filename = Console.ReadLine();

        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.");
            return;
        }

        goals.Clear();
        using (StreamReader reader = new StreamReader(filename))
        {
            totalPoints = int.Parse(reader.ReadLine());
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                string type = parts[0];
                string name = parts[1];
                string description = parts[2];
                int points = int.Parse(parts[3]);

                if (type == "Simple")
                {
                    bool completed = bool.Parse(parts[4]);
                    var goal = new SimpleGoal(name, description, points);
                    goal.SetCompleted(completed); // Use method to set completed
                    goals.Add(goal);
                }
                else if (type == "Eternal")
                {
                    goals.Add(new EternalGoal(name, description, points));
                }
                else if (type == "Checklist")
                {
                    int targetCount = int.Parse(parts[4]);
                    int currentCount = int.Parse(parts[5]);
                    int bonusPoints = int.Parse(parts[6]);
                    var checklistGoal = new ChecklistGoal(name, description, points, targetCount, bonusPoints);
                    checklistGoal.SetCurrentCount(currentCount); // Use method to set current count
                    goals.Add(checklistGoal);
                }
            }
        }

        Console.WriteLine("Goals loaded successfully.");
    }
}