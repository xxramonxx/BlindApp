using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using BlindApp.Interfaces;
using BlindApp.Droid;
using Java.Util.Logging;
using System.Collections.ObjectModel;

[assembly: Xamarin.Forms.Dependency(typeof(CompassImplementation))]

namespace BlindApp.Droid
{

    public class CompassImplementation : Java.Lang.Object, ICompass, IDisposable, ISensorEventListener
    {
        SensorManager sensorManager;
        Sensor accelerometer;
        Sensor magnetometer;

        float[] lastAccelerometer = new float[3];
        float[] lastMagnetometer = new float[3];

        bool lastAccelerometerSet;
        bool lastMagnetometerSet;

        float[] r = new float[9];
        float[] orientationField = new float[3];

        bool listenting;

        public CompassImplementation()
        {
            Init();
        }

        void Init()
        {

            var ctx = Application.Context;
            if (ctx == null)
            {
                System.Diagnostics.Debug.WriteLine("Context not found, can not start.");
                return;
            }
            if (sensorManager == null)
                sensorManager = ctx.GetSystemService(Context.SensorService) as SensorManager;


            if (accelerometer == null)
                accelerometer = sensorManager?.GetDefaultSensor(SensorType.Accelerometer);

            if (magnetometer == null)
                magnetometer = sensorManager?.GetDefaultSensor(SensorType.MagneticField);

        }

        public bool IsSupported
        {
            get { Init(); return sensorManager != null && accelerometer != null && magnetometer != null; }
        }

        public void Start()
        {
            if (listenting)
            {
                System.Diagnostics.Debug.WriteLine("Already Listening.");
                return;
            }

            if (!IsSupported)
            {
                System.Diagnostics.Debug.WriteLine("Not supported on this device.");
                return;
            }

            listenting = true;

            sensorManager.RegisterListener(this, accelerometer, SensorDelay.Normal);
            sensorManager.RegisterListener(this, magnetometer, SensorDelay.Normal);

        }

        public void Stop()
        {
            if (!listenting)
                return;

            listenting = false;

            if (accelerometer != null)
                sensorManager?.UnregisterListener(this, accelerometer);

            if (magnetometer != null)
                sensorManager?.UnregisterListener(this, magnetometer);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {

        }

        public event EventHandler<CompassChangedEventArgs> CompassChanged;

        protected virtual void OnCompassChanged(CompassChangedEventArgs e) =>
            CompassChanged?.Invoke(this, e);

        public void OnSensorChanged(SensorEvent e)
        {
            lock (locker)
            {
                if (e.Sensor == accelerometer && !lastAccelerometerSet)
                {
                    CopyValues(e.Values, lastAccelerometer);
                    lastAccelerometerSet = true;
                }
                else if (e.Sensor == magnetometer && !lastMagnetometerSet)
                {
                    CopyValues(e.Values, lastMagnetometer);
                    lastMagnetometerSet = true;
                }

                if (lastAccelerometerSet && lastMagnetometerSet)
                {
                    SensorManager.GetRotationMatrix(r, null, lastAccelerometer, lastMagnetometer);
                    SensorManager.GetOrientation(r, orientationField);           

                    var azimut = orientationField[0]; // orientation contains: azimut, pitch and roll
                    var pitch = orientationField[1];
                    var roll = orientationField[2];

                    var azimuthInDegress = (Java.Lang.Math.ToDegrees(azimut) + 360.0) % 360.0;

                    OnCompassChanged(new CompassChangedEventArgs(azimuthInDegress));
                    lastMagnetometerSet = false;
                    lastAccelerometerSet = false;
                }          
            }
        }
        
        private void CopyValues(System.Collections.Generic.IList<float> source, float[] destination)
        {
            for (int i = 0; i < source.Count; ++i)
            {
                destination[i] = source[i];
            }
        }

        object locker = new object();


        /// <summary>
        /// Dispose of class and parent classes
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose up
        /// </summary>
        ~CompassImplementation()
        {
            Dispose(false);
        }
        private bool disposed = false;
        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Stop();
                    sensorManager = null;
                    accelerometer = null;
                    magnetometer = null;
                }

                disposed = true;
            }
        }
    }
}