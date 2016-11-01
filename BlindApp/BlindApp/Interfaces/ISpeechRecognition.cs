namespace BlindApp
{
    public interface ISpeechRecognition
    {
        bool IsListening();

        void Initialize();
        void Start();
        void Stop();

  //      event EventHandler ResultsChanged;
    }
}
