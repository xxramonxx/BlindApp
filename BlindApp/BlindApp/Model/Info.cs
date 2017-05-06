using System;
namespace BlindApp
{
	public class Info :  MapPoint, IComparable<Info>
	{
		public string Information { get; set; }

		public int CompareTo(Info that)
		{
			//if (string.Compare(this.Employee, that.Employee, StringComparison.CurrentCulture) < 0) return -1;
			//if (this.Employee == that.Employee) return 0;
			return 1;
		}
	}
}
