﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace BlindApp
{
    public class MainPage : MasterDetailPage
    {
        public static MasterDetailPage masterDetailPage;

        public MainPage()
        {
            masterDetailPage = this;
            Master = new MenuPage();
            Detail = new NavigationPage(new SpeechDetailPage());
        }

        public static void DetailChange(String PageName)
        {
            var switchPage = new Page();
            if (PageName == "SpeechSynthetizer")
            {
                switchPage = new SpeechDetailPage();
            }
            if (PageName == "BeaconLocator")
            {
                switchPage = new BeaconPage();
            }

            masterDetailPage.Detail = new NavigationPage(switchPage);
            masterDetailPage.IsPresented = false;
        }
    }
}