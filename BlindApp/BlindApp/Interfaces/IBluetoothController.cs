
namespace BlindApp.Interfaces
{
    public interface IBluetoothController
    {
		//BluetoothAdapter GetAdapter();
		bool IsAdapterInicialized { get; set; }
        bool IsDiscovering();
        bool IsEnabled();

        void Start();
        void Stop();
    }
}
