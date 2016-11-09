
namespace BlindApp.Model
{
    class Target
    {
        public string Employee { get; set; }
        public int Room { get; set; }
        public int Floor { get; set; }
        public string Office
        {
            get { return Floor + "." + Room; }
        }
        public string Endpoint { get; set; }
    }
}
