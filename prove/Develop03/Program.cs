using System;
using System.Collections.Generic;
using System.Linq;
public class Reference
{
    private string _book;
    private int _startVerse;
    private int? _endVerse;

    public Reference(string book, int startVerse)
    {
        _book = book;
        _startVerse = startVerse;
        _endVerse = null;
    }

    public Reference(string book, int startVerse, int endVerse)
    {
        _book = book;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public override string ToString()
    {
        return _endVerse == null ? $"{_book} {_startVerse}" : $"{_book} {_startVerse}-{_endVerse}";
    }
}

public class Word
{
    private string _text;
    private bool _isHidden;

    public Word(string text)
    {
        _text = text;
        _isHidden = false;
    }

    public void Hide()
    {
        _isHidden = true;
    }

    public bool IsHidden()
    {
        return _isHidden;
    }

    public override string ToString()
    {
        return _isHidden ? "____" : _text;
    }
}



public class Scripture
{
    private Reference _reference;
    private List<Word> _words;

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = text.Split(' ').Select(word => new Word(word)).ToList();
    }

    public void HideRandomWords()
    {
        Random random = new Random();
        int wordsToHide = 3;

        for (int i = 0; i < wordsToHide; i++)
        {
            var visibleWords = _words.Where(w => !w.IsHidden()).ToList();
            if (visibleWords.Count == 0) break;

            var wordToHide = visibleWords[random.Next(visibleWords.Count)];
            wordToHide.Hide();
        }
    }

    public bool AreAllWordsHidden()
    {
        return _words.All(word => word.IsHidden());
    }

    public override string ToString()
    {
        string wordsDisplay = string.Join(" ", _words);
        return $"{_reference}\n{wordsDisplay}";
    }
}

//mashing all together
public class Program
{
    public static void Main(string[] args)
    {
        Reference scriptureReference = new Reference("John", 3, 16);
        Scripture scripture = new Scripture(scriptureReference, "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life.");

        while (true)
        {
            Console.Clear();
            Console.WriteLine(scripture);

            if (scripture.AreAllWordsHidden())
            {
                Console.WriteLine("All words are hidden. Well done!");
                break;
            }

            Console.WriteLine("\nPress Enter to hide more words, or type 'quit' to exit.");
            string input = Console.ReadLine();
            if (input.ToLower() == "quit")
            {
                break;
            }

            scripture.HideRandomWords();
        }
    }
}