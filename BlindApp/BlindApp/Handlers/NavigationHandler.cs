using BlindApp.Database.Tables;
using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindApp
{
    public static class NavigationHandler
    {
        public static Position Position = new Position();
        public static Stack<Point> Path { get; set; }
        public static Point NextMilestone
        {
            get { return Path.Count > 0 ? Path.Peek() : new Point(); }
        }
        public static double RemainingMetersNextMilestone;
        public static long RemainingStepsNextMilestone
        {
            get { return Convert.ToInt32(RemainingMetersNextMilestone / STEP_METERS); }
        }

        public static long RemainingSteps
        {
            get { return Convert.ToInt32(RemainingMeters / STEP_METERS); }
        }
        public static float RemainingMeters;

        public static bool DestinationReached;

        private const float STEP_METERS = 1.5f;


        public static void Init()
        {
            Path = new Stack<Point>();
            RemainingMeters = 0;
            DestinationReached = false;
        }

        public static void Find (string from, string to)
        {
            Init();
            var node = Map.FindUsingHeap(from,to);
            if (node == null)
                return;

            RemainingMeters = node.PathPrice;

            // skip najblizsi ? neviem ci chcem aj aktualne najblizsi beacon

            while (node.Data.ToString() != from ) // node != null // ak chcem aj pociatocny
            { 
                Path.Push(node.Data);
                node = node.PreviousNode;
            }

            StartNavigation();
            Debug.WriteLine(Path.Peek().UID);
        }

        private static void StartNavigation()
        {
            Task.Run(() =>
            {
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {


                    
                    return !DestinationReached;
                });
            });
        }
    }
}
