// Klass för träningsapp. Agerar på User, Exercise och Workout
// Skapad av Kajsa Classon, HT24.

using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic; // Installeras med: 'dotnet add package Microsoft.Data.Sqlite'

namespace ptApp
{
    public class PtApp
    {
        const string databaseFile = "ptApp.db"; // Namn för databasfil
        string connectionString = $"Data Source={databaseFile}";

        public PtApp()
        {
            //Skapa DB connection objekt
            using (var connection = new SqliteConnection(connectionString))
            {
                // Öppna connection
                connection.Open();

                // Aktivera foreign keys
                using (var command = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
                {
                    command.ExecuteNonQuery();
                }

                // Skapa tabell om den inte finns USERS
                string createTableQuery =
                @"
                CREATE TABLE IF NOT EXISTS users(
                    userId INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE,
                    password TEXT NOT NULL
                );
            ";

                // Skicka query till db
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Skapa tabell om den inte finns EXERCISES
                createTableQuery =
                @"
                CREATE TABLE IF NOT EXISTS exercises(
                    exerciseId INTEGER PRIMARY KEY AUTOINCREMENT,
                    exerciseName TEXT NOT NULL,
                    description TEXT,
                    reps INTEGER,
                    weight REAL
                    );
                ";

                // Skicka query till db
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Skapa tabell om den inte finns WORKOUTS
                createTableQuery =
                @"
                CREATE TABLE IF NOT EXISTS workouts(
                        workoutId INTEGER PRIMARY KEY AUTOINCREMENT,
                        userId INTEGER NOT NULL,
                        date TEXT NOT NULL,
                        duration INTEGER,
                        intensity TEXT,
                        FOREIGN KEY (userId) REFERENCES users(userId) ON UPDATE CASCADE ON DELETE CASCADE
                    );
                ";

                // Skicka query till db
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Skapa tabell om den inte finns WORKOUTS
                createTableQuery =
                @"
                CREATE TABLE workout_exercises(
                    workout_exerciseId INTEGER PRIMARY KEY AUTOINCREMENT,
                    workoutId INTEGER,
                    exerciseId INTEGER,
                    FOREIGN KEY (workoutId) REFERENCES workouts(workoutId),
                    FOREIGN KEY (exerciseId) REFERENCES exercises(ExerciseId)
                    );
                    ";

                // Skicka query till db
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                
            }
        }
    }
}