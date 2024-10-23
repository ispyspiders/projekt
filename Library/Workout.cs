// Klass för träningspass i PT-app
// Skapad av Kajsa Classon, HT24.

using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace ptApp
{
    public class Workout
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateTime { get; set; }
        public int Duration { get; set; }
        public Intensity Intensity { get; set; }
        public List<Exercise>? Exercises { get; set; }

        private PtApp ptApp = new PtApp(); // DB-connection

        public Workout(int userId, string datetime, int duration, string intensity)
        {
            UserId = userId;
            DateTime = DateTime.ParseExact(datetime,"d-M-yyyy", CultureInfo.InvariantCulture);
            Duration = duration;
            Intensity enumIntensity;
            Enum.TryParse(intensity, out enumIntensity);
            Intensity = enumIntensity;
        }

        public bool RegisterWorkout()
        {
            using (var connection = new SqliteConnection(ptApp.connectionString))
            {
                string date = DateTime.ToString("dd-MM-yyyy");
                connection.Open(); //Öppna db-anslutning
                string query = $@"INSERT INTO workouts (userId, date, duration, intensity)
                VALUES
                ({UserId}, @Date, {Duration}, @Intensity);";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@Date", date));
                    command.Parameters.Add(new SqliteParameter("@Intensity", Intensity));

                    int result = command.ExecuteNonQuery();
                    if (result >= 1) return true;
                    else return false;
                }
            }
        }

        // Kontrollerar en sträng om den har formatet DD-MM-YYYY
        public static bool CheckIfValidDate(string date)
        {
            string[] dateArr = date.Split('-'); // dela upp datum input för kontroll
            if (dateArr.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                Console.WriteLine($"\nFelaktigt datumformat. Vänligen ange datum i formatet DD-MM-YYYY.");
                Console.ResetColor(); // Återställ textfärg
                return false;
            }
            else
            {
                int day;
                int month;
                int year;
                DateTime dtNow = DateTime.Now;

                // Kontroll om dag går göra om till integer.
                if (!int.TryParse(dateArr[0], out day))
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                    Console.WriteLine("Day is not a number.");
                    Console.ResetColor();
                    return false;
                }
                else
                {
                    // Kontroll om dag har giltigt värde.
                    if (day < 1 || day > 31)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine("Day has to be a number between 1 and 31.");
                        Console.ResetColor();
                        return false;
                    }
                }

                // Kontroll om månad går göra om till integer
                if (!int.TryParse(dateArr[1], out month))
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                    Console.WriteLine("Day is not a number.");
                    Console.ResetColor();
                    return false;
                }
                else
                {
                    if (month < 1 || month > 12)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine("Month has to be a number between 1 and 12.");
                        Console.ResetColor();
                        return false;
                    }
                }

                // Kontroll om år går göra om till integer
                if (!int.TryParse(dateArr[2], out year))
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                    Console.WriteLine("Year is not a number.");
                    Console.ResetColor();
                    return false;
                }
                else
                {
                    // Kontroll att år inte är i framtiden
                    if (year > dtNow.Year)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                        Console.WriteLine($"År kan inte vara större än {dtNow.Year}.");
                        Console.ResetColor();
                        return false;
                    }
                    if (year == dtNow.Year)
                    {
                        if (month > dtNow.Month)
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                            Console.WriteLine($"Datum får inte ligga i framtiden.");
                            Console.ResetColor();
                            return false;
                        }
                        if (month == dtNow.Month)
                        {
                            if (day > dtNow.Day)
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // Sätt textfärg till röd.
                                Console.WriteLine($"Datum får inte ligga i framtiden.");
                                Console.ResetColor(); return false;
                            }
                        }

                    }
                }

                return true;
            }
        }
    }
}