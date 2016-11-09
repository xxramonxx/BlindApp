using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;

namespace BlindApp.Database.Tables
{
    [Table("Targets")]
    public class Target
    {
        [PrimaryKey, AutoIncrement, Column("ID")]
        public int ID { get; set; }
        public string Employee { get; set; }
        public string Office { get; set; }
        public int Floor { get; set; }
        public int Room { get; set; }
    }

    public class TargetsTable
    {
        SQLiteConnection sqlite;

        public TargetsTable(SQLiteConnection sqlite)
        {
            this.sqlite = sqlite;
        }

        public Boolean TableExistence() { return sqlite.GetTableInfo("Targets").Any(); }

        public void CreateTable() { sqlite.CreateTable<Target>(); }
        public void WipeTable() { sqlite.DeleteAll<Target>(); }
        public void DropTable() { sqlite.DropTable<Target>(); }

        public void Insert(Target project) { sqlite.Insert(project); }
        public void Update(Target project) { sqlite.Update(project); }
        public void Execute(String query) { sqlite.Execute(query); }
        public void Delete(Target project) { sqlite.Delete(project); }

        public Target SelectSingleRow(String query) { return sqlite.Query<Target>(query).FirstOrDefault(); }
        public List<Target> SelectMoreRows(String query) { return sqlite.Query<Target>(query); }

        public Target GetByID(int ID) { return sqlite.Get<Target>(ID); }

        public int GetLastID() { return sqlite.ExecuteScalar<int>("SELECT last_insert_rowid()"); }
    }
}