// Klass för användare
// Skapad av Kajsa Classon, HT24.
using System.Data;
using System.Text;
using Microsoft.Data.Sqlite; // Installeras med: 'dotnet add package Microsoft.Data.Sqlite'

namespace ptApp
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        private string? Password { get; set; }

        private PtApp ptApp = new PtApp(); // DB-connection

        // Registrera en ny användare
        public bool RegisterUser(string username, string password)
        {
            using (var connection = new SqliteConnection(ptApp.connectionString))
            {
                PasswordHasher passwordhasher = new PasswordHasher();
                string passwordHash = passwordhasher.Hash(password);

                // Öppna connection
                connection.Open();

                string query = @"INSERT INTO users (username, password)
                                VALUES
                                (@Username, @PasswordHash);";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@Username", username));
                    command.Parameters.Add(new SqliteParameter("@PasswordHash", passwordHash));

                    int result = command.ExecuteNonQuery();
                    if (result >= 1) return true;
                    else return false;
                }
            }
        }

        // Hämta användare för kontroll om användarnamn finns i db
        public bool GetUserByName(string username)
        {
            using (var connection = new SqliteConnection(ptApp.connectionString))
            {
                // Öppna connection
                connection.Open();

                string query = $@"SELECT username FROM users WHERE username=@Username;";
                // Skicka query till db
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@Username", username));

                    using var reader = command.ExecuteReader();
                    if (reader.HasRows) return true;
                    else return false;
                }
            }
        }

        public int? GetUserId(string username)
        {
            using (var connection = new SqliteConnection(ptApp.connectionString))
            {
                // Öppna connection
                connection.Open();

                string query = $@"SELECT userId FROM users WHERE username=@Username;";
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@Username", username));

                    object? result = command.ExecuteScalar();

                    if (result == null) return null;
                    if (result is int userId) return userId;
                    return Convert.ToInt32(result); // Om resultat är av annan typ konvertera till int

                }
            }
        }

        public int? GetUsersTotalWorkoutTime(int userId)
        {
            int totalDuration = 0;
            string query = $@"SELECT SUM(duration) FROM workouts WHERE userId=@id";
            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open();

                using var command = new SqliteCommand(query, connection);
                command.Parameters.Add(new SqliteParameter("@id", userId));

                object? result = command.ExecuteScalar(); // läs ut ett värde från db
                if (result != DBNull.Value) // Om resultatet inte är null från db
                {
                    totalDuration = Convert.ToInt32(result); // konvertera till int
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"Ett fel inträffade vid hämtning av total träningstid: {ex.Message}");
            }
            return totalDuration;
        }

        public List<Workout> GetWorkoutsForUserId(int userId)
        {
            string query = $@"SELECT * FROM workouts WHERE userId=@id ORDER BY date;";
            var workouts = new List<Workout>(); //Variabel för lista med träningspass

            try
            {
                using var connection = new SqliteConnection(ptApp.connectionString);
                connection.Open(); // Öppna db anslutning

                using var command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", userId);

                using var reader = command.ExecuteReader(); // Läs in svar från db
                if (reader.HasRows)
                {
                    while (reader.Read()) // Så länge det finns workouts att läsa ut
                    {
                        string dateString = reader.GetString(reader.GetOrdinal("date"));
                        Workout workout = new Workout // Skapa ett nytt workout-objekt
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("workoutId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                            DateTime = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Duration = reader.GetInt32(reader.GetOrdinal("duration")),
                            Intensity = (Intensity)Enum.Parse(typeof(Intensity), reader.GetString(reader.GetOrdinal("intensity")), true)
                        };
                        workouts.Add(workout); //lägg till i lista
                    }
                }
            }
            // Fånga misslyckad inläsning.
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid inläsning av träningspass: {ex.Message}");
            }

            return workouts;
        }

        // Logga in användare
        public bool LoginUser(string username, string passwordInput)
        {
            try
            {
                using (var connection = new SqliteConnection(ptApp.connectionString))
                {
                    // Öppna connection
                    connection.Open();
                    // Läs in lösenord för användare med valt användarnamn
                    string query = $@"SELECT password FROM users WHERE username=@Username;";
                    // Skicka query till db
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.Add(new SqliteParameter("@Username", username));
                        var retrievedPassword = command.ExecuteScalar() as string;
                        if (!String.IsNullOrEmpty(retrievedPassword))
                        {
                            PasswordHasher passwordhasher = new PasswordHasher();
                            bool result = passwordhasher.Validate(retrievedPassword, passwordInput);
                            return result;
                        }
                        else return false;
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}