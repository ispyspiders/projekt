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
            MenuManager menuManager = new MenuManager();

            while (true)
            {
                // Skriv ut meny
                Console.Clear();
                Console.CursorVisible = false;
                menuManager.DrawMainMenu();

                
            }
        }
    }
}