using BlindApp.Database.Tables;
using System.Collections.Generic;
using Xamarin.Forms;
using BlindApp.Database;
using System;
using System.Linq;
using System.Diagnostics;
using BlindApp.Model;

namespace BlindApp.Views.Pages
{
    public partial class TargetsPage : ContentPage
    {
        public static ListView ListViewObject;
        public static int ChoiceFlag;

        public TargetsPage(int Flag)
        {
            Map.Init();
            // Building.Init();

            InitializeComponent();
            SpeechRecognition.SetContext(this);

            ListViewObject = this.FindByName<ListView>("ListView");

            var cell = new DataTemplate(typeof(TextCell));
            cell.SetValue(TextCell.TextColorProperty, "Black");
            cell.SetValue(TextCell.DetailColorProperty, "Black");

            ChoiceFlag = Flag;
            if (ChoiceFlag == 1) {
                Title = "Vyhľadávanie osoby";
                cell.SetBinding(TextCell.TextProperty, "Employee");
            }
            else
            {
                Title = "Vyhľadávanie miestnosti";
                cell.SetBinding(TextCell.TextProperty, "Office");
            }

            ListViewObject.ItemTemplate = cell;

            TargetsTable TargetsTable = new TargetsTable(Initializer.DatabaseConnect());
            List<Target> Targets;

            if (ChoiceFlag == 1)
            {
                // Targets = TargetsTable.SelectMoreRows("SELECT *,substr(EmployeeParsed, 1, instr(EmployeeParsed, ' ') - 1) AS first_name, substr(EmployeeParsed, instr(EmployeeParsed, ' ') + 1) AS last_name from Targets ORDER BY last_name");
                Targets = Building.Targets.OrderBy(x => x.EmployeeParsed.Split(' ').Last()).ToList();
                ListViewObject.ItemsSource = Targets;
            }
            else
            {
                Targets = Building.Targets.OrderBy(x => x.Office).ToList();
                // remove duplicity
                ListViewObject.ItemsSource = Targets.GroupBy(x => x.Office).Select(y => y.First());
            }

            ListViewObject.ItemSelected += (sender, e) =>
            {
                var selection = sender as ListView;
                ProcessSelection(selection.SelectedItem as Target);
            };
        }

        public async void ProcessSelection(Target Target)
        {
            var endp =  Target.GetNearestEndpoint();

            TextToSpeech.Speak("Navigujem k cieľu " + Target.Employee + " miestnosť " + Target.Office);

            NavigationHandler.Find(Target);

            await Navigation.PushAsync(new NavigationProcessPage());
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
