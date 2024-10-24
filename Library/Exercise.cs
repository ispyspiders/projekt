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

        public int RegisterExercise()
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
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    else{
                        throw new Exception("Fel vid registrering av övning.");
                    }
                }

            }


        }
    }

}