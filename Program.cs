// Projektuppgift i kursen Programmering i C#.NET (DT071G) vid Mittuniveristetet
// Skapad av Kajsa Classon, HT24
// Beskrivning

using System;
using Microsoft.Data.Sqlite;


namespace ptApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                // Skriv ut meny
                Console.Clear();
                Console.CursorVisible = false;
                Console.WriteLine("T R Ä N I N G S D A G B O K \n\n");

                Console.WriteLine("1. ");

                Console.WriteLine("\nX. Avsluta\n");

                int input = (int)Console.ReadKey(true).Key;
                switch (input)
                {
                    case '1':
                        break;

                    case 88: // X för exit
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}