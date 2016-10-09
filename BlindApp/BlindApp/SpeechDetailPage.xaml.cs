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

namespace BlindApp
{
    public partial class SpeechDetailPage : ContentPage
    {
        int counter = 0;
        TextToSpeech speaker;

        public SpeechDetailPage()
        {
            Title = "Porozpravajme sa";
            InitializeComponent ();

            speaker = new TextToSpeech();
        }
        public void marhaEvent(object obj, EventArgs e)
        {
            Debug.WriteLine("aaaa");
        }

        public void OnButtonClicked(object obj, EventArgs e)
        {
            counter += 1;
            if (counter == 4)
            {
                speaker.speakNext("Pred vami sa nachádza schodisko, buďte opatrný");
                counter = 0;
            }
            else if (counter == 3)
            {
                speaker.speakNext("Nerozprávam však veľmi k veci");
            }
            else if (counter == 2)
            {
                speaker.speakNext("Rada sa rozprávam");
            }
            else if (counter == 1)
            {
                speaker.speakNext("Dobrý večer priatelia, volám sa Esmeralda !");
            }


        }
    }
}
