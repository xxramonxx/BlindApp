using System;

namespace BlindApp
{
    public interface IAltBeaconService
    {
        void InitializeService();
        void StartMonitoring();
        void StartRanging();

        void StopMonitoring();
        void StopRanging();

        event EventHandler<ListChangedEventArgs> ListChanged;
    }
}