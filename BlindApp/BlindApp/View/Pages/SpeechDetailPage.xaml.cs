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
        TextToSpeech speaker;

        public SpeechDetailPage()
        {
            Title = "Porozpravajme sa";
            InitializeComponent();

            speaker = new TextToSpeech();
        }

        public async void OnButtonClicked(object obj, EventArgs e)
        {
            Button button = obj as Button;
            Debug.WriteLine(button.IsFocused);
            CrossVibrate.Current.Vibration(200);

            counter += 1;
            if (counter == 4)
            {
                speaker.speak("Pred vami sa nachádza schodisko, buďte opatrný");
                counter = 0;
            }
            else if (counter == 3)
            {
                speaker.speak("Nerozprávam však veľmi k veci");
            }
            else if (counter == 2)
            {
                speaker.speak("Rada sa rozprávam");
            }
            else if (counter == 1)
            {
                speaker.speak("Dobrý večer ctený používateľ, kam chcete ísť ?");
            }
            await Task.Delay(1000);
            Debug.WriteLine(button.IsFocused);
        }
    }
}
