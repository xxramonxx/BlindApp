using System;
using Xamarin.Forms;

namespace BlindApp
{
    public class ListItemView : ViewCell
    {
        public ListItemView()
        {
            View = BuildContent();
        }

        private View BuildContent()
        {
            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Fill,
                Spacing = 0,
                Padding = 0,
            };

            var beaconId = new Label
            {
                Text = "Id",
                TextColor = Color.Black,
                FontFamily = "sans-serif",
                FontSize = 17
            };

            beaconId.SetBinding(Label.TextProperty, "Id");

            var beaconIdLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Spacing = 0,
                Padding = new Thickness(0, 10, 10, 5),
                Children = { beaconId }
            };

            viewLayout.Children.Add(beaconIdLayout);

            var beaconMinor = new Label
            {
                Text = "Minor",
                TextColor = Color.Black,
                FontFamily = "sans-serif",
                FontSize = 17
            };

            beaconMinor.SetBinding(Label.TextProperty, "Minor");

            var beaconMinorLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Spacing = 0,
                Padding = new Thickness(0, 10, 10, 5),
                Children = { beaconMinor }
            };

            viewLayout.Children.Add(beaconMinorLayout);

            var beaconMAC = new Label
            {
                Text = "MAC",
                TextColor = Color.Black,
                FontFamily = "sans-serif",
                FontSize = 17
            };

            beaconMAC.SetBinding(Label.TextProperty, "MAC");

            var beaconMACLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.End,
                Spacing = 0,
                Padding = new Thickness(0, 10, 10, 5),
                Children = { beaconMAC }
            };

            viewLayout.Children.Add(beaconMACLayout);

            var beaconDistance = new Label
            {
                Text = "1.3m",
                TextColor = Color.Black,
                FontFamily = "sans-serif-light",
                FontSize = 36
            };

            beaconDistance.SetBinding(Label.TextProperty, "Distance");

            var beaconDistanceLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.End,
                Spacing = 0,
                Padding = new Thickness(0, 0, 10, 10),
                Children = { beaconDistance }
            };

            viewLayout.Children.Add(beaconDistanceLayout);

            return viewLayout;
        }
    }
}

