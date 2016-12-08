using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BlindApp.Views.Pages
{
    public partial class NavigationProcessPage : ContentPage
    {
        public NavigationProcessPage()
        {
            InitializeComponent();
            Title = "Navigácia";

            SpeechRecognition.SetContext(this);

            Task.Run(() =>
            {
                Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    nextPoint.Text = NavigationHandler.NextMilestone.ToString();

                    remainingSteps.Text = NavigationHandler.RemainingSteps.ToString();

                    xCoordinate.Text = String.Format("{0:#0.000}", NavigationHandler.Position.XCoordinate);
                    yCoordinate.Text = String.Format("{0:#0.000}", NavigationHandler.Position.YCoordinate);

                    return true;
                });
            });
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
