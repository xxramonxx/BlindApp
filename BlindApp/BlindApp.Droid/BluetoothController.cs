using Android.Bluetooth;
using BlindApp.Droid;
using Xamarin.Forms;
using BlindApp.Interfaces;

[assembly: Dependency(typeof(BluetoothController))]

namespace BlindApp.Droid
{
    public class BluetoothController : IBluetoothController
    {
        BluetoothAdapter adapter;

        public BluetoothAdapter GetAdapter()
        {
            if (adapter == null)
            {
                adapter = BluetoothAdapter.DefaultAdapter;
            }
            return adapter;
        }

        public bool IsDiscovering()
        {
            return GetAdapter().IsDiscovering;
        }

        public bool IsEnabled()
        {
            return GetAdapter().IsEnabled;
        }

        public void Start()
        {
            GetAdapter().Enable();
        }

        public void Stop()
        {
            GetAdapter().Disable();
        }
    }
}