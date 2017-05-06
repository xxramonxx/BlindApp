using System;
namespace BlindApp.Interfaces
{
    public interface IThreadManager
    {
        Action ThreadDelegate { get; set; }

		int CreateNewThread();
		void Start(int PID);
		void Stop(int PID);
    }
}
