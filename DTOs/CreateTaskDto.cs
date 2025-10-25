namespace backend.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public DateTime? DueDate { get; set; }
        public int? DependsOnTaskId { get; set; }
        public int EstimatedHours { get; set; } = 1;
    }
}
