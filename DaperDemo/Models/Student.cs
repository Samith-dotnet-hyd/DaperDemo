namespace DaperDemo.Models
{
    public class Student
    {
        public int Id { get; set; }              // mapped to Id
        public string Name { get; set; } = "";
        public string? Email { get; set; }
        public DateTime? EnrolledDate { get; set; }
    }
}
