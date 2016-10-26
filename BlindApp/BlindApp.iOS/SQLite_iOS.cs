using System;
using System.IO;
using Prototyper.Interface;
using Prototyper.iOS;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]

namespace Prototyper.iOS
{
	public class SQLite_iOS : ISQLite
	{
		public SQLiteConnection GetConnection(String DatabaseFile)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(documentsPath, "..", "Library");
			var path = Path.Combine(libraryPath, DatabaseFile);

			var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
			var connection = new SQLiteConnection(platform, path);

			return connection;
		}
	}
}