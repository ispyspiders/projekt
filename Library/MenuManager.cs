// Klass för att hantera menyer i PTapp.
// Skapad av Kajsa Classon, HT24.

using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

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

        private void DrawErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
            Console.WriteLine(message);
            Console.ResetColor(); // Återställ textfärg
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
            Console.CursorVisible = false; // släck cursor
            Console.ReadKey();
        }

        private void DrawSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green; // Sätt textfärg till grön.
            Console.WriteLine(message);
            Console.ResetColor(); // Återställ textfärg
            Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
            Console.CursorVisible = false; // släck cursor
            Console.ReadKey();
        }

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
            Console.Clear();
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
            Console.Clear();
            if (loggedinUserId is not null)
            {
                userId = (int)loggedinUserId;
            }
            // Hämta användarens samtliga pass
            List<Workout> activeUsersWorkouts = user.GetWorkoutsForUserId(userId); // Hämta lista med träningspass för användare
            Console.WriteLine("T R Ä N I N G S D A G B O K \n");
            Console.WriteLine($"Inloggad som: {username}");
            Console.Write($"Antal tränade pass: ");
            if (activeUsersWorkouts.Count > 0)
                Console.WriteLine($"{activeUsersWorkouts.Count}");
            else
                Console.WriteLine($"0");
            if (user.getUsersTotalWorkoutTime(userId) is not null)
            {
                Console.WriteLine($"Total träningstid: {user.getUsersTotalWorkoutTime(userId)} min");
            }

            Console.WriteLine($"---------------------------------\n");

            Console.WriteLine("1. Registrera pass");
            Console.WriteLine("2. Uppdatera pass");
            Console.WriteLine("3. Radera pass");

            Console.WriteLine("\nX. Logga ut\n");


            if (activeUsersWorkouts.Count > 0) // Om användare har pass registrerade skriv ut till konsol
            {
                Console.WriteLine($"---------------------------------\n");

                // ANVÄNDARES PASS
                Console.WriteLine("Mina pass: \n");
                // skriv ut pass till konsol
                for (int i = 0; i < activeUsersWorkouts.Count; i++)
                {
                    var workout = activeUsersWorkouts[i];
                    Console.WriteLine($"[{i}] {workout.DateTime:dd-MM-yyyy}, {workout.Duration} min, {workout.Intensity} intensitet");
                }

                Console.WriteLine($"\n---------------------------------\n");
            }

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
                    for (int i = 0; i < activeUsersWorkouts.Count; i++)
                    {
                        var workout = activeUsersWorkouts[i];
                        Console.WriteLine($"[{i}] {workout.DateTime:dd-MM-yyyy}, {workout.Duration} min, {workout.Intensity} intensitet");
                    }

                    Console.WriteLine($"\n---------------------------------\n");

                    Console.Write("Ange index för det pass du vill uppdatera: ");
                    Console.CursorVisible = true; // Tänd cursor
                    string? indexInput = Console.ReadLine(); // läsin input
                    if (String.IsNullOrWhiteSpace(indexInput)) // om null eller blanksteg
                    {
                        DrawErrorMessage("\nIndex för träningspass måste anges.");
                    }
                    else
                    {
                        if (!int.TryParse(indexInput, out int updateIndex)) // in inte går att omvandla till int
                        {
                            DrawErrorMessage("\nIndex för träningspass ej numeriskt.");
                        }
                        else
                        {
                            if (updateIndex >= 0 && updateIndex < activeUsersWorkouts.Count) // Om update index finns i listan
                            {
                                int updateId = activeUsersWorkouts[updateIndex].Id;
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
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"\nInget träningspass med index {updateIndex} hittades.");
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
                    for (int i = 0; i < activeUsersWorkouts.Count; i++)
                    {
                        var workout = activeUsersWorkouts[i];
                        Console.WriteLine($"[{i}] {workout.DateTime:dd-MM-yyyy}, {workout.Duration} min, {workout.Intensity} intensitet");
                    }

                    Console.WriteLine($"\n---------------------------------\n");

                    Console.Write("Ange index för det pass du vill radera: ");
                    Console.CursorVisible = true; // Tänd cursor
                    string? deleteInput = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(deleteInput)) // om null eller blanksteg
                    {
                        DrawErrorMessage("\nIndex för träningspass måste anges.");
                    }
                    else
                    {
                        if (!int.TryParse(deleteInput, out int deleteIndex)) // in inte går att omvandla till int
                        {
                            DrawErrorMessage("\nIndex för träningspass ej numeriskt.");
                        }
                        else
                        {
                            if (deleteIndex >= 0 && deleteIndex < activeUsersWorkouts.Count) // Om deleteindex finns i listan
                            {
                                int deleteId = activeUsersWorkouts[deleteIndex].Id; // läs in id för det index som ska raderas
                                Workout? woToDel = wo.GetWorkoutInfo(deleteId);
                                if (woToDel is not null) // Finns id i db
                                {
                                    if (woToDel.UserId == loggedinUserId) // Om användarId för pass som ska raderas tillhör inloggad användare
                                    {
                                        // Är du säker
                                        Console.Clear(); // Rensa skärm
                                        Console.CursorVisible = false; // släck cursor
                                        Console.WriteLine($"R A D E R A   P A S S {deleteIndex}\n\n");

                                        Console.WriteLine($"Är du säker på att du vill radera pass {deleteIndex}?\n");
                                        Console.WriteLine("1. Radera");
                                        Console.WriteLine("\nX. Avbryt");

                                        input = (int)Console.ReadKey(true).Key;
                                        switch (input)
                                        {
                                            case '1': // Radera
                                                if (wo.DeleteWorkout(deleteId)) // Om lyckad radereing
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Green; // Sätt textfärg till grön.
                                                    Console.WriteLine("\nPass raderat!");
                                                    Console.ResetColor(); // Återställ textfärg
                                                    Console.WriteLine("\nTryck på valfri tangent för att fortsätta.");
                                                    Console.CursorVisible = false; // släck cursor
                                                    Console.ReadKey();
                                                }
                                                else // Vid misslyckad radering 
                                                {
                                                    DrawErrorMessage($"\nRadering av träningspass misslyckades!.");
                                                }
                                                break;
                                            case 88: // x för avbryt
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        DrawErrorMessage("\nObehörig användare! Pass ej raderat.");
                                    }
                                }
                                else // pass hittas inte i db
                                {
                                    DrawErrorMessage($"\nInget pass med id {deleteId} hittades i databasen.");
                                }
                            }
                            else // Index hittas inte i litsa av träningspass
                            {
                                DrawErrorMessage($"\nInget pass med index {deleteIndex} hittades.");
                            }
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
            Console.Clear();
            Console.WriteLine($"T R Ä N I N G S P A S S\n\n");

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
                        for (int i = 0; i < woExercises.Count; i++)
                        {
                            var exercise = woExercises[i];
                            Console.WriteLine($"  [{i}] {exercise.Name}, {exercise.Weight} kg x {exercise.Reps} reps");
                        }
                    }
                    else // Inga övningar finns att hämta ut.
                    {
                        Console.WriteLine("  Inga övningar registrerade.");
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
                case '1': // Registrera övning
                    string? nameInput;
                    string? weightInput;
                    string? repsInput;

                    // NAMN FÖR ÖVNING
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("R E G I S T R E R A   Ö V N I N G\n\n");

                        // Namn för övning
                        Console.Write("Namn på övning: ");
                        Console.CursorVisible = true; // Tänd cursor
                        nameInput = Console.ReadLine(); // läs in input

                        if (String.IsNullOrWhiteSpace(nameInput)) // Har input inte ett innehåll, skriv ut felmeddelande
                        {
                            DrawErrorMessage("\nNamn måste anges.");
                        }
                    }
                    while (String.IsNullOrWhiteSpace(nameInput));

                    // VIKT
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("R E G I S T R E R A   Ö V N I N G\n\n");

                        Console.WriteLine($"Namn på övning: {nameInput}\n");

                        Console.Write("Vikt (kg): ");
                        Console.CursorVisible = true; // Tänd cursor
                        weightInput = Console.ReadLine(); // läs in input

                        if (String.IsNullOrWhiteSpace(weightInput)) // Har input inte ett innehåll, skriv ut felmeddelande
                        {
                            DrawErrorMessage("\nVikt måste anges. Vänligen ange vikt i hela kg. Används ingen vikt ange 0.");
                        }
                        else
                        {
                            if (!int.TryParse(weightInput, out int weight)) // kontroll att vikt är numeriskt
                            {
                                weightInput = null;
                                DrawErrorMessage("\nVikt måste vara numersikt. Vänligen ange vikt i hela kg.");
                            }
                            if (weight < 0)
                            {
                                weightInput = null;
                                DrawErrorMessage("\nVikt kan ej vara negativt.");
                            }
                        }
                    }
                    while (String.IsNullOrWhiteSpace(weightInput));

                    // REPS
                    do
                    {
                        Console.Clear(); // Rensa skärm
                        Console.CursorVisible = false; // släck cursor
                        Console.WriteLine("R E G I S T R E R A   Ö V N I N G\n\n");
                        Console.WriteLine($"Namn på övning: {nameInput}");
                        Console.WriteLine($"Vikt: {weightInput} kg\n");

                        Console.Write("Antal reps: ");
                        Console.CursorVisible = true; // Tänd cursor
                        repsInput = Console.ReadLine(); // läs in input

                        if (String.IsNullOrWhiteSpace(repsInput)) // Har input inte ett innehåll, skriv ut felmeddelande
                        {
                            DrawErrorMessage("\nAntal repetitioner måste anges.");
                        }
                        else
                        {
                            if (!int.TryParse(repsInput, out int reps)) // kontroll att vikt är numeriskt
                            {
                                repsInput = null;
                                DrawErrorMessage("\nReps måste vara numersikt. Vänligen ange antal repetitioner som ett heltal.");
                            }
                            if (reps < 0)
                            {
                                repsInput = null;
                                DrawErrorMessage("\nAntal repetitioner kan ej vara negativt.");
                            }
                        }
                    }
                    while (String.IsNullOrWhiteSpace(repsInput));

                    // KONTROLLERA ATT INLOGGAD ANVÄNDARE + PASSANVÄNDARE ÄR SAMMA
                    if (activeWorkout is not null && loggedinUserId is not null) // Om vald workout inte är null och inloggad användare inte är null
                    {
                        // Hämta info om pass
                        Workout? workoutInfo = wo.GetWorkoutInfo((int)activeWorkout);
                        if (workoutInfo is not null)
                        {
                            if (workoutInfo.UserId == loggedinUserId) // Om användar id för aktiv workout är samma som inloggad användares id
                            {
                                // Skapa nytt Exercise-object
                                Exercise exerciseToReg = new Exercise(nameInput, int.Parse(repsInput), int.Parse(weightInput));
                                // SKICKA FÖRFRÅGAN TILL DB
                                int? regResult = exerciseToReg.RegisterExercise((int)activeWorkout);
                                if (regResult > 0)
                                {
                                    DrawSuccessMessage("\nÖvning registrerad!");
                                }
                                else
                                {
                                    DrawErrorMessage("\nFel vid registrering av övning!");
                                }
                            }
                            else
                            { // Användar id stämmer inte mellan inloggad användare och passets ägare
                                DrawErrorMessage("\nObehörig användare! Fel vid registrering av övning.");
                            }

                        }
                    }

                    break;
                case '2': // Uppdatera övning
                    Console.Clear(); // Rensa skärm
                    Console.CursorVisible = false; // släck cursor
                    Console.WriteLine("U P P D A T E R A   Ö V N I N G\n\n");

                    // // Skriv ut övningar
                    if (activeWorkout is not null)
                    {
                        List<Exercise> woExercises = wo.GetExercisesForWorkout((int)activeWorkout);
                        if (woExercises.Count > 0) // Om övningar finns, skriv ut dessa och fråga efter index att uppdatera
                        {
                            Console.WriteLine($"Övningar:");
                            for (int i = 0; i < woExercises.Count; i++)
                            {
                                var exercise = woExercises[i];
                                Console.WriteLine($"  [{i}] {exercise.Name}, {exercise.Weight} kg x {exercise.Reps} reps");
                            }
                            Console.WriteLine($"\n---------------------------------\n");

                            // Ange index för övning att uppdatera
                            Console.Write("Ange index för den övning du vill uppdatera: ");
                            Console.CursorVisible = true; // Tänd cursor
                            string? indexInput = Console.ReadLine(); // läsin input
                            if (String.IsNullOrWhiteSpace(indexInput)) // om null eller blanksteg
                            {
                                DrawErrorMessage("\nIndex för övning måste anges.");
                            }
                            else
                            {
                                if (!int.TryParse(indexInput, out int updateIndex)) // index inte går att omvandla till int
                                {
                                    DrawErrorMessage("\nIndex för övning ej numeriskt.");
                                }
                                else
                                {
                                    if (updateIndex >= 0 && updateIndex < woExercises.Count) // Om update index finns i listan
                                    {
                                        int updateId = woExercises[updateIndex].Id; // Läs in id för valt index

                                        // VIKT
                                        do
                                        {
                                            Console.Clear(); // Rensa skärm
                                            Console.CursorVisible = false; // släck cursor
                                            Console.WriteLine("U P P D A T E R A   Ö V N I N G\n\n");

                                            Console.WriteLine($"Namn på övning: {woExercises[updateIndex].Name}\n");

                                            Console.Write("Vikt (kg): ");
                                            Console.CursorVisible = true; // Tänd cursor
                                            weightInput = Console.ReadLine(); // läs in input

                                            if (String.IsNullOrWhiteSpace(weightInput)) // Har input inte ett innehåll, skriv ut felmeddelande
                                            {
                                                DrawErrorMessage("\nVikt måste anges. Vänligen ange vikt i hela kg. Används ingen vikt ange 0.");
                                            }
                                            else
                                            {
                                                if (!int.TryParse(weightInput, out int weight)) // kontroll att vikt är numeriskt
                                                {
                                                    weightInput = null;
                                                    DrawErrorMessage("\nVikt måste vara numersikt. Vänligen ange vikt i hela kg.");
                                                }
                                                if (weight < 0)
                                                {
                                                    weightInput = null;
                                                    DrawErrorMessage("\nVikt kan ej vara negativt.");
                                                }
                                            }
                                        }
                                        while (String.IsNullOrWhiteSpace(weightInput));

                                        // REPS
                                        do
                                        {
                                            Console.Clear(); // Rensa skärm
                                            Console.CursorVisible = false; // släck cursor
                                            Console.WriteLine("U P P D A T E R A   Ö V N I N G\n\n");
                                            Console.WriteLine($"Namn på övning: {woExercises[updateIndex].Name}");
                                            Console.WriteLine($"Vikt: {weightInput} kg\n");

                                            Console.Write("Antal reps: ");
                                            Console.CursorVisible = true; // Tänd cursor
                                            repsInput = Console.ReadLine(); // läs in input

                                            if (String.IsNullOrWhiteSpace(repsInput)) // Har input inte ett innehåll, skriv ut felmeddelande
                                            {
                                                DrawErrorMessage("\nAntal repetitioner måste anges.");
                                            }
                                            else
                                            {
                                                if (!int.TryParse(repsInput, out int reps)) // kontroll att vikt är numeriskt
                                                {
                                                    repsInput = null;
                                                    DrawErrorMessage("\nReps måste vara numersikt. Vänligen ange antal repetitioner som ett heltal.");
                                                }
                                                if (reps < 0)
                                                {
                                                    repsInput = null;
                                                    DrawErrorMessage("\nAntal repetitioner kan ej vara negativt.");
                                                }
                                            }
                                        }
                                        while (String.IsNullOrWhiteSpace(repsInput));

                                        // Skapa ett exercise-objekt med ny info
                                        Exercise updateEx = new Exercise
                                        {
                                            Id = updateId,
                                            Name = woExercises[updateIndex].Name,
                                            Description = "",
                                            Reps = int.Parse(repsInput),
                                            Weight = int.Parse(weightInput)
                                        };
                                        if (updateEx.UpdateExercise(updateEx)) DrawSuccessMessage("\nÖvning uppdaterad!");
                                        else DrawErrorMessage("\nFel vid uppdatering av övning.");
                                    }
                                    else
                                    {
                                        DrawErrorMessage($"Ingen övning med index {updateIndex} hittades.");
                                    }
                                }
                            }
                        }
                        else // Inga övningar finns att hämta ut.
                        {
                            DrawErrorMessage("\nDet finns inga övningar att uppdatera.");
                        }
                    }
                    break;
                case '3': // Radera övning
                    Console.Clear(); // Rensa skärm
                    Console.CursorVisible = false; // släck cursor
                    Console.WriteLine("R A D E R A   Ö V N I N G\n\n");

                    // // Skriv ut övningar
                    if (activeWorkout is not null)
                    {
                        List<Exercise> woExercises = wo.GetExercisesForWorkout((int)activeWorkout);
                        if (woExercises.Count > 0)
                        {
                            Console.WriteLine($"Övningar:");
                            for (int i = 0; i < woExercises.Count; i++)
                            {
                                var exercise = woExercises[i];
                                Console.WriteLine($"  [{i}] {exercise.Name}, {exercise.Weight} kg x {exercise.Reps} reps");
                            }
                            Console.WriteLine($"\n---------------------------------\n");

                            Console.Write("Ange index för den övning du vill radera: ");
                            Console.CursorVisible = true; // Tänd cursor
                            string? indexInput = Console.ReadLine(); // läsin input
                            if (String.IsNullOrWhiteSpace(indexInput))
                            {
                                DrawErrorMessage("\nIndex för övning måste anges.");
                            }
                            else
                            {
                                if (!int.TryParse(indexInput, out int deleteIndex)) // index inte går att omvandla till int
                                {
                                    DrawErrorMessage("\nIndex för övning ej numeriskt.");
                                }
                                else
                                {
                                    if (deleteIndex >= 0 && deleteIndex < woExercises.Count) // Om deleteindex finns i listan
                                    {
                                        int deleteId = woExercises[deleteIndex].Id; // läs in id för det index som ska raderas
                                        Exercise ex = new Exercise();
                                        Exercise? exToDel = ex.GetExerciseInfo(deleteId);
                                        if (exToDel is not null) // finns id i db
                                        {
                                            // Är du säker
                                            Console.Clear(); // Rensa skärm
                                            Console.CursorVisible = false; // släck cursor
                                            Console.WriteLine($"R A D E R A   Ö V N I N G   [{deleteIndex}]\n\n");

                                            Console.WriteLine($"Är du säker på att du vill radera övning [{deleteIndex}] {exToDel.Name}?\n");
                                            Console.WriteLine("1. Radera");
                                            Console.WriteLine("\nX. Avbryt");

                                            input = (int)Console.ReadKey(true).Key;
                                            switch (input)
                                            {
                                                case '1': // Radera
                                                    if (exToDel.DeleteExercise(deleteId)) // Om lyckad radereing
                                                    {
                                                        DrawSuccessMessage("\nÖvning raderad!");
                                                    }
                                                    else // Vid misslyckad radering 
                                                    {
                                                        DrawErrorMessage($"\nRadering av övning misslyckades!.");
                                                    }
                                                    break;
                                                case 88: // x för avbryt
                                                    break;
                                            }
                                        }
                                        else DrawErrorMessage("\nRadering av övning misslyckades! Ingen övning med detta id hittades i databasen.");// Övning med id finns ej i db
                                    }
                                    else
                                    {
                                        DrawErrorMessage($"\nIngen övning med index [{deleteIndex}] hittades.");
                                    }
                                }
                            }
                        }
                        else // Inga övningar finns att hämta ut.
                        {
                            DrawErrorMessage("\nDet finns inga övningar att radera.");
                        }
                    }
                    break;
                case '4': // Redigera pass-information
                    break;

                case 88: // Avbryt, backar till inloggad användares meny genom att sätta activeWorkout till null och ladda om.
                    activeWorkout = null;
                    Console.Clear();
                    break;
            }
        }
    }
}