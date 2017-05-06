using ScnViewGestures.Plugin.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public partial class SpeechRecognitionPage : ContentPage
    {
        public static Label labelObject;

        public SpeechRecognitionPage()
        {
            InitializeComponent();
            SpeechRecognition.SetContext(this);
      
            labelObject = this.FindByName<Label>("Label1");

            ViewGestures area = this.FindByName<ViewGestures>("Area");
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
