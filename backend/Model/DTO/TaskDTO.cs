namespace backend.Model.DTO
{
    public class TaskDTO : BaseIdentifiableClass
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int? ColumnId { get; set; }
        //    dueDate: "2024-07-28",
    }
}
