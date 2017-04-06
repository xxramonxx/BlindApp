using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindApp.Interfaces
{
    public interface ICompass : IDisposable
    {

        bool IsSupported { get; }
        void Start();
        void Stop();

        event EventHandler<CompassChangedEventArgs> CompassChanged;
    }
}
