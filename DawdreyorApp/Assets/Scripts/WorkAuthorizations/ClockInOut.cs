using UnityEngine;
using System.Collections;
using System.IO;

public class ClockInOut : MonoBehaviour {

	private string txtPath;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		//txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ClockIn() {
		//Make file if it doesn't exsist
		if (!File.Exists (txtPath))
			File.Create (txtPath ).Dispose ();

		string[] oldText = File.ReadAllLines(txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write old stuff
		if (oldText.Length > 0) {
			for (int i = 0; i < oldText.Length; i++) {
				streamW.WriteLine (oldText [i]);
			}
		}

		string minuteNow = System.DateTime.Now.TimeOfDay.ToString();

		//Write New stuff
		streamW.WriteLine("TimeIn," + minuteNow);
			
		streamW.Flush ();
		streamW.Close ();
	}

	public void ClockOut() {
		//Make file if it doesn't exsist
		if (!File.Exists (txtPath))
			File.Create (txtPath ).Dispose ();

		string[] oldText = File.ReadAllLines(txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write old stuff
		if (oldText.Length > 0) {
			for (int i = 0; i < oldText.Length; i++) {
				streamW.WriteLine (oldText [i]);
			}
		}

		string minuteNow = System.DateTime.Now.TimeOfDay.ToString();

		//Write New stuff
		streamW.WriteLine("TimeOut," + minuteNow);

		streamW.Flush ();
		streamW.Close ();
	}
}
