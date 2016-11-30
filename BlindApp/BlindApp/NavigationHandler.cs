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
        public static Stack<Point> Path { get; set; }
        public static Point NextMilestone
        {
            get { return Path.Peek(); }
        }
        public static long remainingSteps
        {
            get { return Convert.ToInt32(remainingMeters / STEP_METERS); }
        }
        public static float remainingMeters;

        private const float STEP_METERS = 1.5f;

        public static void Find (string from, string to)
        {
            Path = new Stack<Point>();
            var node = Map.FindUsingHeap(from,to);
            remainingMeters = node.PathPrice;

            while (node.PreviousNode != null) { 
            // Mozno iba node ? neviem ci chcem aj aktualne najblizsi beacon
                Path.Push(node.Data);
                node = node.PreviousNode;
            }
            Debug.WriteLine(Path.Peek().UID);
        }
    }
}
