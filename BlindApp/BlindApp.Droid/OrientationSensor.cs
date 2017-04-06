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
using Xamarin.Forms;

//[assembly: Xamarin.Forms.Dependency(typeof(OrientationSensor))]
namespace BlindApp.Droid
{
    public class OrientationSensor : Java.Lang.Object, ICompass, ISensorEventListener 
    {

        public static int SENSOR_UNAVAILABLE = -1;

        // references to other objects
        SensorManager m_sm;
        ISensorEventListener m_parent;   // non-null if this class should call its parent after onSensorChanged(...) and onAccuracyChanged(...) notifications
        Activity m_activity;            // current activity for call to getWindowManager().getDefaultDisplay().getRotation()

        // raw inputs from Android sensors
        float m_Norm_Gravity;           // length of raw gravity vector received in onSensorChanged(...).  NB: should be about 10
        float[] m_NormGravityVector;    // Normalised gravity vector, (i.e. length of this vector is 1), which points straight up into space
        float m_Norm_MagField;          // length of raw magnetic field vector received in onSensorChanged(...). 
        float[] m_NormMagFieldValues;   // Normalised magnetic field vector, (i.e. length of this vector is 1)

        // accuracy specifications. SENSOR_UNAVAILABLE if unknown, otherwise SensorManager.SENSOR_STATUS_UNRELIABLE, SENSOR_STATUS_ACCURACY_LOW, SENSOR_STATUS_ACCURACY_MEDIUM or SENSOR_STATUS_ACCURACY_HIGH
        int m_GravityAccuracy;          // accuracy of gravity sensor
        int m_MagneticFieldAccuracy;    // accuracy of magnetic field sensor

        // values calculated once gravity and magnetic field vectors are available
        float[] m_NormEastVector;       // normalised cross product of raw gravity vector with magnetic field values, points east
        float[] m_NormNorthVector;      // Normalised vector pointing to magnetic north
        bool m_OrientationOK;        // set true if m_azimuth_radians and m_pitch_radians have successfully been calculated following a call to onSensorChanged(...)
        float m_azimuth_radians;        // angle of the device from magnetic north
        float m_pitch_radians;          // tilt angle of the device from the horizontal.  m_pitch_radians = 0 if the device if flat, m_pitch_radians = Math.PI/2 means the device is upright.
        float m_pitch_axis_radians;     // angle which defines the axis for the rotation m_pitch_radians

        public event EventHandler<CompassChangedEventArgs> CompassChanged;

        protected virtual void OnCompassChanged(CompassChangedEventArgs e) =>
         CompassChanged?.Invoke(this, e);

        public bool IsSupported
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public OrientationSensor()
        {
            var ctx = Android.App.Application.Context;

            m_sm = ctx.GetSystemService(Context.SensorService) as SensorManager;
            m_activity = null;
            m_NormGravityVector = m_NormMagFieldValues = null;
            m_NormEastVector = new float[3];
            m_NormNorthVector = new float[3];
            m_OrientationOK = false;
        }

        public void Start()
        {
            m_NormGravityVector = new float[3];
            m_NormMagFieldValues = new float[3];
            m_OrientationOK = false;
            int count = 0;
            Sensor SensorGravity = m_sm.GetDefaultSensor(SensorType.Gravity);
            if (SensorGravity != null)
            {
                m_sm.RegisterListener(this, SensorGravity, SensorDelay.Fastest);
                m_GravityAccuracy = (int) SensorStatus.AccuracyHigh;
                count++;
            }
            else
            {
                m_GravityAccuracy = SENSOR_UNAVAILABLE;
            }
            Sensor SensorMagField = m_sm.GetDefaultSensor(SensorType.MagneticField);
            if (SensorMagField != null)
            {
                m_sm.RegisterListener(this, SensorMagField, SensorDelay.Fastest);
                m_MagneticFieldAccuracy = (int) SensorStatus.AccuracyHigh;
                count++;
            }
            else
            {
                m_MagneticFieldAccuracy = SENSOR_UNAVAILABLE;
            }
        }

        public void Stop()
        {
            m_activity = null;
            m_NormGravityVector = m_NormMagFieldValues = null;
            m_OrientationOK = false;
            m_sm.UnregisterListener(this);
        }

