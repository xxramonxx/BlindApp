using System;
using AltBeaconOrg.BoundBeacon;
using BlindApp.Droid.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Android.App;
using System.Diagnostics;
using Android.OS;

[assembly: Xamarin.Forms.Dependency(typeof(AltBeaconService))]

namespace BlindApp.Droid.Services
{
    public class AltBeaconService : Java.Lang.Object, IAltBeaconService
    {
        private const long SCAN_INTERVAL = 500;

        private readonly MonitorNotifier _monitorNotifier;
        private readonly RangeNotifier _rangeNotifier;

        private BeaconManager _beaconManager;
        
        public event EventHandler<ListChangedEventArgs> ListChanged;

        Region _tagRegion;
        Region _emptyRegion;
   
        public AltBeaconService()
        {
            _monitorNotifier = new MonitorNotifier();
            _rangeNotifier = new RangeNotifier();
        }

        public BeaconManager BeaconManagerImpl
        {
            get
            {
                if (_beaconManager == null)
                {
                    _beaconManager = InitializeBeaconManager();
                }
                return _beaconManager;
            }
        }

        public void InitializeService()
        {
            _beaconManager = InitializeBeaconManager();
        }

        private BeaconManager InitializeBeaconManager()
        {
            // Enable the BeaconManager 
            BeaconManager bm = BeaconManager.GetInstanceForApplication(Xamarin.Forms.Forms.Context);

            var iBeaconParser = new BeaconParser();
            //	Estimote > 2013
            iBeaconParser.SetBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24");
            bm.BeaconParsers.Add(iBeaconParser);

            _monitorNotifier.EnterRegionComplete += EnteredRegion;
            _monitorNotifier.ExitRegionComplete += ExitedRegion;
            _monitorNotifier.DetermineStateForRegionComplete += DeterminedStateForRegionComplete;
            _rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

            _tagRegion = new Region("myUniqueBeaconId", Identifier.Parse("E4C8A4FC-F68B-470D-959F-29382AF72CE7"), null, null);
            _tagRegion = new Region("myUniqueBeaconId", Identifier.Parse("B9407F30-F5F8-466E-AFF9-25556B57FE6D"), null, null);
            _emptyRegion = new Region("myEmptyBeaconId", null, null, null);

            bm.SetBackgroundMode(false);
            bm.Bind((IBeaconConsumer) Xamarin.Forms.Forms.Context);

            return bm;
        }

        public void StartMonitoring()
        {
            BeaconManagerImpl.SetForegroundBetweenScanPeriod(SCAN_INTERVAL);

            BeaconManagerImpl.SetMonitorNotifier(_monitorNotifier);
            _beaconManager.StartMonitoringBeaconsInRegion(_tagRegion);
            _beaconManager.StartMonitoringBeaconsInRegion(_emptyRegion);
        }

        public void StartRanging()
        {
            BeaconManagerImpl.SetForegroundBetweenScanPeriod(SCAN_INTERVAL);

            BeaconManagerImpl.SetRangeNotifier(_rangeNotifier);
            _beaconManager.StartRangingBeaconsInRegion(_tagRegion);
            _beaconManager.StartRangingBeaconsInRegion(_emptyRegion);
        }

        public void StopMonitoring()
        {
            _beaconManager.StopMonitoringBeaconsInRegion(_tagRegion);
            _beaconManager.StopMonitoringBeaconsInRegion(_emptyRegion);
        }

        public void StopRanging()
        {
            _beaconManager.StopRangingBeaconsInRegion(_tagRegion);
            _beaconManager.StopRangingBeaconsInRegion(_emptyRegion);
        }

        private void DeterminedStateForRegionComplete(object sender, MonitorEventArgs e)
        {
            Console.WriteLine("DeterminedStateForRegionComplete");
        }

        private void ExitedRegion(object sender, MonitorEventArgs e)
        {
            Console.WriteLine("ExitedRegion");
        }

        private void EnteredRegion(object sender, MonitorEventArgs e)
        {
            Console.WriteLine("EnteredRegion");
        }

        async void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {
            if (e.Beacons.Count > 0)
            {
                await UpdateData(e.Beacons.ToList());
            }
        }

        private async Task UpdateData(List<Beacon> beacons)
        {
            await Task.Run(() =>
            {
                var handler = ListChanged;
                if (handler != null)
                {
                    ((Activity)Xamarin.Forms.Forms.Context).RunOnUiThread(() =>
                    {
                        // transform data from service to Shared code
                        var data = new List<SharedBeacon>();
                        beacons.ForEach(b =>
                        {
                            data.Add(new SharedBeacon
                            {
                                UID = b.Id1.ToString(),
                                Major = b.Id1.ToString(),
                                Minor = b.Id3.ToString(),
                                Distance = b.Distance,
                                Rssi = b.Rssi,
                                MAC = b.BluetoothAddress.ToString()
                            });
                        });
                        handler(this, new ListChangedEventArgs(data));
                    });
                }
            });
        }
    }
}