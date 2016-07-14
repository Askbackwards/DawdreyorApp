using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtraNotes : MonoBehaviour {

	public GameObject extraNotesInput;

	private string txtPath, notes;
	private int timeOut, timeIn;

	// Use this for initialization
	void Start () {
		notes = "";
		timeIn = -1;
		timeOut = -1;
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		//txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";

		if (File.Exists (txtPath)) {
			string[] oldText = File.ReadAllLines (txtPath);
			for (int i = 0; i < oldText.Length; i++) {
				if (oldText[i].Contains ("Extra")) {
					string[] split = oldText[i].Split(",".ToCharArray());
					extraNotesInput.GetComponentInChildren<Text> ().text = split[1];
					notes = split [1];
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddExtraNotes(string NewNotes) {

		if (NewNotes != "") {
			notes = NewNotes;
		}

		//Make file if it doesn't exsist
		if (!File.Exists (txtPath))
			File.Create (txtPath ).Dispose ();

		string[] oldText = File.ReadAllLines(txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write old stuff
		if (oldText.Length > 0) {
			for (int i = 0; i < oldText.Length; i++) {
				if (!oldText [i].Contains ("TimeOut") && !oldText [i].Contains ("TimeIn")) {
					streamW.WriteLine (oldText [i]);
				} else if (oldText [i].Contains ("TimeOut")) {
					timeOut = i;
				} else if (oldText [i].Contains ("TimeIn")) {
					timeIn = i;
				}
			}
		}

		//Write new stuff
		streamW.WriteLine("ExtraNotes," + notes);

		//Write Time out
		if (timeIn != -1)
			streamW.WriteLine(oldText[timeIn]);
		if (timeOut != -1)
			streamW.WriteLine (oldText [timeOut]);

		streamW.Flush ();
		streamW.Close ();

		SceneManager.LoadScene ("NewWorkAuthorizationMenu");

	}
}
