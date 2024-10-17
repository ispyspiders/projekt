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
            PtApp ptApp = new PtApp();

            while (true)
            {
                // Skriv ut meny
                Console.Clear();
                Console.CursorVisible = false;
                Console.WriteLine("T R Ä N I N G S D A G B O K \n\n");

                Console.WriteLine("1. Logga in");
                Console.WriteLine("2. Skapa ny användare");

                Console.WriteLine("\nX. Avsluta\n");

                int input = (int)Console.ReadKey(true).Key;
                switch (input)
                {
                    case '1': // Logga in
                        break;

                    case '2': // Skapa användare
                        Console.Clear();
                        Console.CursorVisible = false;
                        Console.WriteLine("S K A P A   N Y   A N V Ä N D A R E\n\n");
                        break;

                    case 88: // X för exit
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}