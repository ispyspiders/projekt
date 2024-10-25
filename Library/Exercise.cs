// Klass för enskild övning i PT-app
// Skapad av Kajsa Classon, HT24.

using Microsoft.Data.Sqlite;

namespace ptApp
{
    public class Exercise
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Reps { get; set; }
        public int? Weight { get; set; }

        private PtApp ptApp = new PtApp(); // DB-connection

        public Exercise()
        {

        }

        public Exercise(string name, int reps, int weight)
        {
            Name = name;
            Description = "";
            Reps = reps;
            Weight = weight;
        }

        public Exercise? GetExerciseInfo(int exerciseId)
        {
            string query = @"SELECT * FROM exercises WHERE exerciseId= @ExerciseId;";
            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open();
                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@ExerciseId", exerciseId);
                using var reader = command.ExecuteReader(); // läs in svar från db
                if (reader.Read())
                {
                    Exercise exercise = new Exercise
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("exerciseId")),
                        Name = reader.GetString(reader.GetOrdinal("exerciseName")),
                        Description = reader.GetString(reader.GetOrdinal("description")),
                        Reps = reader.GetInt32(reader.GetOrdinal("reps")),
                        Weight = reader.GetInt32(reader.GetOrdinal("weight"))
                    };
                    return exercise;
                }
                else return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid inläsning av övning: {ex.Message}");
                return null;
            }
        }

        public int RegisterExercise(int workoutId)
        {
            using (var connection = new SqliteConnection(ptApp.connectionString))
            {
                connection.Open(); // Öppna db-anslutning

                string query = @"INSERT INTO exercises (exerciseName, description, reps, weight)
                VALUES
                (@ExerciseName, @Description, @Reps, @Weight);";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@ExerciseName", Name));
                    command.Parameters.Add(new SqliteParameter("@Description", Description));
                    command.Parameters.Add(new SqliteParameter("@Reps", Reps));
                    command.Parameters.Add(new SqliteParameter("@Weight", Weight));

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        command.CommandText = "SELECT last_insert_rowid();";
                        int exerciseId = Convert.ToInt32(command.ExecuteScalar());

                        // Registrera relation i workout_exercises
                        string relationQuery = @"INSERT INTO workout_exercises (workoutId, exerciseId)
                        VALUES (@WorkoutId, @ExerciseId);";

                        using (var relationCommand = new SqliteCommand(relationQuery, connection))
                        {
                            relationCommand.Parameters.Add(new SqliteParameter("@WorkoutId", workoutId));
                            relationCommand.Parameters.Add(new SqliteParameter("@ExerciseId", exerciseId));

                            relationCommand.ExecuteNonQuery(); // lägg till relation
                        }
                        return exerciseId; // returnera id för tillagt övning
                    }
                    else
                    {
                        throw new Exception("Fel vid registrering av övning.");
                    }
                }
            }
        }

        public bool UpdateExercise(Exercise exercise)
        {
            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open(); // öppna db-anslutning
                string query = @"UPDATE exercises
                SET exerciseName = @ExerciseName,
                    description = @Description,
                    reps = @Reps,
                    weight = @Weight
                WHERE exerciseId = @ExerciseId;
                ";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseName", exercise.Name);
                    command.Parameters.AddWithValue("@Description", exercise.Description);
                    command.Parameters.AddWithValue("@Reps", exercise.Reps);
                    command.Parameters.AddWithValue("@Weight", exercise.Weight);
                    command.Parameters.AddWithValue("@ExerciseId", exercise.Id);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid uppdatering av exercise: {ex.Message}");
                return false;
            }
        }
    }


}