﻿#define DEBUG

using BlindApp;
using BlindApp.Database;
using BlindApp.Database.Tables;
using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using System.Collections;

namespace BlindApp.Model
{
    public static class Map
    {
        public static bool initialized = false;

        private static Dictionary<string, Node<SharedBeacon>> nodeMap;

        public static async void Init()
        {
            nodeMap = new Dictionary<string, Node<SharedBeacon>>();

            //// TODO: Iitialize by first captured beacon group number
        }

        private static void CreateNodes()
        {
            PointsTable beaconsTable = new PointsTable(Initializer.DatabaseConnect());
            var beaconList = beaconsTable.SelectMoreRows("select * from Points");

            foreach (var beacon in beaconList)
            {
                var node = new Node<SharedBeacon>();
                node.Data = beacon;
                nodeMap.Add(beacon.ToString(), node);
            }
        }

        public static Queue<MapPoint> NewFind(Point start, Point target)
        {
            var path = new Queue<MapPoint>();

            var beacons = Building.Beacons;

            SharedBeacon nearestBeacon = GetCurrnetNearestBeacon(target);
            path.Enqueue(nearestBeacon);

            SharedBeacon targetBeacon = null;
            List<string> visited = new List<string>();

            var limit = 0;

            while (targetBeacon == null && limit < 1000)
            {
                visited.Add(nearestBeacon.ToString());
                var currentFitness = EvaluateFitness(start, nearestBeacon.Location, target);
                var radius = 100;
                while (radius < 2000)
                {
                    var beaconsAround = beacons.Where(item =>
                        ((item.Location.X > nearestBeacon.Location.X - radius 
                       && item.Location.X < nearestBeacon.Location.X + radius)
                       && item.Location.Y > nearestBeacon.Location.Y - radius 
                       && item.Location.Y < nearestBeacon.Location.Y + radius)
                       && EvaluateFitness(start, item.Location, target) < currentFitness
                       && !visited.Contains(item.ToString())).ToList();

                    if (beaconsAround.Count > 0)
                    {
                        nearestBeacon = beaconsAround.OrderByDescending(item => EvaluateFitness(start, item.Location, target)).LastOrDefault(); ;
                        path.Enqueue(nearestBeacon);
                        break;
                    }

                    radius += 100;
                }

              //  next
                limit++;
            }
            return path;
        }

        private static SharedBeacon GetCurrnetNearestBeacon(Point target)
        {
            var visibleBeacons = App.BeaconsHandler.VisibleData.Clone<List<SharedBeacon>>().Take(4);
            var position = NavigationHandler.Instance.Position.Location;

            return visibleBeacons.OrderByDescending(x => EvaluateFitness(position, x.Location, target)).LastOrDefault();
        }

        private static double EvaluateFitness(Point start, Point current, Point target)
        {
            // TODO: vzdialenost k cielu ^2 - vzdialenost odomna
            var distanceToTarget = current.Distance(target);


            return distanceToTarget;
        }

        private static void CreateEdges()
        {
            PathsTable pathsTable = new PathsTable(Initializer.DatabaseConnect());

            foreach (var node in nodeMap.Values)
            {
                var pathList = pathsTable.SelectMoreRows("SELECT * from Paths WHERE Start='" + node.Data.ToString() + "' OR End='" + node.Data.ToString() + "'");

                foreach (var path in pathList)
                {
                    var edge = new Edge<SharedBeacon>();
                    var pattern = (path.Start != node.Data.ToString() ? path.Start : path.End);
                    var toNode = new Node<SharedBeacon>();

                    if (nodeMap.TryGetValue(pattern, out toNode))
                    {
                        edge.To = toNode;
                        edge.Price = path.Distance;
                    }
                    node.Neighbours.AddLast(edge);
                }
            }
        }

        public static Node<SharedBeacon> FindUsingHeap(string from, string to)
        {
            InitNodes();
            if (initialized != true)
                throw new Exception("Map not initialized");

            var heap = new Heap<Node<SharedBeacon>>();

            var localNodeMap = new Dictionary<string, Node<SharedBeacon>>(nodeMap);

            localNodeMap[from].PathPrice = 0;
            heap.Add(localNodeMap[from]);

            while (heap.size > 0)
            {
                Node<SharedBeacon> currentPoint = heap.PopMin();
                currentPoint.Visited = true;

                if (currentPoint.Data.ToString() == to)
                {
                    return currentPoint;
                }

                foreach (var edge in currentPoint.Neighbours)
                {
                    var neighbour = edge.To;
                    var price = currentPoint.PathPrice + edge.Price;

                    if (neighbour.Visited != true)
                    {
                        if (neighbour.PathPrice > price)
                        {
                            neighbour.PathPrice = price;
                            neighbour.PreviousNode = currentPoint;
                        }
                        heap.Add(edge.To);
                    }
                }
            }
            return null;
        }

        public static string FindUsingQueue(string from, string to)
        {
            var queue = new Queue<Node<SharedBeacon>>();
            queue.Enqueue(nodeMap[from]);

            while (queue.Count > 0)
            {
                Node<SharedBeacon> currentPoint = queue.Dequeue();
                currentPoint.Visited = true;

                foreach (var edge in currentPoint.Neighbours)
                {
                    var neighbour = edge.To;
                    if (neighbour.Visited != true)
                    {
                        neighbour.PathPrice = currentPoint.PathPrice + edge.Price;

                        if (neighbour.Data.ToString() == to)
                        {
                            return "Found";
                        }

                        queue.Enqueue(edge.To);
                    }
                }
            }

            return "";
        }

        private static void InitNodes()
        {
            foreach (var node in nodeMap.Values)
            {
                node.PreviousNode = null;
                node.PathPrice = int.MaxValue;
                node.Visited = false;
            }
        }
    }
}
