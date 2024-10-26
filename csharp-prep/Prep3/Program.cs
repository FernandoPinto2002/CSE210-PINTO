using System;

class Program
{
    static void Main(string[] args)
    {
        Random rnd = new Random();
        int number = rnd.Next(1,10);
        string guess = Console.ReadLine();
        int number_guessed = int.Parse(guess);
        
        while (number_guessed != number)
        {
            Console.WriteLine("Try again, what is your guess");
            guess = Console.ReadLine();
            number_guessed = int.Parse(guess);

            if (number_guessed == number)
            {
                Console.WriteLine("You did it!");
            }

        }
            
    }
}