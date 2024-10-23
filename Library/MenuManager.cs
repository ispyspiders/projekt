// Klass för att hantera menyer i PTapp.
// Skapad av Kajsa Classon, HT24.

using System.ComponentModel;
using System.Data;

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
        // PtApp ptApp = new PtApp();
        User user = new User();
        string? username; // Variabel för användarnamn
        string? password; // Variabel för lösenord
        string? loggedinUser = null; // Variabel för inloggad användare, null till en början
        int? loggedinUserId = null; // Variabel för inloggad användares id, null till en början


        public void DrawMenu()
        {
            MenuState menuState = MenuState.main;
            if (loggedinUser is not null) menuState = MenuState.loggedIn;

            switch (menuState)
            {
                case MenuState.main:
                    DrawMainMenu();
                    break;
                case MenuState.loggedIn:
                    DrawLoggedinMenu();
                    break;
                case MenuState.workout:
                    break;
            }
        }

        // Main menu, not logged in
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
                    Console.Clear(); // Rensa skärm
                    Console.CursorVisible = false; // släck cursor
                    Console.WriteLine("L O G G A   I N\n\n");

                    Console.Write("Användarnamn: ");
                    Console.CursorVisible = true; // Tänd cursor
                    username = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(username)) // Är användarnamn null eller blanksteg skriv ut felmeddelande
                    {
                        Console.WriteLine("\nAnvändarnamn måste anges.");
                        Console.ResetColor(); // Återställ textfärg
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                        Console.ReadKey();
                    }
                    else
                    {
                        if (!user.GetUserByName(username)) // Finns användarnamnet inte i db
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nIngen användare med användarnamn {username} finns i databasen.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                            break; // Avbryt
                        }
                        Console.Write("Lösenord: ");
                        password = Console.ReadLine();
                        if (String.IsNullOrWhiteSpace(password)) // Är lösenord null eller blanksteg skriv ut felmeddelande
                        {
                            Console.WriteLine("\nLösenord måste anges.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.ReadKey();
                        }
                        else
                        {
                            if (user.LoginUser(username, password)) //Om inloggning lyckas
                            {
                                loggedinUser = username; // spara username i variabel för inloggad användare
                                loggedinUserId = user.GetUserId(username); // spara id i variabel för inloggad användare
                            }
                        }
                    }
                    break;

                case '2': // Skapa användare
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
                    if (user.GetUserByName(username))
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

                    if (user.RegisterUser(username, password))
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

        // Main menu, user logged in
        public void DrawLoggedinMenu()
        {
            Console.WriteLine("T R Ä N I N G S D A G B O K \n");
            Console.WriteLine($"Inloggad som: {username}");
            Console.WriteLine($"Antal tränade pass: ?");
            Console.WriteLine($"Total träningstid: ? \n");

            Console.WriteLine($"---------------------------------\n");

            Console.WriteLine("1. Registrera pass");
            Console.WriteLine("2. Uppdatera pass");
            Console.WriteLine("3. Radera pass");

            Console.WriteLine("\nX. Logga ut\n");

            Console.WriteLine($"---------------------------------\n");

            Console.WriteLine("Mina pass");



            int input = (int)Console.ReadKey(true).Key;
            switch (input)
            {
                case '1': // Registrera pass
                    break;
                case '2':
                    break;
                case '3':
                    break;
                case 88: // x för logga ut
                    loggedinUser = null;
                    loggedinUserId = null;
                    Console.Clear();
                    break;
            }
        }

    }
}