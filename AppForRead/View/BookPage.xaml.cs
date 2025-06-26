using AppForRead.DataBase;
using AppForRead.Model;
using System.Net;

namespace AppForRead.View;

public partial class BookPage : ContentPage
{
    private DataBaseService _dataBaseService;

    private bool _isRefreshing;
    public bool IsRefreshing
    {
        get { return _isRefreshing; }
        set { _isRefreshing = value; OnPropertyChanged(); }
    }

    public Book Book { get; set; }

    public int CurrentPage { get; set; } = 1;

    public int PageCount { get; set; } = 0;

    public BookPage(DataBaseService dataBaseService, int bookID)
	{
		InitializeComponent();
        BindingContext = this;

        _dataBaseService = dataBaseService;
        LoadBook(bookID);
        TitleLabel.Text = Title;
    }

    private async void LoadBook(int id)
    {
        try
        {
            IsRefreshing = true;
            Book = await _dataBaseService.GetBookAsync(id);

            PageCount = (int)(Book.Text.Length / Preferences.Get("PageSize", 2000d));

            UpdateView();
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

    private void UpdateView()
    {
        TitleLabel.Text = Book.Title;
        UpdatePage();
    }

    private void UpdatePage()
    {
        var size = (int)Preferences.Get("PageSize", 2000d);
        TextLabel.Text = Book.Text.Substring(0 + (CurrentPage - 1) * size, size);

        PageNumerLabel.Text = CurrentPage + " / " + PageCount;

        if(CurrentPage == 1)
        {
            BackButton.IsEnabled = false;
        }
        else
        {
            BackButton.IsEnabled = true;
        }

        if (CurrentPage == PageCount)
        {
            NextButton.IsEnabled = false;
        }
        else
        {
            NextButton.IsEnabled = true;
        }
    }

    private void Back_Clicked(object sender, EventArgs e)
    {
        if(CurrentPage == 1)
        {
            return;
        }
        else
        {
            CurrentPage--;
            UpdatePage();
        }
    }

    private void Next_Clicked(object sender, EventArgs e)
    {
        if (CurrentPage == PageCount)
        {
            return;
        }
        else
        {
            CurrentPage++;
            UpdatePage();
        }
    }
}