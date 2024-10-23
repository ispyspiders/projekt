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
            Enum.TryParse(intensity,true, out enumIntensity);
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
                        command.CommandText = "SELECT last_insert_rowid();";
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                    else
                    {
                        throw new Exception("Inget träningspass registrerades.");
                    }
                    // if (result >= 1) return true;
                    // else return false;
                }
            }
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