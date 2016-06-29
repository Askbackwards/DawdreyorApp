using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class ExtraNotes : MonoBehaviour {

	private string txtPath;
	private int timeOut;

	// Use this for initialization
	void Start () {
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
		//txtPath = "C:/Users/nomore/Desktop/" + PlayerPrefs.GetString ("WOID") + "_Info.txt";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddExtraNotes(string notes) {

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
				} else {
					timeOut = i;
				}
			}
		}

		//Write new stuff
		streamW.WriteLine("ExtraNotes," + notes);

		//Write Time out
		streamW.WriteLine(oldText[timeOut - 1]);
		streamW.WriteLine (oldText [timeOut]);

		streamW.Flush ();
		streamW.Close ();

		SceneManager.LoadScene ("NewWorkAuthorizationMenu");

	}
}
