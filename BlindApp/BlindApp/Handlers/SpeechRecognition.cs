using BlindApp.Database;
using BlindApp.Database.Tables;
using BlindApp.Model;
using BlindApp.Views.Pages;
using Java.Text;
using ScnViewGestures.Plugin.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BlindApp
{
    class SpeechRecognition
    {
        private static string CurrentContext;
        private static ISpeechRecognition SpeechService;

        public static void Init()
        {
            SpeechService = DependencyService.Get<ISpeechRecognition>();
            SpeechService.Initialize();
        }

        internal static void Start()
        {
            if (!SpeechService.IsListening())
            {
                SpeechService.Start();
            }
        }

        internal static void Stop()
        {
            if (SpeechService.IsListening())
            {
                SpeechService.Stop();
            }
        }

        public static void SetContext(Page Context)
        {
            CurrentContext = Context.GetType().FullName;
        }

        public static string GetContext()
        {
            return CurrentContext;
        }
    }

    public class Callback : OnTaskCompleted
    {
        public void onTaskCompleted(IList<string> result)
        {
            if (result != null && result.Count > 0)
            {
                var currentContext = SpeechRecognition.GetContext();
                if (currentContext == typeof(SpeechRecognitionPage).FullName)
                {
                    SpeechRecognitionPageTask(result);
                }
                else if (currentContext == typeof(IntroPage).FullName)
                {
                    IntroPageTask(result);
                }
                else if (currentContext == typeof(TargetsPage).FullName)
                {
                    TargetsPageTask(result);
                }
                else
                {
                    TextToSpeech.Speak("Neznámy konktext vstupu");
                }  
            }
            else
            {
                TextToSpeech.Speak("Nerozoznala som nahrávku");
            }
        }

        private void SpeechRecognitionPageTask(IList<string> result)
        {
            TargetsTable TargetsTable = new TargetsTable(Initializer.DatabaseConnect());

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

        private void IntroPageTask(IList<string> result)
        {                              
            if (result[0].Contains("osob"))
            {
                MainPage.MasterDetailPage.Detail.Navigation.PushAsync(new TargetsPage(1)); // TODO: Refactoring
            }
            else if (result[0].Contains("miestnos") || result[0].Contains("kancel"))
            {
                MainPage.MasterDetailPage.Detail.Navigation.PushAsync(new TargetsPage(2));
            }
            else
            {
                TextToSpeech.Speak("Nerozoznala som nahrávku");
            }
        }

        private void TargetsPageTask(IList<string> result)
        {

            TargetsTable TargetsTable = new TargetsTable(Initializer.DatabaseConnect());
            List<Target> Targets;

            if (result[0].Contains("navig")) {
                var a = TargetsPage.ListViewObject.ItemsSource as List<Target>;
                TargetsPage.ListViewObject.SelectedItem = a.First();
                return;
            }

            if (TargetsPage.ChoiceFlag == 1)
                Targets = TargetsTable.GetTargetsByName(result[0].RemoveDiacritics());
            else
                Targets = TargetsTable.GetTargetsByOffice(result[0].RemoveDiacritics());

            if (Targets.Count == 0 && (result[0].Contains("všetk") || result[0].Contains("zruš")))
            {
                // read all data
                if (TargetsPage.ChoiceFlag == 1)
                {
                    Targets = TargetsTable.SelectMoreRows("SELECT *,substr(EmployeeParsed, 1, instr(EmployeeParsed, ' ') - 1) AS first_name, substr(EmployeeParsed, instr(EmployeeParsed, ' ') + 1) AS last_name from Targets ORDER BY last_name");
                }
                else
                {
                    Targets = TargetsTable.SelectMoreRows("SELECT * from Targets ORDER BY Office");
                    Targets = Targets.GroupBy(x => x.Office).Select(y => y.First()).ToList();
                }
            }
            else
            {
                StringBuilder StringBuilder = new StringBuilder();
                StringBuilder.Append("Našla som " + Targets.Count);

                if (Targets.Count == 1)
                    StringBuilder.Append(" výsledok\n");
                else if (Targets.Count > 1 && Targets.Count < 5)
                    StringBuilder.Append(" výsledky\n");
                else
                    StringBuilder.Append(" výsledkov\n");

                for (var i = 0; i < Targets.Count && i < 3; i++)
                {
                    if (TargetsPage.ChoiceFlag == 1)
                        StringBuilder.Append(Targets[i].Employee + " miestnosť " + Targets[i].Office + "\n");
                    else
                        StringBuilder.Append("Miestnosť " + Targets[i].Office + "\n");
                }

                if (Targets.Count >= 3)
                {
                    StringBuilder.Append("a ďalšie");
                }

                TextToSpeech.speakNext(StringBuilder.ToString());
            }

            TargetsPage.ListViewObject.ItemsSource = Targets;
        }
    }
}
