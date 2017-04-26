using BlindApp.Database.Tables;
using System;
using System.Reflection;
using System.Text;
using System.Globalization;
using Java.Text;
using System.IO;
using Java.Lang;

namespace BlindApp.Database
{
    static class PathsTableInitializer
    {
        public static void Execute()
        {
            PathsTable pathsTable = new PathsTable(Initializer.DatabaseConnect());
            var assembly = typeof(PathsTableInitializer).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            Stream stream = assembly.GetManifestResourceStream("BlindApp.Sources.beacons_connections.csv");

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
                pathsTable.Insert(
                    new Tables.Path {
                        Start = attributes[0],
                        End = attributes[1],
     //                   Distance = Float.ParseFloat(attributes[2]), 
                    }
                );
            }
        }  
    }
}
