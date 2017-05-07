using System;
using Xamarin.Forms;

namespace BlindApp.Model
{
    public class Target : MapPoint,IComparable<Target>
    {
        public string Employee { get; set; }
        public string EmployeeParsed { get; set; }
        public string FirstName
        {
            get 
            {
                return EmployeeParsed.Split(' ')[0];
            }
        }
        public string LastName
        {
            get
            {
                var result = EmployeeParsed.Split(' ');
                return result[result.Length];
            }
        }
               
        public string Office { get; set; }

		public int Room { get; set; }
        public int Minor { get; set; }


        public int CompareTo(Target that)
        {
            if (string.Compare(this.Employee, that.Employee, StringComparison.CurrentCulture) < 0) return -1;
            if (this.Employee == that.Employee) return 0;
            return 1;
        }
    }
}
