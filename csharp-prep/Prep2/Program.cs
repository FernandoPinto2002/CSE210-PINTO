using System;

class Program
{
    static void Main(string[] args)
    {   

        Console.Write("Please enter your grade percentage: ");
        string grade = Console.ReadLine();
        int number = int.Parse(grade);
        string letter = "";

        if (number >= 90)
        {
            
            letter = "A";
            

        }

        else if (number >= 80)
        {
            letter = "B";
        }

         else if (number >= 70)
        {
            letter = "C";
        }

         else if (number >= 60)
        {
            letter = "D";
        }        

        else 
        {
            letter = "F";
        }
       
       Console.WriteLine($"You earned a {letter} grade, that means that you");
         if (number >= 90)
        {
            Console.WriteLine("You passed!");

        }

        else if (number >= 80)
        {
            Console.WriteLine("You passed!");
        }

         else if (number >= 70)
        {
            Console.WriteLine("You passed but almost failed");
        }
         else if (number >= 60)
        {
            Console.WriteLine("You failed!");
        }

        else 
        {
            Console.WriteLine("You failed!");
        }
        
    }
        
    
}