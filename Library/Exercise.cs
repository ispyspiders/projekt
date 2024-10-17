// Klass för enskild övning i PT-app
// Skapad av Kajsa Classon, HT24.

namespace ptApp
{
    public class Exercise
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Reps { get; set; }
        public int? Weight { get; set; }
    }
}