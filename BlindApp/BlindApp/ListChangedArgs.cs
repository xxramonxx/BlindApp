using System;

namespace BlindApp
{

    public class ListChangedEventArgs : EventArgs
    {
        public System.Collections.Generic.List<SharedBeacon> Data { get; protected set; }

        public ListChangedEventArgs(System.Collections.Generic.List<SharedBeacon> data)
        {
            Data = data;
        }
    }
}
