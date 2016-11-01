using System;
using System.Diagnostics;
using Prototyper.Interface;
using SQLite.Net;
using Xamarin.Forms;

namespace BlindApp.Database
{
	public class Initialize
	{
		SQLiteConnection sqlite;
		static String DatabaseFile = "PrototyperSQLite.db";
		static int DatabaseVersion = 1;

		public Initialize()
		{
			sqlite = DependencyService.Get<ISQLite>().GetConnection(DatabaseFile);

            //Tables initialize
            Tables.PointsTable pointsTable = new Tables.PointsTable(sqlite);
			Tables.DatabaseVersionTable versionTable = new Tables.DatabaseVersionTable(sqlite); // not used so far

            pointsTable.DropTable();

            //If not exists, create new 
            if (!pointsTable.TableExistence())
                pointsTable.CreateTable();

			if (!versionTable.TableExistence())
			{
				versionTable.CreateTable();
				versionTable.Insert(new Tables.DatabaseVersion
				{
					Version = DatabaseVersion
				});
			}
            PointsTableInitializer.Execute();
            //Check if database is actual
            UpgradeDatabaseCheck(versionTable.ActualVersion());
		}

		private void UpgradeDatabaseCheck(int version)
		{
			switch (version)
			{
				case 1:
					Debug.WriteLine("Database version is up-to-date.");
                    break;
                case 2:
                    Debug.WriteLine("Database version is up-to-date.");
                    break;
            }
		}

		public static SQLiteConnection DatabaseConnect() { return DependencyService.Get<ISQLite>().GetConnection(DatabaseFile); }
	}
}