using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BlindApp.Droid;
using BlindApp.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(ThreadManager))]
namespace BlindApp.Droid
{
    public class ThreadManager : IThreadManager
    {
		List<CThread> Threads = new List<CThread>();

		public Action ThreadDelegate { get; set; }

		public int CreateNewThread()
		{
			var pid = Threads.Count + 1;
			var thread = new CThread(ThreadDelegate, pid);

			Threads.Add(thread);

			return pid;
		}

		public void Start(int PID)
		{
			Threads.Single(thread => thread.PID == PID).Start();
		}

		public void Stop(int PID)
		{
			Threads.Single(thread => thread.PID == PID).Stop();
		}

		class CThread
		{
			public int PID { get; set; }
			Thread Thread { get; set; }

			public CThread(Action threadDelegate, Int32 pid)
			{
				Thread = new Thread(new ThreadStart(delegate
				{
					threadDelegate?.Invoke();
				}));
				PID = pid;
			}

			public void Start()
			{
				Thread.Start();
			}

			public void Stop()
			{
				Thread.Abort();
			}
		}
    }
}