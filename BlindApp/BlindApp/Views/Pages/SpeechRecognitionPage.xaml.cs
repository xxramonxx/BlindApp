using BlindApp.Database;
using BlindApp.Database.Tables;
using Java.Text;
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
        public static ISpeechRecognition speechService;
        public static Label labelObject;

        public SpeechRecognitionPage()
        {
            InitializeComponent();
            speechService = DependencyService.Get<ISpeechRecognition>();
            speechService.Initialize();
            labelObject = this.FindByName<Label>("Label1");

            ViewGestures area = this.FindByName<ViewGestures>("Area");
        }

        private void OnTouchDown(object sender, EventArgs args)
        {
           
            if (!speechService.IsListening())
            {
                speechService.Start();
            }
        }

        private void OnTouchUp(object sender, EventArgs args)
        {
            
            if (speechService.IsListening())
            {
                speechService.Stop();
            }
        }
    }

    public class Callback : OnTaskCompleted
    {
        public void onTaskCompleted(IList<string> result)
        {
            if (result != null && result.Count > 0)
            {
                TargetsTable TargetsTable = new TargetsTable(Initialize.DatabaseConnect());

                var Targets = TargetsTable.GetTargetsByName(result[0].RemoveDiacritics());
                StringBuilder StringBuilder = new StringBuilder();
                StringBuilder.Append("Našla som " + Targets.Count);

                if (Targets.Count == 1)
                    StringBuilder.Append(" výsledok\n");
                else if (Targets.Count > 1 && Targets.Count < 5)
                    StringBuilder.Append(" výsledky\n");
                else
                    StringBuilder.Append(" výsledkov\n");

                foreach (var Entry in Targets)
                {
                    StringBuilder.Append(Entry.Employee + " miestnosť " + Entry.Office + "\n");
                }
                Debug.WriteLine(StringBuilder.ToString());
                SpeechRecognitionPage.labelObject.Text = result[0];
                TextToSpeech.speakNext(StringBuilder.ToString());
            }
            else
            {
                TextToSpeech.speak("Nerozoznala som nahrávku");
            }
        }
    }
}
