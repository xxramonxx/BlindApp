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
            BindingContext = NavigationHandler.Instance;

            SpeechRecognition.SetContext(this);

            //Task.Run(() =>
            //{
                //Device.StartTimer(TimeSpan.FromSeconds(1.5), () =>
                //{
                    //nextPoint.Text = NavigationHandler.Instance.NextMilestone.ToString();

                    //remainingSteps.Text = NavigationHandler.Instance.RemainingSteps.ToString();

                    //xCoordinate.Text = String.Format("{0:#0.00}", NavigationHandler.Instance.Position.XCoordinate);
                    //yCoordinate.Text = String.Format("{0:#0.00}", NavigationHandler.Instance.Position.YCoordinate);

               //     rotation = NavigationHandler.Instance.Position.Rotation.ToString();

                //    return true;
                //});
            //});

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
