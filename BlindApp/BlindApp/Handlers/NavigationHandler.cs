using BlindApp.Database.Tables;
using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BlindApp
{
    public static class NavigationHandler
    {
        public static Position Position = new Position();
        public static Queue<SharedBeacon> Path { get; set; }
        public static SharedBeacon NextMilestone
        {
            get { return Path.Count > 0 ? Path.Peek() : new SharedBeacon(); }
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
        public static double RemainingMeters;

        public static bool DestinationReached;

        private const double STEP_METERS = 1.5f;


        public static void Init()
        {
            RemainingMeters = 0;
            DestinationReached = false;

            // var result = Map.NewFind(new Point(5000.0, -1000.0), new Point(9584, -880));
        }

        public static void Find (Target target)
        {
            Init();

#if DEBUG

            Position.XCoordinate = 4800;
            Position.YCoordinate = -1000;

#endif

            var path = Map.NewFind(Position.Location, target.Location);

            // HACK target ako beacon
            path.Enqueue(new SharedBeacon{
                XCoordinate = target.Location.X,
                YCoordinate = target.Location.Y
            });

            if (path.Count == 0) return;

            RemainingMeters = GetPathDistance(path);

            Path = path.Clone();

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

        private static double GetPathDistance(Queue<SharedBeacon> path)
        {
            double distance = 0;

            Point current;
            var next = path.Dequeue().Location;
            distance += Position.Location.Distance(next);

            current = next;
            while(path.Count > 0)
            {
                distance += path.Peek().Location.Distance(current);
                current = path.Dequeue().Location;
            }

            return distance / 100;
        }
    }
}
