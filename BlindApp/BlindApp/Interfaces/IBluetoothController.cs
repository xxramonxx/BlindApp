using Android.Bluetooth;

namespace BlindApp.Interfaces
{
    public interface IBluetoothController
    {
        BluetoothAdapter GetAdapter();
        bool IsDiscovering();
        bool IsEnabled();

        void Start();
        void Stop();
    }
}
