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

		public BluetoothController()
		{
			if (adapter == null)
            {
                adapter = BluetoothAdapter.DefaultAdapter;
				IsAdapterInicialized = true;
            }
		}

        public BluetoothAdapter GetAdapter()
        {
            return adapter;
        }

		public bool IsAdapterInicialized { get; set; }

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