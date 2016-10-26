using System;
using System.IO;
using Prototyper.Droid;
using Prototyper.Interface;
using SQLite.Net;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_Droid))]

namespace Prototyper.Droid
{
	public class SQLite_Droid : ISQLite
	{
		public SQLiteConnection GetConnection(String DatabaseFile)
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var path = Path.Combine(documentsPath, DatabaseFile);

			var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var connection = new SQLiteConnection(platform, path);

			return connection;
		}
	}
}