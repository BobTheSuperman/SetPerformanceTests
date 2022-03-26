using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SetPerformanceTests
{
    enum WordCount
    {
        OneThousand,
        TenThousand,
        OneHundredThousand,
        Maximum
    }
    class Program
    {
        #region public variables

        public static WordCount TestWordCount = WordCount.Maximum;
        public static Stopwatch SW = new Stopwatch();
        public static HashSet<string> testHashSet = new HashSet<string>();
        public static SortedSet<string> testSortedSet = new SortedSet<string>();

        #endregion

        static void Main(string[] args)
        {
            string[] words = ReadText(TestWordCount);
            Console.WriteLine($"Testing for { words.Length} words");

            AddingTesting(words);

            InitializationTesting(words);

            RemovalTesting(words);
        }

        static void RemovalTesting(string[] words)
        {
            Console.WriteLine($"{Environment.NewLine}RemovalTesting");
            Console.WriteLine($"{Environment.NewLine}Testing \"Remove\"");

            string wordToRemove = words[200];

            SW.Restart();
            testHashSet.Remove(wordToRemove);
            SW.Stop();

            PrintResults(testHashSet.GetType(),testHashSet.Count, SW.ElapsedTicks);

            SW.Restart();
            testSortedSet.Remove(wordToRemove);
            SW.Stop();

            PrintResults(testSortedSet.GetType(), testSortedSet.Count, SW.ElapsedTicks);

            Console.WriteLine($"{Environment.NewLine}Testing \"RemoveWhere\"");

            SW.Restart();
            testHashSet.RemoveWhere(s => s.Contains("ы"));
            SW.Stop();

            PrintResults(testHashSet.GetType(), testHashSet.Count, SW.ElapsedTicks);

            SW.Restart();
            testSortedSet.RemoveWhere(s => s.Contains("ы"));
            SW.Stop();

            PrintResults(testSortedSet.GetType(), testSortedSet.Count, SW.ElapsedTicks);
        }

        static void InitializationTesting(string [] words)
        {
            Console.WriteLine($"{Environment.NewLine}InitializationTesting");

            SW.Restart();
            HashSet<string> testHashSetInit = new HashSet<string>(words);
            SW.Stop();

            PrintResults(testHashSetInit.GetType(), testHashSetInit.Count, SW.ElapsedTicks);

            SW.Restart();
            SortedSet<string> testSortedSetInit = new SortedSet<string>(words);
            SW.Stop();

            PrintResults(testSortedSetInit.GetType(), testSortedSetInit.Count, SW.ElapsedTicks);
        }

        static void AddingTesting(string[] words)
        {
            Console.WriteLine($"{Environment.NewLine}AddingTesting");

            SW.Start();
            foreach (var word in words)
            {
                testHashSet.Add(word);
            }
            SW.Stop();

            PrintResults(testHashSet.GetType(), testHashSet.Count, SW.ElapsedTicks);

            SW.Restart();
            foreach (var word in words)
            {
                testSortedSet.Add(word);
            }
            SW.Stop();

            PrintResults(testSortedSet.GetType(), testSortedSet.Count, SW.ElapsedTicks);
        }

        static void PrintResults(Type type,int count, long time)
        {
            if (type == typeof(HashSet<string>))
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if (type == typeof(SortedSet<string>))
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.WriteLine($"{type.Name}\n\tCount = {count} \n\tSpent time {time}\n");
            Console.ResetColor();
        }

        static string[] ReadText(WordCount wordCount)
        {
            string text = System.IO.File.ReadAllText("../../../Text.txt");

            switch (wordCount)
            {
                case WordCount.OneThousand:
                    return text.Split(' ', 1000, StringSplitOptions.RemoveEmptyEntries);
                case WordCount.TenThousand:
                    return text.Split(' ', 10000, StringSplitOptions.RemoveEmptyEntries);
                case WordCount.OneHundredThousand:
                    return text.Split(' ', 100000, StringSplitOptions.RemoveEmptyEntries);
                case WordCount.Maximum:
                    return text.Split(' ', 1000000, StringSplitOptions.RemoveEmptyEntries);
                default:
                    throw new ArgumentOutOfRangeException(nameof(wordCount), wordCount, null);
            }
        }
    }
}
