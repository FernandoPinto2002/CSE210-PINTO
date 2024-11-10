using System;
using System.Collections.Generic;
using System.Threading;

namespace MindfulnessProgram
{
    // Base class for all activities
    public class Activity
    {
        protected string _name;
        protected string _description;
        protected int _duration;

        public Activity(string name, string description, int duration)
        {
            _name = name;
            _description = description;
            _duration = duration;
        }

        public void StartMessage()
        {
            Console.WriteLine($"Starting {_name}: {_description}");
            Console.WriteLine("Get ready...");
            ShowSpinner(3);
        }

        public void EndMessage()
        {
            Console.WriteLine($"You have completed {_name} for {_duration} seconds.");
            ShowSpinner(3);
        }

        protected void ShowCountdown(int seconds)
        {
            for (int i = seconds; i > 0; i--)
            {
                Console.Write($"{i} ");
                Thread.Sleep(1000);
                Console.Write("\b\b");
            }
            Console.WriteLine();
        }

        protected void ShowSpinner(int seconds)
        {
            string[] spinnerChars = { "|", "/", "-", "\\" };
            for (int i = 0; i < seconds * 4; i++)
            {
                Console.Write(spinnerChars[i % 4]);
                Thread.Sleep(250);
                Console.Write("\b \b");
            }
        }
    }

    // Derived class for breathing activity
    public class BreathingActivity : Activity
    {
        public BreathingActivity(int duration) 
            : base("Breathing Activity", "Focus on your breathing to relax.", duration) { }

        public void RunActivity()
        {
            StartMessage();
            for (int i = 0; i < _duration / 6; i++)
            {
                Console.WriteLine("Breathe in...");
                ShowCountdown(3);
                Console.WriteLine("Breathe out...");
                ShowCountdown(3);
            }
            EndMessage();
        }
    }

    // Derived class for reflection activity
    public class ReflectionActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Think of a time you helped someone in need.",
            "Think of a time you overcame a difficult challenge."
        };
        private List<string> _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "What did you learn about yourself through this experience?"
        };

        public ReflectionActivity(int duration) 
            : base("Reflection Activity", "Reflect on moments of personal strength.", duration) { }

        public void RunActivity()
        {
            StartMessage();
            Random random = new Random();
            Console.WriteLine(_prompts[random.Next(_prompts.Count)]);
            for (int i = 0; i < _duration / 5; i++)
            {
                Console.WriteLine(_questions[random.Next(_questions.Count)]);
                ShowSpinner(2);
            }
            EndMessage();
        }
    }

    // Derived class for listing activity
    public class ListingActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "List people you appreciate.",
            "List your personal strengths."
        };

        public ListingActivity(int duration) 
            : base("Listing Activity", "List positive things in your life.", duration) { }

        public void RunActivity()
        {
            StartMessage();
            Random random = new Random();
            Console.WriteLine(_prompts[random.Next(_prompts.Count)]);
            ShowCountdown(5);
            Console.WriteLine("Start listing items (press Enter after each one):");
            DateTime endTime = DateTime.Now.AddSeconds(_duration);
            int count = 0;
            while (DateTime.Now < endTime)
            {
                Console.ReadLine();
                count++;
            }
            Console.WriteLine($"You listed {count} items.");
            EndMessage();
        }
    }

    // Main program class
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose an activity:");
                Console.WriteLine("1. Breathing Activity");
                Console.WriteLine("2. Reflection Activity");
                Console.WriteLine("3. Listing Activity");
                Console.WriteLine("4. Quit");

                int choice = int.Parse(Console.ReadLine());

                if (choice == 4) break;

                Console.Write("Enter duration (in seconds): ");
                int duration = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        new BreathingActivity(duration).RunActivity();
                        break;
                    case 2:
                        new ReflectionActivity(duration).RunActivity();
                        break;
                    case 3:
                        new ListingActivity(duration).RunActivity();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}