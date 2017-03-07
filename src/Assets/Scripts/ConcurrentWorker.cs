using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public abstract class ConcurrentWorker : MonoBehaviour {

	private Thread t;
	private bool interrupted;
	private string lockObject = "";

	void Start() {
		init();
		t = new Thread(new ThreadStart(outerLoop));
		t.Start();
	}

	public void interrupt() {
		lock (lockObject) {
			interrupted = true;
		}
	}

	void OnApplicationQuit() {
		interrupt();
	}

	private void outerLoop() {
		while (true) {
			bool stop = false;
			lock (lockObject) {
				stop = interrupted;
			}

			if (stop)
				break;

			bool keepGoing = loop(stop);
			if (!keepGoing)
				interrupt();
		}
		print("ConcurrentWorker stopped.");
	}

	protected abstract void init();

	protected abstract bool loop(bool interrupted);

}
