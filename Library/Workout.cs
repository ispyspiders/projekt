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

        public Workout()
        {

        }
        public Workout(int userId, string datetime, int duration, string intensity)
        {
            UserId = userId;
            DateTime = DateTime.ParseExact(datetime, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            Duration = duration;
            Intensity enumIntensity;
            Enum.TryParse(intensity, true, out enumIntensity);
            Intensity = enumIntensity;
        }

        public int RegisterWorkout()
        {
            using (var connection = new SqliteConnection(ptApp.connectionString))
            {
                string date = DateTime.ToString("dd-MM-yyyy");
                connection.Open(); //Öppna db-anslutning
                string query = $@"INSERT INTO workouts (userId, date, duration, intensity)
                VALUES
                (@UserId, @Date, @Duration, @Intensity);";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@UserId", UserId));
                    command.Parameters.Add(new SqliteParameter("@Date", date));
                    command.Parameters.Add(new SqliteParameter("@Duration", Duration));
                    command.Parameters.Add(new SqliteParameter("@Intensity", Intensity));

                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        command.CommandText = "SELECT last_insert_rowid();"; // Läs ut id för den senast tillagda raden
                        return Convert.ToInt32(command.ExecuteScalar()); // Returnera id för den skapade raden
                    }
                    else
                    {
                        throw new Exception("Inget träningspass registrerades.");
                    }
                }
            }
        }

        public bool DeleteWorkout(int workoutId)
        {
            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open(); // öppna db-anslutning
                string query = @"DELETE FROM workouts WHERE workoutId=@WorkoutId;";
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@WorkoutId", workoutId);
                int result = command.ExecuteNonQuery();
                return result > 0; // Om resultatet är mer än 0 returneras true, är det 0 returneras false
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid radering av workout: {ex.Message}");
                return false;
            }
        }

        public Workout? GetWorkoutInfo(int workoutId)
        {
            string query = @"SELECT * FROM workouts WHERE workoutId=@WorkoutId;";
            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open(); // Öppna db anlutning

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@WorkoutId", workoutId);
                using var reader = command.ExecuteReader(); //Läs in svar från db
                if (reader.Read())
                {
                    string dateString = reader.GetString(reader.GetOrdinal("date"));
                    Workout workout = new Workout
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("workoutId")),
                        UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                        DateTime = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                        Duration = reader.GetInt32(reader.GetOrdinal("duration")),
                        Intensity = (Intensity)Enum.Parse(typeof(Intensity), reader.GetString(reader.GetOrdinal("intensity")), true)
                    };
                    return workout;
                }
                else
                {
                    return null; // Inget pass hittades
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid inläsning av passinformation: {ex.Message}");
                return null;
            }
        }

        public List<Exercise> GetExercisesForWorkout(int workoutId)
        {
            var exercises = new List<Exercise>(); //Lista med exercise-objekt

            // Query: Hämta ut id, namn, reps och vikt från exercises, för valt workoutId. Där exerciseId i exercises motsvarar  exerciseId i workout_exercises
            string query = @"
            SELECT e.exerciseId, e.exerciseName, e.reps, e.weight
            FROM exercises e
            JOIN workout_exercises w_e
            ON e.exerciseId = w_e.exerciseId
            WHERE w_e.workoutId = @WorkoutId
            ;";

            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open(); // Öppna db anlutning

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@WorkoutId", workoutId);

                using var reader = command.ExecuteReader(); //läs in svar från db
                if (reader.HasRows)
                {
                    while (reader.Read())
                    { // så länge det finns övningar att läsa ut
                        Exercise exercise = new Exercise // Skapa nytt övnings-objekt
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("exerciseId")),
                            Name = reader.GetString(reader.GetOrdinal("exerciseName")),
                            Reps = reader.GetInt32(reader.GetOrdinal("reps")),
                            Weight = reader.GetInt32(reader.GetOrdinal("weight"))
                        };
                        exercises.Add(exercise); // Lägg till övning i lista med övningar
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid inläsning av övningar: {ex.Message}");
            }
            return exercises;
        }

        // Kontrollerar en sträng om den har formatet DD-MM-YYYY
        public static bool CheckIfValidDate(string date)
        {
            string format = "dd-MM-yyyy";
            DateTime parsedDate;

            bool isValidFormat = DateTime.TryParseExact(date, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate);
            bool isNotInFuture = parsedDate <= DateTime.Now;

            return isValidFormat && isNotInFuture;
        }

        public static bool CheckIfValidIntensity(string input)
        {
            return Enum.TryParse<Intensity>(input, true, out _);
        }
    }
}