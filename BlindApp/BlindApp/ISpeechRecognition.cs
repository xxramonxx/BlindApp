﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlindApp
{
    public interface ISpeechRecognition
    {
        void IsListening();

        void Initialize();
        void Start();
        void Stop();
    }
}
