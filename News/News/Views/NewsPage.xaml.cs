using News.Models;
using News.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        NewsGroup newsGroup;
        NewsService service;

        public NewsPage()
        {
            InitializeComponent();
            service = new NewsService();
            newsGroup = new NewsGroup();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Headline.Text = $"Todays {Title} Headlines";
            MainThread.BeginInvokeOnMainThread(async () => { await LoadNews(); });

        }
        private async Task LoadNews()
        {
            try
            {
                await Task.Delay(2000);
                NewsCategory category = (NewsCategory)Enum.Parse(typeof(NewsCategory), Title, true);
                await service.GetNewsAsync(category);
                Task<NewsGroup> t1 = service.GetNewsAsync(category);
                var items = t1.Result.Articles;
                NewsList.ItemsSource = items;
                Activity.IsRunning = false;

            }
            catch (Exception)
            {
                await DisplayAlert("Error!", "Could not load news", "OK");
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            Activity.IsRunning = true;
            await LoadNews();
        }

        private async void NewsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var newsPage = (NewsItem)e.Item;
                await Navigation.PushAsync(new ArticleView(newsPage.Url));
            }
            catch (Exception)
            {
                await DisplayAlert("Error!", "Could not load this specifik news", "OK");
            }
        }
    }
}