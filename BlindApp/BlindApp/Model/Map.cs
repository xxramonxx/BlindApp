using BlindApp;
using BlindApp.Database;
using BlindApp.Database.Tables;
using BlindApp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlindApp.Model
{
    public static class Map
    {
        public static bool initialized = false;

        private static Dictionary<string, Node<Point>> nodeMap;

        public static async void Init()
        {
            nodeMap = new Dictionary<string, Node<Point>>();

            // TODO: Iitialize by first captured beacon group number
            await Task.Run(() =>
            {

                CreateNodes();
                CreateEdges();
                // do lot of work here
                initialized = true;
            });
            //        Find("DEADBEEF-CA1F-BABE-FEED-FEEDC0DEFACE-0-42", "DEADBEEF-CA1F-BABE-FEED-FEEDC0DEFACE-0-38"); 
            //  FindUsingHeap("DEADBEEF-CA1F-BABE-FEED-FEEDC0DEFACE-0-13", "B9407F30-F5F8-466E-AFF9-25556B57FE6D-36295-11950");
        }

        private static void CreateNodes()
        {
            PointsTable beaconsTable = new PointsTable(Initializer.DatabaseConnect());
            var beaconList = beaconsTable.SelectMoreRows("select * from Points");

            foreach (var beacon in beaconList)
            {
                var node = new Node<Point>();
                node.Data = beacon;
                nodeMap.Add(beacon.ToString(), node);
            }
        }

        private static void CreateEdges()
        {
            PathsTable pathsTable = new PathsTable(Initializer.DatabaseConnect());

            foreach (var node in nodeMap.Values)
            {
                var pathList = pathsTable.SelectMoreRows("SELECT * from Paths WHERE Start='" + node.Data.ToString() + "' OR End='" + node.Data.ToString() + "'");

                foreach (var path in pathList)
                {
                    var edge = new Edge<Point>();
                    var pattern = (path.Start != node.Data.ToString() ? path.Start : path.End);
                    var toNode = new Node<Point>();

                    if (nodeMap.TryGetValue(pattern, out toNode))
                    {
                        edge.To = toNode;
                        edge.Price = path.Distance;
                    }
                    node.Neighbours.AddLast(edge);
                }
            }
        }

        public static Node<Point> FindUsingHeap(string from, string to)
        {
            InitNodes();
            if (initialized != true)
                throw new Exception("Map not initialized");

            var heap = new Heap<Node<Point>>();

            var localNodeMap = new Dictionary<string, Node<Point>>(nodeMap);

            localNodeMap[from].PathPrice = 0;
            heap.Add(localNodeMap[from]);

            while (heap.size > 0)
            {
                Node<Point> currentPoint = heap.PopMin();
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
            var queue = new Queue<Node<Point>>();
            queue.Enqueue(nodeMap[from]);

            while (queue.Count > 0)
            {
                Node<Point> currentPoint = queue.Dequeue();
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

            return "hovno hovno zlata rybka";
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
