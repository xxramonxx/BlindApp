using System;
using System.IO;
using BlindApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Files))]
namespace BlindApp.Droid
{
	public class Files : IFiles
	{
		public void SaveFile(string filename, string text)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, filename);
			File.WriteAllText(filePath, text);
		}
		public string LoadFile(string filename)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var filePath = Path.Combine(documentsPath, filename);

			if (File.Exists(filePath))
				return File.ReadAllText(filePath);

			return "";
		}
	}
}