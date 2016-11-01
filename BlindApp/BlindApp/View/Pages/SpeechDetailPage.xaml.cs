using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Reflection.Emit;

using Xamarin.Forms;
using Plugin.TextToSpeech.Abstractions;
using Plugin.TextToSpeech;
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
                App.Speaker.speak("Pred vami sa nachádza schodisko, buďte opatrný");
                counter = 0;
            }
            else if (counter == 3)
            {
                App.Speaker.speak("Nerozprávam však veľmi k veci");
            }
            else if (counter == 2)
            {
                App.Speaker.speak("Rada sa rozprávam");
            }
            else if (counter == 1)
            {
                App.Speaker.speak("Dobrý večer ctený používateľ, kam chcete ísť ?");
            }
        }
    }
}
