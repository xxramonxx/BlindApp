using System;
using System.Diagnostics;
using Prototyper.Interface;
using SQLite.Net;
using Xamarin.Forms;
using BlindApp.Model;

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
            Tables.TargetsTable targetsTable = new Tables.TargetsTable(sqlite);
            Tables.PathsTable pathsTable = new Tables.PathsTable(sqlite);
            Tables.DatabaseVersionTable versionTable = new Tables.DatabaseVersionTable(sqlite); // not used so far

            //If not exists, create new 
            if (pointsTable.TableExistence())
            {
                pointsTable.DropTable();
            }
            pointsTable.CreateTable();

            if (targetsTable.TableExistence())
            {
                targetsTable.DropTable();
            }
            targetsTable.CreateTable();

            if (pathsTable.TableExistence())
            {
                pathsTable.DropTable();
            }
            pathsTable.CreateTable();

            if (!versionTable.TableExistence())
			{
				versionTable.CreateTable();
				versionTable.Insert(new Tables.DatabaseVersion
				{
					Version = DatabaseVersion
				});
			}
            PointsTableInitializer.Execute();
            TargetsTableInitializer.Execute();
            PathsTableInitializer.Execute();

            Map.Init();

            //Check if database is up-to-date
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