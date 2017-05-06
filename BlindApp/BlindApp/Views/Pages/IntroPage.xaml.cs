using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public partial class IntroPage : ContentPage
    {
        public IntroPage()
        {
            InitializeComponent();
            SpeechRecognition.SetContext(this);

        }

        public void PageDetailChange(object obj, EventArgs e)
        {
            if (obj as Button == Person)
            {
                Navigation.PushAsync(new TargetsPage(1));
            }
            if (obj as Button == Room)
            {
                Navigation.PushAsync(new TargetsPage(2));
            }
        }

        private void OnTouchDown(object sender, EventArgs args)
        {
            SpeechRecognition.Start();
        }

        private void OnTouchUp(object sender, EventArgs args)
        {
            SpeechRecognition.Stop();
        }
    }
}
