namespace TodoApp.API.Models
{
    public class Todo
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set;}
        public bool Iscompleted { get; set; }
    }
}
