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

        private BluetoothAdapter getAdapter()
        {
            if (adapter == null)
            {
                adapter = BluetoothAdapter.DefaultAdapter;
            }
            return adapter;
        }

        public bool IsDiscovering()
        {
            return getAdapter().IsDiscovering;
        }

        public bool IsEnabled()
        {
            return getAdapter().IsEnabled;
        }

        public void Start()
        {
            getAdapter().Enable();
        }

        public void Stop()
        {
            getAdapter().Disable();
        }
    }
}