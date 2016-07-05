using UnityEngine;
using System.Collections;
using System.IO;

public class ClockInOut : MonoBehaviour {

	public GameObject inButton, outButton;

	private string txtPath;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		//txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";

		if (PlayerPrefs.GetInt ("In") == 1) {
			inButton.SetActive (false);
		}

		if (PlayerPrefs.GetInt ("Out") == 1) {
			outButton.SetActive (false);
		}
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

		PlayerPrefs.SetInt ("In", 1);
		inButton.SetActive (false);
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

		PlayerPrefs.SetInt ("Out", 1);
		outButton.SetActive (false);
	}
}
