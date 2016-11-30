﻿using BlindApp.Database.Tables;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BlindApp.Database
{
    static class PointsTableInitializer
    {
        public static void Execute()
        {
            PointsTable pointsTable = new PointsTable(Initialize.DatabaseConnect());
            var assembly = typeof(PointsTableInitializer).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            Stream stream = assembly.GetManifestResourceStream("BlindApp.Sources.beacons_data.csv");

            string text = "";
            using (var reader = new StreamReader(stream))
            {
                text = reader.ReadToEnd();
            }
            var lines = Regex.Replace(text, @"[^\S\n]+", "").Split('\n');
         
            foreach (var entry in lines)
            {
                var attributes = entry.Split(';');
                pointsTable.Insert(
                    new Point {
                        UID = attributes[0],
                        Major = attributes[1],
                        Minor = attributes[2],
                        XCoordinate = attributes[3],
                        YCoordinate = attributes[4],
                        ZCoordinate = attributes[5],
                        Floor = attributes[6]
                    }
                );
            }
        }
    }
}