using System;
using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
            Title = "Title";
        }

        public void PageDetailChange(object obj, EventArgs e)
        {
            Button button = obj as Button;

            if (button == this.FindByName<Button>("SpeechSynthetizer"))
            {
                MainPage.DetailChange("SpeechSynthetizer");
            }
            if (button == this.FindByName<Button>("BeaconLocator"))
            {
                MainPage.DetailChange("BeaconLocator");
            }
            if (button == this.FindByName<Button>("SpeechRecognition"))
            {
                MainPage.DetailChange("SpeechRecognition");
            }
        }
    }
}
