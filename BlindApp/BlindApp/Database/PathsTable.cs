using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;

namespace BlindApp.Database.Tables
{
    [Table("Paths")]
    public class Path
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public float Distance { get; set; }
    }

    public class PathsTable
    {
        SQLiteConnection sqlite;

        public PathsTable(SQLiteConnection sqlite)
        {
            this.sqlite = sqlite;
        }

        public bool TableExistence() { return sqlite.GetTableInfo("Paths").Any(); }

        public void CreateTable() { sqlite.CreateTable<Path>(); }
        public void WipeTable() { sqlite.DeleteAll<Path>(); }
        public void DropTable() { sqlite.DropTable<Path>(); }

        public void Insert(Path project) { sqlite.Insert(project); }
        public void Update(Path project) { sqlite.Update(project); }
        public void Execute(string query) { sqlite.Execute(query); }
        public void Delete(Path project) { sqlite.Delete(project); }

        public Path SelectSingleRow(string query) { return sqlite.Query<Path>(query).FirstOrDefault(); }
        public List<Path> SelectMoreRows(string query) { return sqlite.Query<Path>(query); }

        public Path GetByID(int ID) { return sqlite.Get<Path>(ID); }

        public int GetLastID() { return sqlite.ExecuteScalar<int>("SELECT last_insert_rowid()"); }
    }
}