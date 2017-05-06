using BlindApp.Model;
using MathNet.Numerics.LinearAlgebra;
using System;
using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public class BeaconPage : ContentPage
    {
        ListView _list;
       // BeaconsHandler ViewModel;  


        public BeaconPage()
        {
            BackgroundColor = Color.White;
            Title = "Beacon locator";

            //ViewModel = App.BeaconsHandler;
            //ViewModel.ListChanged += (sender, e) =>
            //{
            //    _list.ItemsSource = ViewModel.VisibleData;
            //};

            //BindingContext = ViewModel;

            /*
                        var view = new SKCanvasView
                        {
                            BackgroundColor = Color.Black,
                            HeightRequest = App.ScreenHeight / 6,
                            WidthRequest = App.ScreenWidth,
                        };

                        view.PaintSurface += PaintCanvas;

                        var layout = new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            Spacing = 0,
                            Padding = new Thickness(0, 0, 0, 0),
                            Children = { view, BuildContent() }
                        };*/

            Content = BuildContent();
        }

        private View BuildContent()
        {
            _list = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(typeof(ListItemView)),
                RowHeight = 170,
            };

            _list.SetBinding(ListView.ItemsSourceProperty, "Data");

            return _list;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}