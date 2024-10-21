// Klass för att hantera menyer i PTapp.
// Skapad av Kajsa Classon, HT24.

using System.ComponentModel;

namespace ptApp
{

    public enum MenuState
    {
        main,
        loggedIn,
        workout
    }
    public class MenuManager
    {
        PtApp ptApp = new PtApp();

        public void DrawMainMenu()
        {
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
                    string? username; // Variabel för användarnamn
                    string? password; // Variabel för lösenord

                    // ANVÄNDARNAMN
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("S K A P A   N Y   A N V Ä N D A R E\n\n");

                        // Ange användarnamn
                        Console.Write("Välj användarnamn: ");
                        Console.CursorVisible = true; // Tänd cursor
                        username = Console.ReadLine();

                        if (String.IsNullOrWhiteSpace(username)) // Är användarnamn null eller blanksteg skriv ut felmeddelande
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine("\nAnvändarnamn måste ha ett innehåll.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                    }
                    while (String.IsNullOrWhiteSpace(username)); // Så länge användarnamn är null eller blanksteg

                    // Kontroll om användarnamn redan finns i db
                    if (ptApp.getUserByName(username))
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine($"\nEn användare med användarnamn {username} finns redan i databasen.");
                        Console.ResetColor(); // Återställ textfärg
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                        break;
                    }

                    // LÖSENORD
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.WriteLine("S K A P A   N Y   A N V Ä N D A R E\n\n");
                        Console.WriteLine($"Välj användarnamn: {username}");

                        // Ange lösenord
                        Console.Write("Välj lösenord: ");
                        password = Console.ReadLine();

                        if (String.IsNullOrWhiteSpace(password))
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nLösenord måste anges.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                        else
                        {
                            if (password.Length < 6)
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nLösenord måste vara minst 6 tecken.");
                                Console.ResetColor(); // Återställ textfärg
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.ReadKey();
                            }

                        }
                    }
                    while (String.IsNullOrWhiteSpace(password) || password.Length < 6);

                    if (ptApp.registerUser(username, password))
                    {
                        // ptApp.registerUser(username, password);
                        Console.ForegroundColor = ConsoleColor.Green; // Sätt textfärg till grön.
                        Console.WriteLine("Konto skapat!");
                        Console.ResetColor(); // Återställ textfärg
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine($"\nFel vid registrering av konto.");
                        Console.ResetColor(); // Återställ textfärg
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                    }
                    break;

                case 88: // X för exit
                    Environment.Exit(0);
                    break;
            }
        }
    }
}