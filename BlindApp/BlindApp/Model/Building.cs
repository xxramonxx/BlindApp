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
        // points of interets //
        public static List<Target> Targets;
        public static List<SharedBeacon> Beacons;
        public static List<string> Warnings;
        public static List<string> Info;


        public static void Init()
        {
            /* TODO:
             * 
             * metoda ktora sparsuje budovu
             * prechadza konkretnz=y folder reprezentujuci budovu s poschodiami ako svg
             * extract a merge beaconov, cielov, warningov, infos (optional poschodie)
             */

            var assembly = typeof(Building).GetTypeInfo().Assembly;

            var uri = "BlindApp.Sources.fiit.fiit_3.svg";

            Stream stream = assembly.GetManifestResourceStream(uri);
            List<string> rawData = null;
            //     await Task.Factory.StartNew(delegate {
            XDocument doc = XDocument.Load(stream);

            var root = doc?.Root as XElement;

            if (root == null) return;

            Beacons = new List<SharedBeacon>();

            InitBeacons(root);
            InitTargets(root);
            InitWarnings(root);
            InitInfos(root);


        }

        private static void InitBeacons(XElement root)
        {
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
                    Debug.WriteLine("Warning: attribute not found while parsing SVG");
                }    
            }
        }

        private static void InitTargets(XElement root)
        {
            var beaconsGroup = root
             .Elements()
             .Where(e => (e.Name.LocalName == "g" && e.Attribute("id").Value == "targets"));

            var beacons = beaconsGroup.Elements().Elements().Where(e => (e.Name.LocalName == "circle"));

            foreach (var beacon in beacons)
            {
                var a = beacon;
                try
                {
                 //   Targets.Add();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error while adding beacon" + beacon.Attribute("minor").Value);
                }
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
