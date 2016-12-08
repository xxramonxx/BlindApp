using System;

using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public class MainPage : MasterDetailPage
    {
        public static MasterDetailPage MasterDetailPage;

        public MainPage()
        {
            MasterDetailPage = this;
            Master = new MenuPage();
            Detail = new NavigationPage(new IntroPage());
        }

        public static void DetailChange(string PageName)
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
            if (PageName == "SpeechRecognition")
            {
                switchPage = new SpeechRecognitionPage();
            }

            MasterDetailPage.Detail = new NavigationPage(switchPage);
            MasterDetailPage.IsPresented = false;
        }
    }
}
