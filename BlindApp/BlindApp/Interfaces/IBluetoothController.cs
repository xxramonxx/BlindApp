namespace BlindApp.Interfaces
{
    public interface IBluetoothController
    {
        bool IsDiscovering();
        bool IsEnabled();

        void Start();
        void Stop();
    }
}
