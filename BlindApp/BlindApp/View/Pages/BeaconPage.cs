using Xamarin.Forms;

namespace BlindApp
{
    public class BeaconPage : ContentPage
    {
        ListView _list;
        BeaconViewModel ViewModel;

        public BeaconPage()
        {
            BackgroundColor = Color.White;
            Title = "Beacon locator";

            ViewModel = new BeaconViewModel();
            ViewModel.ListChanged += (sender, e) =>
            {
                _list.ItemsSource = ViewModel.Data;
            };

            BindingContext = ViewModel;
            Content = BuildContent();
        }

        private View BuildContent()
        {
            _list = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(typeof(ListItemView)),
                RowHeight = 200,
            };

            _list.SetBinding(ListView.ItemsSourceProperty, "Data");

            return _list;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.Init();
        }
    }
}