using System;
using Xamarin.Forms;
using Plugin.Vibrate;

namespace BlindApp
{
    public partial class SpeechDetailPage : ContentPage
    {
        int counter = 0;

        public SpeechDetailPage()
        {
            Title = "Porozpravajme sa";
            InitializeComponent();
        }

        public void OnButtonClicked(object obj, EventArgs e)
        {
            CrossVibrate.Current.Vibration(200);

            counter += 1;
            if (counter == 4)
            {
                TextToSpeech.speak("Pred vami sa nachádza schodisko, buďte opatrný");
                counter = 0;
            }
            else if (counter == 3)
            {
                TextToSpeech.speak("Nerozprávam však veľmi k veci");
            }
            else if (counter == 2)
            {
                TextToSpeech.speak("Rada sa rozprávam");
            }
            else if (counter == 1)
            {
                TextToSpeech.speak("Dobrý večer ctený používateľ, kam chcete ísť ?");
            }
        }
    }
}
