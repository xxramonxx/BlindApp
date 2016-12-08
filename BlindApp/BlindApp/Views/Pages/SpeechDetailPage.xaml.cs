using System;
using Xamarin.Forms;
using Plugin.Vibrate;

namespace BlindApp.Views.Pages
{
    public partial class SpeechDetailPage : ContentPage
    {
        int counter = 0;
        Button button;
        public SpeechDetailPage()
        {
            Title = "Porozpravajme sa";
            InitializeComponent();

            button = this.FindByName<Button>("Test");
      //      AccessibilityEffect.SetAccessibilityLabel(button, "Todo item"); // screenreader description
        }

        public void OnButtonClicked(object obj, EventArgs e)
        {
            CrossVibrate.Current.Vibration(200);

            counter += 1;
            if (counter == 4)
            {
                TextToSpeech.Speak("Pred vami sa nachádza schodisko, buďte opatrný");
                counter = 0;
            }
            else if (counter == 3)
            {
                TextToSpeech.Speak("Nerozprávam však veľmi k veci");
            }
            else if (counter == 2)
            {
                TextToSpeech.Speak("Rada sa rozprávam");
            }
            else if (counter == 1)
            {
                TextToSpeech.Speak("Dobrý večer ctený používateľ, kam chcete ísť ?");
            }
        }
    }
}
