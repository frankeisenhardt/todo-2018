namespace TodoApi.Models
{ 
    public class TodoItem
    {
        public string _id { get; set; }
		public string _rev { get; set; }
        public string type { get; set; }
		public string text { get; set; }
		public bool done { get; set; }
	}

    public class PostTodoItem
    {
		public string type { get; set; }
		public string text { get; set; }
		public bool done { get; set; }
    }
}
