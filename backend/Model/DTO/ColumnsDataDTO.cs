namespace backend.Model.DTO
{
    public class ColumnsDataDTO : BaseIdentifiableClass
    {
        public string ColumnTitle { get; set; } = string.Empty;
        public List<TaskDTO> Tasks { get; set; } = new List<TaskDTO>();
    }
}
