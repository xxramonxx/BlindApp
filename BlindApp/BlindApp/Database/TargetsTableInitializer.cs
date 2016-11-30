using BlindApp.Database.Tables;
using BlindApp.Model;
using System.IO;
using System.Reflection;

namespace BlindApp.Database
{
    static class TargetsTableInitializer
    {
        public static void Execute()
        {
            TargetsTable pointsTable = new TargetsTable(Initialize.DatabaseConnect());
            var assembly = typeof(TargetsTableInitializer).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
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
                pointsTable.Insert(
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
    }
}
