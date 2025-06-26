namespace AppForRead.DataBase
{
    using AppForRead.Model;
    using SQLite;
    using System.Diagnostics;

    public class DataBaseService
    {
        const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        private SQLiteAsyncConnection _database;

        public DataBaseService()
        {
            InitializeDatabase();
        }

        private async Task InitializeDatabase()
        {
            try
            {
                if (_database != null) return;

                _database = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "books.db3"));
                await _database.CreateTableAsync<Book>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database error:{ex.Message}");
            }
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            await InitializeDatabase();
            return await _database.Table<Book>().OrderByDescending(n => n.Id).ToListAsync();
        }

        public async Task<Book> GetBookAsync(int id)
        {
            await InitializeDatabase();
            return await _database.Table<Book>().Where(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveBookAsync(Book book)
        {
            await InitializeDatabase();

            if (book.Id != 0)
            {
                return await _database.UpdateAsync(book);
            }
            else
            {
                return await _database.InsertAsync(book);
            }
        }

        public async Task<int> DeleteBookAsync(Book book)
        {
            await InitializeDatabase();
            return await _database.DeleteAsync(book);
        }
    }
}
