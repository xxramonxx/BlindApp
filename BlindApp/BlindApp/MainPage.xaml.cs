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
    public partial class MainPage : ContentPage
    {
        TextToSpeech speaker;
        public MainPage()
        {
            InitializeComponent();

            speaker = new TextToSpeech();
        }
        public void marhaEvent(object obj, EventArgs e)
        {
            Debug.WriteLine("aaaa");
        }

        public void OnButtonClicked(object obj, EventArgs e)
        {
            speaker.speakNext("mydlil mi barana a dve ovce");

        }
    }
}
