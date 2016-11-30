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
        private ListView ListViewObject;

        public TargetsPage(int ChoiceFlag)
        {
            Model.Map.Init();

            InitializeComponent();
            ListViewObject = this.FindByName<ListView>("ListView");

            var cell = new DataTemplate(typeof(TextCell));
            cell.SetValue(TextCell.TextColorProperty, "Black");
            cell.SetValue(TextCell.DetailColorProperty, "Black");


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

            TargetsTable TargetsTable = new TargetsTable(Initialize.DatabaseConnect());

            var Targets = TargetsTable.SelectMoreRows("select * from Targets");

            var DataList = new List<Target>();
            foreach (var Entry in Targets)
            {
                DataList.Add(Entry);
            }


            if (ChoiceFlag == 1)
            {
                DataList.Sort((x, y) =>
                {
                    if (string.Compare(x.Employee, y.Employee, StringComparison.CurrentCulture) < 0) return -1;
                    if (x.Employee == y.Employee) return 0;
                    return 1;
                });
            }
            else
            {
                DataList.Sort((x, y) =>
                {
                    if (string.Compare(x.Office, y.Office, StringComparison.CurrentCulture) < 0) return -1;
                    if (x.Office == y.Office) return 0;
                    return 1;
                });

            }
            // remove duplicity
            ListViewObject.ItemsSource = DataList.GroupBy(x => x.Office).Select(y => y.First()); ;

            ListViewObject.ItemSelected += async (sender, e) =>
            {
                var selection = sender as ListView;
                var entry = selection.SelectedItem as Target;

                var endp = entry.GetNearestEndpoint();

                NavigationHandler.Find("DEADBEEF-CA1F-BABE-FEED-FEEDC0DEFACE-0-38", entry.GetNearestEndpoint());
                
                await DisplayAlert("Selected", entry.Employee + " was selected.", "OK");
            };
        }

        private void OnTouchDown(object sender, EventArgs args)
        {

            Debug.WriteLine("Down");
        }

        private void OnTouchUp(object sender, EventArgs args)
        {
            Debug.WriteLine("Up");

        }
    }
}
