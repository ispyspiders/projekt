// Klass för träningspass i PT-app
// Skapad av Kajsa Classon, HT24.

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
    }
}