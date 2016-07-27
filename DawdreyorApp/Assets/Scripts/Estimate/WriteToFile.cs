using UnityEngine;
using System.Collections;
using System.IO;

public class WriteToFile : MonoBehaviour {

	private int position, addnum;
	private string name, txtPath, color;
	private bool addIt;
	private GameObject theObject;

	// Use this for initialization
	void Start () {
		addIt = false;
		txtPath = Application.persistentDataPath + "/" + PlayerPrefs.GetString("EstimateNumber") + "_Estimate.txt";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setColor(string choice) {
		color = choice;
	}

	//store where this thing should be written
	public void storeNumber(int place) {
		position = place;
	}

	//store the name of the object
	public void setName(string theName) {
		name = theName;
	}

	public int getNumber() {
		return position;
	}

	public string getName() {
		return name;
	}

	//Write it
	public void writeIt(string input) {
		//Make file if it doesn't exsist
		if (!File.Exists (txtPath)) {
			File.Create (txtPath).Dispose ();
		}

		string[] oldText = File.ReadAllLines(txtPath);

		//OpenFile
		StreamWriter streamW = new StreamWriter(txtPath);

		//Write stuff
		if (oldText.Length > position) {
			for (int i = 0; i < oldText.Length + addnum; i++) {
				if (i != position) {
					if (addIt) {
						if (oldText [i - 1].Replace(" ", "") != "")
							streamW.WriteLine (oldText [i-1]);
					} else {
						if (oldText [i].Replace(" ", "") != "")
							streamW.WriteLine (oldText [i]);
					}
				} else {
					streamW.WriteLine (name + "," + input);
					addIt = true;
					addnum = 1;
					if (oldText [i].Contains (name)) {
						addIt = false;
						addnum = 0;
					}
				}
			}
		} else {
			for (int i = 0; i < oldText.Length; i++) {
				if (!oldText [i].Contains (name) && oldText[i].Replace(" ", "") != "") {
					streamW.WriteLine (oldText [i]);
					Debug.Log ("Wrote");
				}
			}
			streamW.WriteLine (name + "," + input);
		}
		addnum = 0;
		addIt = false;
		streamW.Flush ();
		streamW.Close ();
	}

	//----------------------------------------------------------------------------------

	public void WriteDropdown(int picked) {
		string[] options = theObject.gameObject.GetComponent<DefineList> ().GetOptions ();
		writeIt(options[picked]);
	}

	public void SetObject(GameObject setObject) {
		theObject = setObject;
	}

	public void ColorWrite() {
		writeIt (color);
	}
}
