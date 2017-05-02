using System;
using System.Threading;
using BlindApp.Droid;
using BlindApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(CustomThread))]
namespace BlindApp.Droid
{
    public class CustomThread : ICustomThread
    {
        private Action _thread { get; set; }
        public Action Thread
        {
            get
            {
                return _thread;
            }
            set
            {
                _thread = value;

                _RunInThread();
            }
        }

        private void _RunInThread() 
        {
            new Thread(new ThreadStart(delegate {
                Thread?.Invoke();
            })).Start();
        }
    }
}