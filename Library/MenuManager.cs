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
        Workout wo = new Workout();
        string? username; // Variabel för användarnamn
        string? password; // Variabel för lösenord
        string? loggedinUser = null; // Variabel för inloggad användare, null till en början
        int? loggedinUserId = null; // Variabel för inloggad användares id, null till en början
        int userId;
        int? activeWorkout = null;

        public void DrawMenu()
        {
            MenuState menuState = MenuState.main;
            if (loggedinUser is not null) menuState = MenuState.loggedIn;
            if (activeWorkout is not null) menuState = MenuState.workout;

            switch (menuState)
            {
                case MenuState.main:
                    DrawMainMenu();
                    break;
                case MenuState.loggedIn:
                    DrawLoggedinMenu();
                    break;
                case MenuState.workout:
                    DrawWorkoutMenu();
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
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
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
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
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
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine("\nInloggning misslyckades! Fel avändarnamn/lösenord.");
                                Console.ResetColor(); // Återställ textfärg
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.ReadKey();
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
            if (loggedinUserId is not null) { userId = (int)loggedinUserId; }
            // Hämta användarens samtliga pass
            List<Workout> activeUsersWorkouts = user.GetWorkoutsForUserId(userId);

            Console.WriteLine("T R Ä N I N G S D A G B O K \n");
            Console.WriteLine($"Inloggad som: {username}");
            Console.WriteLine($"Antal tränade pass: {activeUsersWorkouts.Count}");
            Console.WriteLine($"Total träningstid: {user.getUsersTotalWorkoutTime(userId)} min");

            Console.WriteLine($"---------------------------------\n");

            Console.WriteLine("1. Registrera pass");
            Console.WriteLine("2. Uppdatera pass");
            Console.WriteLine("3. Radera pass");

            Console.WriteLine("\nX. Logga ut\n");

            Console.WriteLine($"---------------------------------\n");

            // ANVÄNDARES PASS
            Console.WriteLine("Mina pass: \n");
            // skriv ut pass till konsol
            foreach (Workout workout in activeUsersWorkouts)
            {
                Console.WriteLine($"[{workout.Id}] {workout.DateTime:dd-MM-yyyy}, {workout.Duration} min, {workout.Intensity} intensitet");
            }

            Console.WriteLine($"\n---------------------------------\n");

            int input = (int)Console.ReadKey(true).Key;
            switch (input)
            {
                case '1': // Registrera pass
                    string? dateInput;
                    string? durationInput;
                    string? intensityInput;

                    // DATUM FÖR PASS
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("R E G I S T R E R A   P A S S\n\n");

                        // Datum för pass
                        Console.Write("Datum för pass (DD-MM-YYYY): ");
                        Console.CursorVisible = true; // Tänd cursor
                        dateInput = Console.ReadLine(); // läs in input

                        if (String.IsNullOrWhiteSpace(dateInput)) // Har datuminput inte ett innehåll, skriv ut felmeddelande
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nDatum måste anges.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                        else
                        {
                            if (!Workout.CheckIfValidDate(dateInput))
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nOgiltigt datumformat.");
                                Console.ResetColor(); // Återställ textfärg                                
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.CursorVisible = false; // släck cursor
                                Console.ReadKey();
                                dateInput = null;
                            }
                        }
                    }
                    while (String.IsNullOrWhiteSpace(dateInput));

                    // TIDSÅTGÅNG
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("R E G I S T R E R A   P A S S\n\n");

                        Console.Write($"Datum för pass: {dateInput}\n");

                        // Tidsåtgång
                        Console.Write("Tidsåtgång (minuter): ");
                        Console.CursorVisible = true; // Tänd cursor
                        durationInput = Console.ReadLine(); // läs in input

                        if (String.IsNullOrWhiteSpace(durationInput)) // Kontroll att input inte är null eller blanksteg
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nTidsåtgång måste anges. Vänligen ange tidsåtgång i hela minuter.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                        else
                        {
                            if (!int.TryParse(durationInput, out int duration)) // kontroll att tidsåtgång är numeriskt
                            {
                                durationInput = null; // sätt input till null så att vi fortsättrer loopen
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nTidsåtgång ej numeriskt. Vänligen ange tidsåtgång i hela minuter.");
                                Console.ResetColor(); // Återställ textfärg
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.CursorVisible = false; // släck cursor
                                Console.ReadKey();
                            }
                            if (duration < 0) // kontroll att tidsåtgång inte är negativt
                            {
                                durationInput = null; // sätt input till null så att vi fortsättrer loopen
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nTidsåtgång får ej vara negativt. Vänligen ange tidsåtgång i hela minuter.");
                                Console.ResetColor(); // Återställ textfärg
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.CursorVisible = false; // släck cursor
                                Console.ReadKey();
                            }
                        }
                    }
                    while (String.IsNullOrWhiteSpace(durationInput));

                    // INTENSITET
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("R E G I S T R E R A   P A S S\n\n");

                        Console.WriteLine($"Datum för pass: {dateInput}");
                        Console.WriteLine($"Tidsåtgång: {durationInput}\n");

                        // Intensitet
                        Console.Write("Intensitet (Låg/Medel/Hög): ");
                        Console.CursorVisible = true; // Tänd cursor
                        intensityInput = Console.ReadLine(); // läs in input
                        if (String.IsNullOrWhiteSpace(intensityInput))
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nIntensitet måste anges. Vänligen ange Låg, Medel eller Hög.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                        else
                        {
                            if (!Workout.CheckIfValidIntensity(intensityInput))
                            {
                                intensityInput = null;
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nOgiltigt värde! Vänligen ange Låg, Medel eller Hög.");
                                Console.ResetColor(); // Återställ textfärg
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.CursorVisible = false; // släck cursor
                                Console.ReadKey();
                            }

                        }

                    }
                    while (String.IsNullOrWhiteSpace(intensityInput));

                    if (loggedinUserId is not null)
                    {
                        // userId = (int)loggedinUserId;
                        Workout woToReg = new Workout(userId, dateInput, int.Parse(durationInput), intensityInput);
                        activeWorkout = woToReg.RegisterWorkout();
                        if (activeWorkout > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green; // Sätt textfärg till grön.
                            Console.WriteLine("Pass registrerat!");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nFel vid registrering av pass.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                    }
                    break;
                case '2': // Uppdatera pass
                    Console.Clear(); // Rensa skärm
                    Console.CursorVisible = false; // släck cursor
                    Console.WriteLine("U P P D A T E R A   P A S S\n\n");


                    Console.WriteLine($"---------------------------------\n");

                    Console.WriteLine("Mina pass: \n");
                    // skriv ut pass till konsol
                    foreach (Workout workout in activeUsersWorkouts)
                    {
                        Console.WriteLine($"[{workout.Id}] {workout.DateTime:dd-MM-yyyy}, {workout.Duration} min, {workout.Intensity} intensitet");
                    }

                    Console.WriteLine($"\n---------------------------------\n");

                    Console.Write("Ange id för det pass du vill uppdatera: ");
                    Console.CursorVisible = true; // Tänd cursor
                    string? idInput = Console.ReadLine(); // läsin input
                    if (String.IsNullOrWhiteSpace(idInput)) // om null eller blanksteg
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine($"\nId för träningspass måste anges.");
                        Console.ResetColor(); // Återställ textfärg
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                        Console.CursorVisible = false; // släck cursor
                        Console.ReadKey();
                    }
                    else
                    {
                        if (!int.TryParse(idInput, out int updateId)) // in inte går att omvandla till int
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nId för träningspass ej numeriskt.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                        else
                        {
                            if (wo.GetWorkoutInfo(updateId) is not null) // Om det går hämta workout med valt id från db
                            {
                                activeWorkout = updateId; // sätt aktivt träningspass till angivet id, tränigspass meny laddas.
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nInget träningspass med id {updateId} hittades.");
                                Console.ResetColor(); // Återställ textfärg
                                Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                Console.CursorVisible = false; // släck cursor
                                Console.ReadKey();
                            }
                        }
                    }
                    break;
                case '3': // Radera pass
                    Console.Clear(); // Rensa skärm
                    Console.CursorVisible = false; // släck cursor
                    Console.WriteLine("R A D E R A   P A S S\n\n");


                    Console.WriteLine($"---------------------------------\n");

                    Console.WriteLine("Mina pass: \n");
                    // skriv ut pass till konsol
                    foreach (Workout workout in activeUsersWorkouts)
                    {
                        Console.WriteLine($"[{workout.Id}] {workout.DateTime:dd-MM-yyyy}, {workout.Duration} min, {workout.Intensity} intensitet");
                    }

                    Console.WriteLine($"\n---------------------------------\n");

                    Console.Write("Ange id för det pass du vill radera: ");
                    Console.CursorVisible = true; // Tänd cursor
                    string? deleteInput = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(deleteInput)) // om null eller blanksteg
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine($"\nId för träningspass måste anges.");
                        Console.ResetColor(); // Återställ textfärg
                        Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                        Console.CursorVisible = false; // släck cursor
                        Console.ReadKey();
                    }
                    else
                    {
                        if (!int.TryParse(deleteInput, out int deleteId)) // in inte går att omvandla till int
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"\nId för träningspass ej numeriskt.");
                            Console.ResetColor(); // Återställ textfärg
                            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                            Console.CursorVisible = false; // släck cursor
                            Console.ReadKey();
                        }
                        else
                        {
                            // Finns id i db
                            // Är du säker
                            // RAdera
                        }
                    }
                    break;
                case 88: // x för logga ut
                    loggedinUser = null;
                    loggedinUserId = null;
                    Console.Clear();
                    break;
            }
        }

        // Pass-meny
        public void DrawWorkoutMenu()
        {
            Console.WriteLine($"T R Ä N I N G S P A S S   {activeWorkout}\n\n");

            if (activeWorkout is not null) // Om vald workout inte är null
            {
                // Skriv ut pass information
                Workout? workoutInfo = wo.GetWorkoutInfo((int)activeWorkout);
                if (workoutInfo is not null)
                {
                    Console.WriteLine($"Datum: {workoutInfo.DateTime:dd-MM-yyyy}");
                    Console.WriteLine($"Tidsåtgång: {workoutInfo.Duration}");
                    Console.WriteLine($"Intensitet: {workoutInfo.Intensity}");

                    // Skriv ut övningar
                    Console.WriteLine($"\nÖvningar:");
                    List<Exercise> woExercises = wo.GetExercisesForWorkout((int)activeWorkout);
                    if (woExercises.Count > 0)
                    {
                        foreach (Exercise exercise in woExercises)
                        {
                            Console.WriteLine($"\t[{exercise.Id} {exercise.Name}, {exercise.Weight} kg x {exercise.Reps}]");
                        }
                    }
                    else // Inga övningar finns att hämta ut.
                    {
                        Console.WriteLine("Inga övningar registrerade.");
                    }
                }
                else // Pass hittas inte i db.
                {
                    Console.WriteLine("Inga uppgifter hittades för det angivna träningspasset.");
                }
            }
            Console.WriteLine($"\n---------------------------------\n");

            Console.WriteLine("1. Registrera övning ");
            Console.WriteLine("2. Uppdatera övning");
            Console.WriteLine("3. Radera övning");

            Console.WriteLine("\n4. Redigera passinformation");

            Console.WriteLine("\nX. Avbryt\n");

            int input = (int)Console.ReadKey(true).Key;
            switch (input)
            {
                case '1':
                    break;
                case '2':
                    break;
                case '3':
                    break;
                case '4':
                    break;

                case 88: // Avbryt, backar till inloggad användares meny genom att sätta activeWorkout till null och ladda om.
                    activeWorkout = null;
                    Console.Clear();
                    break;
            }
        }
    }
}