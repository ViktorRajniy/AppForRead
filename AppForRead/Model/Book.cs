namespace AppForRead.Model
{
    using SQLite;

    [Table("Book")]
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public string Text { get; set; }
    }
}
