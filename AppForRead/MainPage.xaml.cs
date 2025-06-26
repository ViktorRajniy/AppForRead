using AppForRead.DataBase;
using AppForRead.Model;
using AppForRead.View;
using System.Collections.ObjectModel;

namespace AppForRead
{
    public partial class MainPage : ContentPage
    {
        private readonly DataBaseService _dataBaseService;

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { _isRefreshing = value; OnPropertyChanged(); }
        }

        private List<Book> _books = [];
        public List<Book> Books
        {
            get { return _books; }
            set { _books = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Book> ShownBooks = [];

        public MainPage(DataBaseService dataBaseService)
        {
            InitializeComponent();
            _dataBaseService = dataBaseService;
            LoadBooks();
            UpdateShownBooks(Books);
            BooksListView.ItemsSource = ShownBooks;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadBooks();
        }
        public async Task LoadBooks()
        {
            try
            {
                IsRefreshing = true;
                Books = await _dataBaseService.GetBooksAsync();
                UpdateShownBooks(Books);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Fail to load notes", "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private void UpdateShownBooks(List<Book> newNotes)
        {
            ShownBooks.Clear();
            if (newNotes != null)
            {
                foreach (var note in newNotes)
                {
                    ShownBooks.Add(note);
                }
            }
        }

        private async void BooksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Book selectedBook)
            {
                await Navigation.PushAsync(new BookPage(_dataBaseService, selectedBook.Id));
                BooksListView.SelectedItem = null;
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var search = e.NewTextValue?.ToLower();
            if (search.Equals(string.Empty))
            {
                UpdateShownBooks(Books);
            }
            else
            {
                var filteredNotes = Books
                    .Where(n => n.Title.ToLower().Contains(search))
                    .ToList();
                UpdateShownBooks(filteredNotes);
            }
        }

        private async void SettingsButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
    }
}
