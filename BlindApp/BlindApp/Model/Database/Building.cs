using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.Globalization;

namespace BlindApp.Model
{
    public static class Building
    {
        public static List<SharedBeacon> Beacons;
        // points of interets //
        public static List<Target> Targets;

        public static List<string> Warnings;
        public static List<string> Info;


        public static void Init()
        {
            Task.Run(() => {
	            var assembly = typeof(Building).GetTypeInfo().Assembly;

	            var uri = "BlindApp.Sources.fiit.fiit_3.svg";

	            Stream stream = assembly.GetManifestResourceStream(uri);
	      
                XDocument doc = XDocument.Load(stream);

	            var root = doc?.Root as XElement;

	            if (root == null) return;

	            InitBeacons(root);
                InitTargets(root);
                InitWarnings(root);
                InitInfos(root);
            });
        }

        private static void InitBeacons(XElement root)
        {
            Beacons = new List<SharedBeacon>();

            var beaconsGroup = root
                .Elements()
                .Where(e => (e.Name.LocalName == "g" && e.Attribute("id").Value == "beacons"));

            var beacons = beaconsGroup.Elements().Elements().Where(e => (e.Name.LocalName == "circle"));

            foreach (var beacon in beacons)
            {
                try
                {
                    Beacons.Add(
                        new SharedBeacon
                        {
                            UID = beacon.Attribute("id").Value,
                            Major = beacon.Attribute("major").Value,
                            Minor = beacon.Attribute("minor").Value,
                            XCoordinate = Convert.ToDouble(beacon.Attribute("cx").Value, CultureInfo.InvariantCulture),
                            YCoordinate = Convert.ToDouble(beacon.Attribute("cy").Value, CultureInfo.InvariantCulture) * -1,
                            ZCoordinate = 0,
                            Floor = beacon.Attribute("major").Value
                        }
                    );
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Warning: attribute not found while parsing SVG: " + e.Message);
                }    
            }
        }

        private static void InitTargets(XElement root)
        {
            Targets = new List<Target>();

            LoadPersons();

            var beaconsGroup = root
             .Elements()
             .Where(e => (e.Name.LocalName == "g" && e.Attribute("id").Value == "targets"));

            var beacons = beaconsGroup.Elements().Where(e => (e.Name.LocalName == "circle"));

            foreach (var target in Targets)
            {
                try
                {
                    var _mapTarget = beacons.SingleOrDefault(x => x.Attribute("room").Value == target.Office);
                    target.XCoordinate = Convert.ToDouble(_mapTarget.Attribute("cx").Value, CultureInfo.InvariantCulture);
                    target.YCoordinate = Convert.ToDouble(_mapTarget.Attribute("cy").Value, CultureInfo.InvariantCulture) * -1;

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error while adding target" + e.Message);
                }
            }
        }

        private static void LoadPersons()
        {
            var assembly = typeof(Building).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("BlindApp.Sources.osoby.csv");
               
            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            //   var lines = Regex.Replace(text, @"[^\S\n]+", "").Split('\n');
            var lines = text.Split('\n');

            foreach (var entry in lines)
            {
                var attributes = entry.Split(';');
                int Room;
                if (attributes[1].Length < 5)
                {
                    Room = int.Parse(attributes[1].Split('.')[1]);
                }
                else
                {
                    Room = 0;
                }
	                Targets.Add(
	                    new Target {
	                        Employee = attributes[0].ToLower().CapitalizeFirstLetters(), // remove accents
	                        EmployeeParsed = attributes[0].ToLower().RemoveDiacritics(),
	                        Office = attributes[1],
	                        Floor = int.Parse(attributes[2]),
	                        Room = Room,
	                        Minor = int.Parse(attributes[3]),
	                }
                );
            }
        }

        private static void InitWarnings(XElement root)
        {


        }

        private static void InitInfos(XElement root)
        {
        }
    }
}
