using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        }

        public void PageDetailChange(object obj, EventArgs e)
        {
            Button button = obj as Button;

            if (button == this.FindByName<Button>("Person"))
            {
                Navigation.PushAsync(new TargetsPage(1));
            }
            if (button == this.FindByName<Button>("Room"))
            {
                Navigation.PushAsync(new TargetsPage(2));
            }
        }

        public void OnButtonClicked(object obj, EventArgs e)
        {

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
