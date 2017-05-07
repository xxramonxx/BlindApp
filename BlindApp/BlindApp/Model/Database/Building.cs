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
        public static List<Info> Info;


        public static void Init()
        {
            var assembly = typeof(Building).GetTypeInfo().Assembly;

            //var uri = "BlindApp.Sources.fiit.fiit_3.svg";
         	var uri = "BlindApp.Sources.fiit.fiit_1.svg";

            Stream stream = assembly.GetManifestResourceStream(uri);
      
            XDocument doc = XDocument.Load(stream);

            var root = doc?.Root as XElement;

            if (root == null) return;

            InitBeacons(root);
            InitTargets(root);
            InitWarnings(root);
            InitInfos(root);
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
							Major = "0", //beacon.Attribute("major").Value,
                            Minor = beacon.Attribute("minor").Value,
                            XCoordinate = Convert.ToDouble(beacon.Attribute("cx").Value, CultureInfo.InvariantCulture),
                            YCoordinate = Convert.ToDouble(beacon.Attribute("cy").Value, CultureInfo.InvariantCulture) * -1,
                            ZCoordinate = 0,
                            Floor = "0"//beacon.Attribute("major").Value
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
					var _mapTarget = beacons.FirstOrDefault(x => x.Attribute("room").Value == target.Office);
					if (_mapTarget != null)
					{

						target.XCoordinate = Convert.ToDouble(_mapTarget.Attribute("cx").Value, CultureInfo.InvariantCulture);
						target.YCoordinate = Convert.ToDouble(_mapTarget.Attribute("cy").Value, CultureInfo.InvariantCulture) * -1;
					}
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
	                        Floor = attributes[2],
	                        Room = Room
	                }
                );
            }
        }

        private static void InitWarnings(XElement root)
        {


        }

        private static void InitInfos(XElement root)
        {
			Info = new List<Info>();

            var beaconsGroup = root
				.Elements()
				.Where(e => (e.Name.LocalName == "g" && e.Attribute("id").Value == "info"));

			var beacons = beaconsGroup.Elements().Where(e => (e.Name.LocalName == "circle"));

			foreach (var beacon in beacons)
			{
				try
				{
					Info.Add(
						new Info
						{
							XCoordinate = Convert.ToDouble(beacon.Attribute("cx").Value, CultureInfo.InvariantCulture),
							YCoordinate = Convert.ToDouble(beacon.Attribute("cy").Value, CultureInfo.InvariantCulture) * -1,
							ZCoordinate = 0,
							Information = beacon.Attribute("info").Value
						}
					);
				}
				catch (Exception e)
				{
					Debug.WriteLine("Warning: attribute not found while parsing SVG: " + e.Message);
				}
			}
        }

		public static List<Target> GetTargetsByName(string param)
		{
			//var result = SelectMoreRows("select  from Targets WHERE EmployeeParsed LIKE '%" + param + "%'");
			//if (result.Count == 0)
			//{
			//	var keywords = param.Split(' ');
			//	foreach (var key in keywords.Reverse())
			//	{
			//		result = SelectMoreRows("select  from Targets WHERE EmployeeParsed LIKE '%" + key + "%'");
			//		if (result.Count > 0)
			//			return result;
			//	}
			//}
			return Targets;
		}

		public static List<Target> GetTargetsByOffice(string param)
		{
			//	param = param.Replace("bodka", ".").Replace(" ", "");
			//	return SelectMoreRows("select * from Targets WHERE replace( Office, '.', '')='" + param + "'"   }
			return Targets;
		}
    }
}