        public void OnSensorChanged(SensorEvent evnt)
        {
            var SensorType = evnt.Sensor.Type;
            switch (SensorType)
            {
                case SensorType.Gravity:
                    if (m_NormGravityVector == null) m_NormGravityVector = new float[3];
                    CopyValues(evnt.Values, m_NormGravityVector);
                    m_Norm_Gravity = (float)Math.Sqrt(m_NormGravityVector[0] * m_NormGravityVector[0] + m_NormGravityVector[1] * m_NormGravityVector[1] + m_NormGravityVector[2] * m_NormGravityVector[2]);
                    for (int i = 0; i < m_NormGravityVector.Length; i++) m_NormGravityVector[i] /= m_Norm_Gravity;
                    break;
                case SensorType.MagneticField:
                    if (m_NormMagFieldValues == null) m_NormMagFieldValues = new float[3];
                    CopyValues(evnt.Values, m_NormMagFieldValues);
                    m_Norm_MagField = (float)Math.Sqrt(m_NormMagFieldValues[0] * m_NormMagFieldValues[0] + m_NormMagFieldValues[1] * m_NormMagFieldValues[1] + m_NormMagFieldValues[2] * m_NormMagFieldValues[2]);
                    for (int i = 0; i < m_NormMagFieldValues.Length; i++) m_NormMagFieldValues[i] /= m_Norm_MagField;
                    break;
            }
            if (m_NormGravityVector != null && m_NormMagFieldValues != null)
            {
                // first calculate the horizontal vector that points due east
                float East_x = m_NormMagFieldValues[1] * m_NormGravityVector[2] - m_NormMagFieldValues[2] * m_NormGravityVector[1];
                float East_y = m_NormMagFieldValues[2] * m_NormGravityVector[0] - m_NormMagFieldValues[0] * m_NormGravityVector[2];
                float East_z = m_NormMagFieldValues[0] * m_NormGravityVector[1] - m_NormMagFieldValues[1] * m_NormGravityVector[0];
                float norm_East = (float)Math.Sqrt(East_x * East_x + East_y * East_y + East_z * East_z);
                if (m_Norm_Gravity * m_Norm_MagField * norm_East < 0.1f)
                {  // Typical values are  > 100.
                    m_OrientationOK = false; // device is close to free fall (or in space?), or close to magnetic north pole.
                }
                else
                {
                    m_NormEastVector[0] = East_x / norm_East; m_NormEastVector[1] = East_y / norm_East; m_NormEastVector[2] = East_z / norm_East;

                    // next calculate the horizontal vector that points due north                   
                    float M_dot_G = (m_NormGravityVector[0] * m_NormMagFieldValues[0] + m_NormGravityVector[1] * m_NormMagFieldValues[1] + m_NormGravityVector[2] * m_NormMagFieldValues[2]);
                    float North_x = m_NormMagFieldValues[0] - m_NormGravityVector[0] * M_dot_G;
                    float North_y = m_NormMagFieldValues[1] - m_NormGravityVector[1] * M_dot_G;
                    float North_z = m_NormMagFieldValues[2] - m_NormGravityVector[2] * M_dot_G;
                    float norm_North = (float)Math.Sqrt(North_x * North_x + North_y * North_y + North_z * North_z);
                    m_NormNorthVector[0] = North_x / norm_North; m_NormNorthVector[1] = North_y / norm_North; m_NormNorthVector[2] = North_z / norm_North;

                    // take account of screen rotation away from its natural rotation
                    IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                    var rotation = windowManager.DefaultDisplay.Rotation;

                    float screen_adjustment = 0;
                    switch (rotation)
                    {
                        case SurfaceOrientation.Rotation0: screen_adjustment = 0; break;
                        case SurfaceOrientation.Rotation90: screen_adjustment = (float)Math.PI / 2; break;
                        case SurfaceOrientation.Rotation180: screen_adjustment = (float)Math.PI; break;
                        case SurfaceOrientation.Rotation270: screen_adjustment = 3 * (float)Math.PI / 2; break;
                    }
                    // NB: the rotation matrix has now effectively been calculated. It consists of the three vectors m_NormEastVector[], m_NormNorthVector[] and m_NormGravityVector[]

                    // calculate all the required angles from the rotation matrix
                    // NB: see http://math.stackexchange.com/questions/381649/whats-the-best-3d-angular-co-ordinate-system-for-working-with-smartfone-apps
                    float sin = m_NormEastVector[1] - m_NormNorthVector[0], cos = m_NormEastVector[0] + m_NormNorthVector[1];
                    m_azimuth_radians = (float)(sin != 0 && cos != 0 ? Math.Atan2(sin, cos) : 0);
                    m_pitch_radians = (float)Math.Acos(m_NormGravityVector[2]);
                    sin = -m_NormEastVector[1] - m_NormNorthVector[0]; cos = m_NormEastVector[0] - m_NormNorthVector[1];
                    float aximuth_plus_two_pitch_axis_radians = (float)(sin != 0 && cos != 0 ? Math.Atan2(sin, cos) : 0);
                    m_pitch_axis_radians = (float)(aximuth_plus_two_pitch_axis_radians - m_azimuth_radians) / 2;
                    m_azimuth_radians += screen_adjustment;
                    m_pitch_axis_radians += screen_adjustment;
                    m_OrientationOK = true;
                }
            }
            if (m_parent != null) m_parent.OnSensorChanged(evnt);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            var SensorType = sensor.Type;
            switch (SensorType)
            {
                case SensorType.Gravity: m_GravityAccuracy = (int) accuracy; break;
                case SensorType.MagneticField: m_MagneticFieldAccuracy = (int) accuracy; break;
            }
            if (m_parent != null) m_parent.OnAccuracyChanged(sensor, accuracy);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void CopyValues(System.Collections.Generic.IList<float> source, float[] destination)
        {
            for (int i = 0; i < source.Count; ++i)
            {
                destination[i] = source[i];
            }
        }
    }
}