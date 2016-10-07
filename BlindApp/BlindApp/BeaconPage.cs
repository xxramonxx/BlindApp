﻿using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace BlindApp
{
    public class BeaconPage : ContentPage
    {
        ListView _list;
        BeaconViewModel _viewModel;

        public BeaconPage()
        {
            BackgroundColor = Color.White;
            Title = "AltBeacon Forms Sample";

            _viewModel = new BeaconViewModel();
            _viewModel.ListChanged += (sender, e) =>
            {
                _list.ItemsSource = _viewModel.Data;
            };

            BindingContext = _viewModel;
            Content = BuildContent();
        }

        private View BuildContent()
        {
            _list = new ListView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(typeof(ListItemView)),
                RowHeight = 90,
            };

            _list.SetBinding(ListView.ItemsSourceProperty, "Data");

            return _list;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.Init();
        }
    }
}